using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction.Unit;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Web.Controllers.Master
{
    [HandleError]
    public class UnitTypeController : Controller
    {
        private readonly IMUnitTypeRepository _mUnitTypeRepository;
        private readonly IMCostCenterRepository _mCostCenterRepository;
        private readonly ITUnitRepository _tUnitRepository;

        public UnitTypeController(IMUnitTypeRepository mUnitTypeRepository, IMCostCenterRepository mCostCenterRepository, ITUnitRepository tUnitRepository)
        {
            Check.Require(mUnitTypeRepository != null, "mUnitTypeRepository may be not null");
            Check.Require(mCostCenterRepository != null, "mCostCenterRepository may be not null");
            Check.Require(tUnitRepository != null, "tUnitRepository may not be null");

            _mUnitTypeRepository = mUnitTypeRepository;
            _mCostCenterRepository = mCostCenterRepository;
            this._tUnitRepository = tUnitRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public ActionResult ListForSubGrid(string id)
        {
            var unitTypes = _mUnitTypeRepository.GetByCostCenterId(id);

            var jsonData = new
            {
                rows = (
                    from unitType in unitTypes
                    select new
                        {
                            i = unitType.Id.ToString(),
                            cell = new string[]
                        {
                            unitType.UnitTypeName,
                            unitType.UnitTypeTotal.HasValue ? unitType.UnitTypeTotal.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                            unitType.UnitTypeDesc               
                        }
                        }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUnitType()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string CostCenterId)
        {
            int totalRecords = 10;
            var unitTypes = _mUnitTypeRepository.GetByCostCenterId(CostCenterId);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from unitType in unitTypes
                    select new
                        {
                            i = unitType.Id.ToString(),
                            cell = new string[]
                        {
                            unitType.Id,
                            unitType.UnitTypeName,
                            unitType.UnitTypeTotal.ToString(),
                            unitType.UnitTypeDesc               
                        }
                        }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MUnitType viewModel, FormCollection formCollection)
        {
            try
            {
                UpdateNumericData(viewModel, formCollection);
                MUnitType unitType = new MUnitType();
                unitType.SetAssignedIdTo(Guid.NewGuid().ToString());
                TransferFormValuesTo(unitType, viewModel);
                unitType.CostCenterId = _mCostCenterRepository.Get(formCollection["CostCenterId"]);
                unitType.CreatedDate = DateTime.Now;
                unitType.CreatedBy = User.Identity.Name;

                _mUnitTypeRepository.Save(unitType);

                GenerateEachUnit(unitType);

                _mUnitTypeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mUnitTypeRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void GenerateEachUnit(MUnitType unitType)
        {
            if (unitType.UnitTypeTotal.HasValue)
            {
                TUnit unit;
                for (int i = 0; i < unitType.UnitTypeTotal.Value; i++)
                {
                    unit = new TUnit();
                    unit.CostCenterId = unitType.CostCenterId;
                    unit.UnitTypeId = unitType;
                    unit.UnitStatus = EnumUnitStatus.New.ToString();
                    unit.UnitNo = (i + 1).ToString();

                    unit.SetAssignedIdTo(Guid.NewGuid().ToString());
                    unit.CreatedDate = DateTime.Now;
                    unit.CreatedBy = User.Identity.Name;
                    unit.DataStatus = EnumDataStatus.New.ToString();
                    _tUnitRepository.Save(unit);
                }
            }
        }

        [Transaction]
        public ActionResult Update(MUnitType viewModel, FormCollection formCollection)
        {
            MUnitType mUnitTypeToUpdate = _mUnitTypeRepository.Get(viewModel.Id);
            TransferFormValuesTo(mUnitTypeToUpdate, viewModel);
            mUnitTypeToUpdate.CostCenterId = _mCostCenterRepository.Get(formCollection["CostCenterId"]);
            mUnitTypeToUpdate.ModifiedDate = DateTime.Now;
            mUnitTypeToUpdate.ModifiedBy = User.Identity.Name;
            mUnitTypeToUpdate.DataStatus = EnumDataStatus.Updated.ToString();
            _mUnitTypeRepository.Update(mUnitTypeToUpdate);

            try
            {
                _mUnitTypeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mUnitTypeRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(MUnitType viewModel, FormCollection formCollection)
        {
            MUnitType mUnitTypeToDelete = _mUnitTypeRepository.Get(viewModel.Id);

            if (mUnitTypeToDelete != null)
            {
                _mUnitTypeRepository.Delete(mUnitTypeToDelete);
            }

            try
            {
                _mUnitTypeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mUnitTypeRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void UpdateNumericData(MUnitType viewModel, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["UnitTypeTotal"]))
            {
                string UnitTypeTotal = formCollection["UnitTypeTotal"].Replace(",", "");
                int? total = Convert.ToInt32(UnitTypeTotal);
                viewModel.UnitTypeTotal = total;
            }
        }

        private void TransferFormValuesTo(MUnitType unitType, MUnitType viewModel)
        {
            unitType.UnitTypeName = viewModel.UnitTypeName;
            unitType.UnitTypeStatus = viewModel.UnitTypeStatus;
            unitType.UnitTypeDesc = viewModel.UnitTypeDesc;
            unitType.UnitTypeTotal = viewModel.UnitTypeTotal;
        }

    }
}
