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
    public class SupplierController : Controller
    {
        private readonly IMSupplierRepository _mSupplierRepository;
        private readonly IRefAddressRepository _refAddressRepository;
        private readonly IMAccountRefRepository _mAccountRefRepository;
        private readonly IMAccountRepository _mAccountRepository;
        public SupplierController(IMSupplierRepository mSupplierRepository, IRefAddressRepository refAddressRepository, IMAccountRefRepository mAccountRefRepository, IMAccountRepository mAccountRepository)
        {
            Check.Require(mSupplierRepository != null, "mSupplierRepository may not be null");
            Check.Require(refAddressRepository != null, "refAddressRepository may not be null");
            Check.Require(mAccountRefRepository != null, "mAccountRefRepository may not be null");
            Check.Require(mAccountRepository != null, "mAccountRepository may not be null");

            this._mSupplierRepository = mSupplierRepository;
            this._refAddressRepository = refAddressRepository;
            this._mAccountRefRepository = mAccountRefRepository;
            this._mAccountRepository = mAccountRepository;
        }


        public ActionResult Index()
        {
            return View();
        }


        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var sups = _mSupplierRepository.GetPagedSupplierList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from sup in sups
                    select new
                    {
                        i = sup.Id.ToString(),
                        cell = new string[] {
                            sup.Id, 
                            sup.SupplierName, 
                             GetAccountRef(sup.Id) != null ? GetAccountRef(sup.Id).AccountId.Id : null,
                         GetAccountRef(sup.Id) != null ? GetAccountRef(sup.Id).AccountId.AccountName : null,
                            sup.SupplierMaxDebt.HasValue ? sup.SupplierMaxDebt.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                            sup.SupplierStatus,
                          sup.AddressId != null?  sup.AddressId.AddressLine1 : null,
                          sup.AddressId != null?  sup.AddressId.AddressLine2 : null,
                          sup.AddressId != null?  sup.AddressId.AddressLine3 : null,
                          sup.AddressId != null?  sup.AddressId.AddressPhone : null,
                          sup.AddressId != null?  sup.AddressId.AddressCity : null,
                            sup.SupplierDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private MAccountRef GetAccountRef(string supplierId)
        {
            MAccountRef accountRef = _mAccountRefRepository.GetByRefTableId(EnumReferenceTable.Supplier, supplierId);
            if (accountRef != null)
            {
                return accountRef;
            }
            return null;
        }

        [Transaction]
        public ActionResult Insert(MSupplier viewModel, FormCollection formCollection)
        {
            try
            {
                _mSupplierRepository.DbContext.BeginTransaction();

                UpdateNumericData(viewModel, formCollection);
                RefAddress address = new RefAddress();
                TransferFormValuesTo(address, formCollection);
                address.SetAssignedIdTo(Guid.NewGuid().ToString());
                address.CreatedDate = DateTime.Now;
                address.CreatedBy = User.Identity.Name;
                address.DataStatus = EnumDataStatus.New.ToString();
                _refAddressRepository.Save(address);

                MSupplier mSupplierToInsert = new MSupplier();
                TransferFormValuesTo(mSupplierToInsert, viewModel);
                mSupplierToInsert.SetAssignedIdTo(viewModel.Id);
                mSupplierToInsert.CreatedDate = DateTime.Now;
                mSupplierToInsert.CreatedBy = User.Identity.Name;
                mSupplierToInsert.DataStatus = EnumDataStatus.New.ToString();

                mSupplierToInsert.AddressId = address;

                _mSupplierRepository.Save(mSupplierToInsert);

                MAccountRef accountRef = new MAccountRef();
                accountRef.SetAssignedIdTo(Guid.NewGuid().ToString());
                accountRef.ReferenceId = mSupplierToInsert.Id;
                accountRef.ReferenceTable = EnumReferenceTable.Supplier.ToString();
                accountRef.ReferenceType = EnumReferenceTable.Supplier.ToString();
                accountRef.AccountId = _mAccountRepository.Get(formCollection["AccountId"]);
                accountRef.CreatedDate = DateTime.Now;
                accountRef.CreatedBy = User.Identity.Name;
                accountRef.DataStatus = EnumDataStatus.New.ToString();
                _mAccountRefRepository.Save(accountRef);

                _mSupplierRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mSupplierRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
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

        [Transaction]
        public ActionResult Delete(MSupplier viewModel, FormCollection formCollection)
        {
            MSupplier mSupplierToDelete = _mSupplierRepository.Get(viewModel.Id);

            if (mSupplierToDelete != null)
            {
                _mSupplierRepository.Delete(mSupplierToDelete);
                _refAddressRepository.Delete(mSupplierToDelete.AddressId);
            }

            try
            {
                _mSupplierRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mSupplierRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MSupplier viewModel, FormCollection formCollection)
        {
            try
            {
                _mSupplierRepository.DbContext.BeginTransaction();

                UpdateNumericData(viewModel, formCollection);
                MSupplier mSupplierToUpdate = _mSupplierRepository.Get(viewModel.Id);
                TransferFormValuesTo(mSupplierToUpdate, viewModel);
                mSupplierToUpdate.ModifiedDate = DateTime.Now;
                mSupplierToUpdate.ModifiedBy = User.Identity.Name;
                mSupplierToUpdate.DataStatus = EnumDataStatus.Updated.ToString();

                RefAddress address = mSupplierToUpdate.AddressId;
                TransferFormValuesTo(address, formCollection);
                address.ModifiedDate = DateTime.Now;
                address.ModifiedBy = User.Identity.Name;
                address.DataStatus = EnumDataStatus.Updated.ToString();

                _mSupplierRepository.Update(mSupplierToUpdate);

                bool isSave = false;
                MAccountRef accountRef = GetAccountRef(mSupplierToUpdate.Id);
                if (accountRef == null)
                {
                    accountRef = new MAccountRef();
                    accountRef.SetAssignedIdTo(Guid.NewGuid().ToString());
                    accountRef.CreatedDate = DateTime.Now;
                    accountRef.CreatedBy = User.Identity.Name;
                    accountRef.DataStatus = EnumDataStatus.New.ToString();
                    isSave = true;
                }
                else
                {
                    accountRef.ModifiedDate = DateTime.Now;
                    accountRef.ModifiedBy = User.Identity.Name;
                    accountRef.DataStatus = EnumDataStatus.Updated.ToString();
                }
                accountRef.ReferenceId = mSupplierToUpdate.Id;
                accountRef.ReferenceTable = EnumReferenceTable.Supplier.ToString();
                accountRef.ReferenceType = EnumReferenceTable.Supplier.ToString();
                accountRef.AccountId = _mAccountRepository.Get(formCollection["AccountId"]);
                if (isSave)
                {
                    _mAccountRefRepository.Save(accountRef);
                }
                else
                {
                    _mAccountRefRepository.Update(accountRef);

                }

                _mSupplierRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mSupplierRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void UpdateNumericData(MSupplier viewModel, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["SupplierMaxDebt"]))
            {
                string wide = formCollection["SupplierMaxDebt"].Replace(",", "");
                decimal? budget = Convert.ToDecimal(wide);
                viewModel.SupplierMaxDebt = budget;
            }
            else
            {
                viewModel.SupplierMaxDebt = null;
            }
        }

        private static void TransferFormValuesTo(MSupplier mSupplierToUpdate, MSupplier mSupplierFromForm)
        {
            mSupplierToUpdate.SupplierName = mSupplierFromForm.SupplierName;
            mSupplierToUpdate.SupplierDesc = mSupplierFromForm.SupplierDesc;
            mSupplierToUpdate.SupplierMaxDebt = mSupplierFromForm.SupplierMaxDebt;
            mSupplierToUpdate.SupplierStatus = mSupplierFromForm.SupplierStatus;
        }
    }
}
