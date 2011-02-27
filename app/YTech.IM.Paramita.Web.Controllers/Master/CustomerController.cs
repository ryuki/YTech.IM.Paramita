using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Web.Controllers.Master
{
    [HandleError]
    public class CustomerController : Controller
    {
        //public CustomerController() : this(new MSupplierRepository(), new RefAddressRepository())
        //{}

        private readonly IMCustomerRepository _mCustomerRepository;
        private readonly IRefPersonRepository _refPersonRepository;
        private readonly IRefAddressRepository _refAddressRepository;
        public CustomerController(IMCustomerRepository mCustomerRepository, IRefPersonRepository refPersonRepository, IRefAddressRepository refAddressRepository)
        {
            Check.Require(mCustomerRepository != null, "mCustomerRepository may not be null");
            Check.Require(refPersonRepository != null, "refPersonRepository may not be null");
            Check.Require(refAddressRepository != null, "refAddressRepository may not be null");

            this._mCustomerRepository = mCustomerRepository;
            this._refPersonRepository = refPersonRepository;
            this._refAddressRepository = refAddressRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var custs = _mCustomerRepository.GetPagedCustomerList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from cust in custs
                    select new
                    {
                        i = cust.Id.ToString(),
                        cell = new string[] {
                            cust.Id, 
                            cust.PersonId != null ?    cust.PersonId.PersonFirstName : null, 
                          cust.PersonId != null ?    cust.PersonId.PersonLastName : null, 
                          cust.PersonId != null ?    cust.PersonId.PersonName : null,  
                          cust.AddressId != null?  cust.AddressId.AddressLine1 : null,
                          cust.AddressId != null?  cust.AddressId.AddressLine2 : null,
                          cust.AddressId != null?  cust.AddressId.AddressLine3 : null,
                          cust.AddressId != null?  cust.AddressId.AddressPhone : null,
                          cust.AddressId != null?  cust.AddressId.AddressCity : null,
                            cust.CustomerDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [Transaction]
        public ActionResult Insert(MCustomer viewModel, FormCollection formCollection)
        {
            RefAddress address = new RefAddress();
            TransferFormValuesTo(address, formCollection);
            address.SetAssignedIdTo(Guid.NewGuid().ToString());
            address.CreatedDate = DateTime.Now;
            address.CreatedBy = User.Identity.Name;
            address.DataStatus = EnumDataStatus.New.ToString();
            _refAddressRepository.Save(address);

            RefPerson person = new RefPerson();
            TransferFormValuesTo(person, formCollection);
            person.SetAssignedIdTo(Guid.NewGuid().ToString());
            person.CreatedDate = DateTime.Now;
            person.CreatedBy = User.Identity.Name;
            person.DataStatus = EnumDataStatus.New.ToString();
            _refPersonRepository.Save(person);

            MCustomer customer = new MCustomer();
            TransferFormValuesTo(customer, viewModel);
            customer.SetAssignedIdTo(viewModel.Id);
            customer.CreatedDate = DateTime.Now;
            customer.CreatedBy = User.Identity.Name;
            customer.DataStatus = EnumDataStatus.New.ToString();

            customer.AddressId = address;
            customer.PersonId = person;

            _mCustomerRepository.Save(customer);

            try
            {
                _mCustomerRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCustomerRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MCustomer viewModel, FormCollection formCollection)
        {
            MCustomer customer = _mCustomerRepository.Get(viewModel.Id);
            TransferFormValuesTo(customer, viewModel);
            customer.ModifiedDate = DateTime.Now;
            customer.ModifiedBy = User.Identity.Name;
            customer.DataStatus = EnumDataStatus.Updated.ToString();

            RefAddress address = customer.AddressId;
            TransferFormValuesTo(address, formCollection);
            address.ModifiedDate = DateTime.Now;
            address.ModifiedBy = User.Identity.Name;
            address.DataStatus = EnumDataStatus.Updated.ToString(); 

            RefPerson person = customer.PersonId;
            TransferFormValuesTo(person, formCollection);
            person.ModifiedDate = DateTime.Now;
            person.ModifiedBy = User.Identity.Name;
            person.DataStatus = EnumDataStatus.Updated.ToString();

            _mCustomerRepository.Update(customer);

            try
            {
                _mCustomerRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCustomerRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(MCustomer viewModel, FormCollection formCollection)
        {
            MCustomer customer = _mCustomerRepository.Get(viewModel.Id);

            if (customer != null)
            {
                _mCustomerRepository.Delete(customer);
                _refAddressRepository.Delete(customer.AddressId);
            }

            try
            {
                _mCustomerRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCustomerRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void TransferFormValuesTo(RefAddress address, FormCollection formCollection)
        {
            address.AddressLine1 = formCollection["AddressLine1"];
            address.AddressLine2 = formCollection["AddressLine2"];
            address.AddressLine3 = formCollection["AddressLine3"];
            address.AddressPhone = formCollection["AddressPhone"];
            address.AddressCity = formCollection["AddressCity"];

        }

        private static void TransferFormValuesTo(MCustomer customer, MCustomer customerForm)
        {
            customer.CustomerDesc = customerForm.CustomerDesc; 
        }

        private void TransferFormValuesTo(RefPerson person, FormCollection formCollection)
        {
            person.PersonFirstName = formCollection["PersonFirstName"];
            person.PersonLastName = formCollection["PersonLastName"];
            //person.PersonGender = formCollection["PersonGender"];
            //if (formCollection["PersonDob"] != null)
            //    person.PersonDob = Convert.ToDateTime(formCollection["PersonDob"]);
            //person.PersonPob = formCollection["PersonPob"];
            //person.PersonPhone = formCollection["PersonPhone"];
            //person.PersonMobile = formCollection["PersonMobile"];
            //person.PersonEmail = formCollection["PersonEmail"];
            //person.PersonReligion = formCollection["PersonReligion"];
            //person.PersonRace = formCollection["PersonRace"];
            //person.PersonIdCardType = formCollection["PersonIdCardType"];
            //person.PersonIdCardNo = formCollection["PersonIdCardNo"];
        }
    }
}
