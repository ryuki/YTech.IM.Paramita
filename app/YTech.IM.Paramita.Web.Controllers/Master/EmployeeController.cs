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
    public class EmployeeController : Controller
    {
        public EmployeeController() : this(new MEmployeeRepository(), new RefAddressRepository(), new RefPersonRepository(),new MDepartmentRepository())
        {}
        private readonly IMEmployeeRepository _mEmployeeRepository;
        private readonly IRefAddressRepository _refAddressRepository;
        private readonly IRefPersonRepository _refPersonRepository;
        private readonly IMDepartmentRepository _mDepartmentRepository;
        public EmployeeController(IMEmployeeRepository mEmployeeRepository, IRefAddressRepository refAddressRepository, IRefPersonRepository refPersonRepository,IMDepartmentRepository mDepartmentRepository)
        {
            Check.Require(mEmployeeRepository != null, "mEmployeeRepository may not be null");
            Check.Require(refAddressRepository != null, "refAddressRepository may not be null");
            Check.Require(refPersonRepository != null, "refPersonRepository may not be null");
            Check.Require(mDepartmentRepository != null, "mDepartmentRepository may not be null");

            this._mEmployeeRepository = mEmployeeRepository;
            this._refAddressRepository = refAddressRepository;
            this._refPersonRepository = refPersonRepository;
            this._mDepartmentRepository = mDepartmentRepository;
        }


        public ActionResult Index()
        {
            return View();
        }


        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var itemCats = _mEmployeeRepository.GetPagedEmployeeList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from itemCat in itemCats
                    select new
                    {
                        i = itemCat.Id.ToString(),
                        cell = new string[] {
                            itemCat.Id, 
                        itemCat.PersonId != null ?    itemCat.PersonId.PersonFirstName : null, 
                          itemCat.PersonId != null ?    itemCat.PersonId.PersonLastName : null, 
                         itemCat.EmployeeStatus, 
                          itemCat.PersonId != null ?    itemCat.PersonId.PersonGender : null, 
                        itemCat.DepartmentId != null?  itemCat.DepartmentId.Id:null, 
                        itemCat.DepartmentId != null?  itemCat.DepartmentId.DepartmentName:null, 
                          //itemCat.PersonId != null ?    itemCat.PersonId.PersonDob.Value.ToString(Helper.CommonHelper.DateFormat) : null, 
                          //itemCat.PersonId != null ?    itemCat.PersonId.PersonPob : null, 
                          //itemCat.PersonId != null ?    itemCat.PersonId.PersonPhone : null, 
                          //itemCat.PersonId != null ?    itemCat.PersonId.PersonMobile : null, 
                          //itemCat.PersonId != null ?    itemCat.PersonId.PersonEmail : null, 
                          //itemCat.PersonId != null ?    itemCat.PersonId.PersonReligion : null, 
                          //itemCat.PersonId != null ?    itemCat.PersonId.PersonRace : null, 
                          //itemCat.PersonId != null ?    itemCat.PersonId.PersonIdCardType : null, 
                          //itemCat.PersonId != null ?    itemCat.PersonId.PersonIdCardNo : null, 
                         itemCat.EmployeeDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MEmployee viewModel, FormCollection formCollection)
        {
            RefPerson person = new RefPerson();
            TransferFormValuesTo(person, formCollection);
            person.SetAssignedIdTo(Guid.NewGuid().ToString());
            person.CreatedDate = DateTime.Now;
            person.CreatedBy = User.Identity.Name;
            person.DataStatus = EnumDataStatus.New.ToString();
            _refPersonRepository.Save(person);

            MEmployee mEmployeeToInsert = new MEmployee();
            TransferFormValuesTo(mEmployeeToInsert, viewModel);
            mEmployeeToInsert.DepartmentId = _mDepartmentRepository.Get(formCollection["DepartmentId"]);
            mEmployeeToInsert.SetAssignedIdTo(viewModel.Id);
            mEmployeeToInsert.CreatedDate = DateTime.Now;
            mEmployeeToInsert.CreatedBy = User.Identity.Name;
            mEmployeeToInsert.DataStatus = EnumDataStatus.New.ToString();

            mEmployeeToInsert.PersonId = person;
            _mEmployeeRepository.Save(mEmployeeToInsert);

            try
            {
                _mEmployeeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mEmployeeRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void TransferFormValuesTo(RefPerson person, FormCollection formCollection)
        {
            person.PersonFirstName = formCollection["PersonFirstName"];
            person.PersonLastName = formCollection["PersonLastName"];
            person.PersonGender = formCollection["PersonGender"];
            if (formCollection["PersonDob"] != null)
                person.PersonDob = Convert.ToDateTime(formCollection["PersonDob"]);
            person.PersonPob = formCollection["PersonPob"];
            person.PersonPhone = formCollection["PersonPhone"];
            person.PersonMobile = formCollection["PersonMobile"];
            person.PersonEmail = formCollection["PersonEmail"];
            person.PersonReligion = formCollection["PersonReligion"];
            person.PersonRace = formCollection["PersonRace"];
            person.PersonIdCardType = formCollection["PersonIdCardType"];
            person.PersonIdCardNo = formCollection["PersonIdCardNo"];
        }

        [Transaction]
        public ActionResult Delete(MEmployee viewModel, FormCollection formCollection)
        {
            MEmployee mEmployeeToDelete = _mEmployeeRepository.Get(viewModel.Id);

            if (mEmployeeToDelete != null)
            {
                _mEmployeeRepository.Delete(mEmployeeToDelete);
            }

            try
            {
                _mEmployeeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mEmployeeRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MEmployee viewModel, FormCollection formCollection)
        {


            MEmployee mEmployeeToUpdate = _mEmployeeRepository.Get(viewModel.Id);
            TransferFormValuesTo(mEmployeeToUpdate, viewModel);
            mEmployeeToUpdate.DepartmentId = _mDepartmentRepository.Get(formCollection["DepartmentId"]);
            mEmployeeToUpdate.ModifiedDate = DateTime.Now;
            mEmployeeToUpdate.ModifiedBy = User.Identity.Name;
            mEmployeeToUpdate.DataStatus = EnumDataStatus.Updated.ToString();

            RefPerson person = mEmployeeToUpdate.PersonId;
            if (person == null)
            {
                person = new RefPerson();
                TransferFormValuesTo(person, formCollection);
                person.SetAssignedIdTo(Guid.NewGuid().ToString());
                person.CreatedDate = DateTime.Now;
                person.CreatedBy = User.Identity.Name;
                person.DataStatus = EnumDataStatus.New.ToString();
                _refPersonRepository.Save(person);

                mEmployeeToUpdate.PersonId = person;
            }
            else
            {
                TransferFormValuesTo(person, formCollection);
                person.ModifiedDate = DateTime.Now;
                person.ModifiedBy = User.Identity.Name;
                person.DataStatus = EnumDataStatus.Updated.ToString();
            }


            _mEmployeeRepository.Update(mEmployeeToUpdate);

            try
            {
                _mEmployeeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mEmployeeRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void TransferFormValuesTo(MEmployee mEmployeeToUpdate, MEmployee mEmployeeFromForm)
        {
            mEmployeeToUpdate.DepartmentId = mEmployeeFromForm.DepartmentId;
            mEmployeeToUpdate.EmployeeStatus = mEmployeeFromForm.EmployeeStatus;
            mEmployeeToUpdate.EmployeeDesc = mEmployeeFromForm.EmployeeDesc;
        }


        [Transaction]
        public virtual ActionResult GetList()
        {
            var employees = _mEmployeeRepository.GetAll();
            StringBuilder sb = new StringBuilder();
            MEmployee mEmployee = new MEmployee();
            sb.AppendFormat("{0}:{1};", string.Empty, "-Pilih Karyawan-");
            for (int i = 0; i < employees.Count; i++)
            {
                mEmployee = employees[i];
                sb.AppendFormat("{0}:{1} {2}", mEmployee.Id, mEmployee.PersonId.PersonFirstName, mEmployee.PersonId.PersonLastName);
                if (i < employees.Count - 1)
                    sb.Append(";");
            }
            return Content(sb.ToString());
        }
    }
}
