﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.Transaction.Unit;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Web.Controllers.ViewModel;

namespace YTech.IM.Paramita.Web.Controllers.Transaction
{
    [HandleError]
    public class UnitController : Controller
    {
        //public UnitController()
        //    : this(new TTransRepository(), new MWarehouseRepository(), new MSupplierRepository(), new MItemRepository(), new TStockCardRepository(), new TStockItemRepository(), new TTransRefRepository(), new TStockRepository(), new TStockRefRepository())
        //{
        //}

        private readonly ITUnitRepository _tUnitRepository;
        private readonly IMCustomerRepository _mCustomerRepository;
        private readonly ITTransUnitRepository _tTransUnitRepository;
        private readonly IMCostCenterRepository _mCostCenterRepository;
        private readonly IMUnitTypeRepository _mUnitTypeRepository;

        public UnitController(ITUnitRepository tUnitRepository, IMCustomerRepository mCustomerRepository, ITTransUnitRepository tTransUnitRepository, IMCostCenterRepository mCostCenterRepository, IMUnitTypeRepository mUnitTypeRepository)
        {
            Check.Require(tUnitRepository != null, "tUnitRepository may not be null");
            Check.Require(mCustomerRepository != null, "mCustomerRepository may not be null");
            Check.Require(tTransUnitRepository != null, "itTransUnitRepository may not be null");
            Check.Require(mCostCenterRepository != null, "mCostCenterRepository may not be null");
            Check.Require(mUnitTypeRepository != null, "mUnitTypeRepository may not be null");

            this._tUnitRepository = tUnitRepository;
            this._mCustomerRepository = mCustomerRepository;
            this._tTransUnitRepository = tTransUnitRepository;
            this._mCostCenterRepository = mCostCenterRepository;
            this._mUnitTypeRepository = mUnitTypeRepository;
        }

        public ActionResult Index()
        {
            UnitViewModel viewModel = UnitViewModel.Create(_mCostCenterRepository);
            return View(viewModel);
        }


        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string costCenterId)
        {
            int totalRecords = 0;
            var units = _tUnitRepository.GetPagedUnitList(sidx, sord, page, rows, ref totalRecords, costCenterId);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from unit in units
                    select new
                    {
                        i = unit.Id.ToString(),
                        cell = new string[] {
                            unit.UnitStatus,
                            unit.Id, 
                            unit.UnitNo, 
                            unit.UnitTypeId.Id, 
                            unit.UnitTypeId.UnitTypeName, 
                            unit.UnitLandWide.HasValue ?  unit.UnitLandWide.Value.ToString(Helper.CommonHelper.IntegerFormat) : null,
                            unit.UnitWide.HasValue ? unit.UnitWide.Value.ToString(Helper.CommonHelper.IntegerFormat) : null,
                            unit.UnitLocation,
                            unit.UnitPrice.HasValue ? unit.UnitPrice.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                            unit.UnitStatus.Equals(EnumUnitStatus.New.ToString())?"Baru":"Terjual",
                            unit.UnitDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [Transaction]
        public ActionResult Insert(TUnit viewModel, FormCollection formCollection)
        {
            UpdateNumericData(viewModel, formCollection);

            TUnit unitToInsert = new TUnit();
            TransferFormValuesTo(unitToInsert, viewModel);
            unitToInsert.UnitTypeId = _mUnitTypeRepository.Get(formCollection["UnitTypeId"]);
            unitToInsert.UnitStatus = EnumUnitStatus.New.ToString();

            unitToInsert.SetAssignedIdTo(viewModel.Id);
            unitToInsert.CreatedDate = DateTime.Now;
            unitToInsert.CreatedBy = User.Identity.Name;
            unitToInsert.DataStatus = EnumDataStatus.New.ToString();
            _tUnitRepository.Save(unitToInsert);

            try
            {
                _tUnitRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _tUnitRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(TUnit viewModel, FormCollection formCollection)
        {
            TUnit unitToDelete = _tUnitRepository.Get(viewModel.Id);

            if (unitToDelete != null)
            {
                _tUnitRepository.Delete(unitToDelete);
            }

            try
            {
                _tUnitRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _tUnitRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(TUnit viewModel, FormCollection formCollection)
        {
            UpdateNumericData(viewModel, formCollection);
            TUnit unitToUpdate = _tUnitRepository.Get(viewModel.Id);
            TransferFormValuesTo(unitToUpdate, viewModel);
            unitToUpdate.UnitTypeId = _mUnitTypeRepository.Get(formCollection["UnitTypeId"]);
            unitToUpdate.ModifiedDate = DateTime.Now;
            unitToUpdate.ModifiedBy = User.Identity.Name;
            unitToUpdate.DataStatus = EnumDataStatus.Updated.ToString();
            _tUnitRepository.Update(unitToUpdate);

            try
            {
                _tUnitRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _tUnitRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private static void UpdateNumericData(TUnit viewModel, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["UnitWide"]))
            {
                string wide = formCollection["UnitWide"].Replace(",", "");
                int? unitWide = Convert.ToInt32(wide);
                viewModel.UnitWide = unitWide;
            }
            if (!string.IsNullOrEmpty(formCollection["UnitLandWide"]))
            {
                string wide = formCollection["UnitLandWide"].Replace(",", "");
                int? unitWide = Convert.ToInt32(wide);
                viewModel.UnitLandWide = unitWide;
            }
            if (!string.IsNullOrEmpty(formCollection["UnitPrice"]))
            {
                string wide = formCollection["UnitPrice"].Replace(",", "");
                decimal? price = Convert.ToDecimal(wide);
                viewModel.UnitPrice = price;
            }
        }

        private void TransferFormValuesTo(TUnit unitToInsert, TUnit unitFrom)
        {
            unitToInsert.UnitLocation = unitFrom.UnitLocation;
            unitToInsert.UnitWide = unitFrom.UnitWide;
            unitToInsert.UnitLandWide = unitFrom.UnitLandWide;
            unitToInsert.UnitPrice = unitFrom.UnitPrice;
            unitToInsert.UnitDesc = unitFrom.UnitDesc;
            unitToInsert.CostCenterId = unitFrom.CostCenterId;
            unitToInsert.UnitNo = unitFrom.UnitNo;
        }

        [Transaction]
        public ActionResult UnitSales(string unitId)
        {
            UnitSalesFormViewModel viewModel = UnitSalesFormViewModel.CreateUnitSalesFormViewModel(_mCustomerRepository, _mCostCenterRepository, _tTransUnitRepository, unitId);

            TempData["UnitId"] = "id=" + unitId;
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnitSales(TTransUnit viewModel, FormCollection formCollection, string unitId)
        {
            _tTransUnitRepository.DbContext.BeginTransaction();

            TUnit unit = _tUnitRepository.Get(unitId);

            TTransUnit transUnit = _tTransUnitRepository.GetByUnitId(unitId);
            bool isSave = true;
            if (transUnit == null)
            {
                transUnit = new TTransUnit();
                transUnit.SetAssignedIdTo(Guid.Empty.ToString());
                transUnit.CreatedDate = DateTime.Now;
                transUnit.CreatedBy = User.Identity.Name;
                transUnit.DataStatus = EnumDataStatus.New.ToString();
            }
            else
            {
                isSave = false;
                transUnit.ModifiedDate = DateTime.Now;
                transUnit.ModifiedBy = User.Identity.Name;
                transUnit.DataStatus = EnumDataStatus.Updated.ToString();
            }
            transUnit.UnitId = unit;
            if (!string.IsNullOrEmpty(formCollection["CustomerId"]))
            {
                transUnit.CustomerId = _mCustomerRepository.Get(formCollection["CustomerId"]);
            }
            transUnit.TransUnitDate = viewModel.TransUnitDate;
            transUnit.TransUnitFactur = viewModel.TransUnitFactur;
            transUnit.TransUnitPrice = Convert.ToDecimal(formCollection["TransUnitPrice"].Replace(",", ""));
            transUnit.TransUnitDesc = viewModel.TransUnitDesc;
            transUnit.TransUnitPaymentMethod = viewModel.TransUnitPaymentMethod;
            transUnit.CostCenterId = unit.CostCenterId;

            //change unit status
            unit.UnitStatus = EnumUnitStatus.Sale.ToString();
            unit.ModifiedDate = DateTime.Now;
            unit.ModifiedBy = User.Identity.Name;
            unit.DataStatus = EnumDataStatus.Updated.ToString();
            _tUnitRepository.Update(unit);

            try
            {
                if (isSave)
                    _tTransUnitRepository.Save(transUnit);
                else
                    _tTransUnitRepository.Update(transUnit);
                _tTransUnitRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
            }
            catch (Exception ex)
            {
                _tTransUnitRepository.DbContext.RollbackTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
                throw ex;
                //return Content(ex.Message);
            }
            return RedirectToAction("UnitSales", new RouteValueDictionary(new { unitId }));
        }

        [Transaction]
        public ActionResult DeleteUnitSales(string unitId)
        {
            _tTransUnitRepository.DbContext.BeginTransaction();
            TUnit unit = _tUnitRepository.Get(unitId);
            //change unit status
            unit.UnitStatus = EnumUnitStatus.New.ToString();
            unit.ModifiedDate = DateTime.Now;
            unit.ModifiedBy = User.Identity.Name;
            unit.DataStatus = EnumDataStatus.Updated.ToString();
            try
            {
                _tUnitRepository.Update(unit);
                _tTransUnitRepository.DeleteByUnitId(unitId);
                _tTransUnitRepository.DbContext.CommitTransaction();
            }
            catch (Exception ex)
            {
                _tTransUnitRepository.DbContext.RollbackTransaction();
            }
            return Content("Pembatalan penjualan unit berhasil dilakukan.");
        }

        public ActionResult GetUnitTypeList(string costCenterId)
        {
            var unitTypes = _mUnitTypeRepository.GetByCostCenterId(costCenterId);
            StringBuilder sb = new StringBuilder();
            MUnitType unitType;
            sb.AppendFormat("{0}:{1}", string.Empty, "-Pilih Tipe Unit-");
            for (int i = 0; i < unitTypes.Count; i++)
            {
                unitType = unitTypes[i];
                sb.AppendFormat(";{0}:{1}", unitType.Id, unitType.UnitTypeName);
            }
            return Content(sb.ToString());
        }
    }
}
