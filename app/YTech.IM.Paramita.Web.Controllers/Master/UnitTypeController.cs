using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Web.Controllers.Master
{
    [HandleError]
    public class UnitTypeController : Controller
    {
        private readonly IMUnitTypeRepository _mUnitTypeRepository;
        private readonly IMCostCenterRepository _mCostCenterRepository;

        public UnitTypeController(IMUnitTypeRepository mUnitTypeRepository, IMCostCenterRepository mCostCenterRepository)
        {
            Check.Require(mUnitTypeRepository != null, "mUnitTypeRepository may be not null");
            Check.Require(mCostCenterRepository != null, "mCostCenterRepository may be not null");

            _mUnitTypeRepository = mUnitTypeRepository;
            _mCostCenterRepository = mCostCenterRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public ActionResult ListForSubGrid(string id)
        {
            var unitTypes = _mUnitTypeRepository.GetByUnitTypeId(id);

            var jsonData = new
            {
                rows = (
                    from unitType in unitTypes select new
                    {
                        i = unitType.Id.ToString(),
                        cell = new string[]
                        {
                            unitType.UnitTypeName,
                            unitType.UnitTypeTotal.ToString(),
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
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string unitTypeId)
        {
            int totalRecords = 10;
            var unitTypes = _mUnitTypeRepository.GetByUnitTypeId(unitTypeId);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from unitType in unitTypes select new
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
            UpdateNumericData(viewModel, formCollection);
            MUnitType unitType = new MUnitType();
            unitType.SetAssignedIdTo(Guid.NewGuid().ToString());
            TransferFormValuesTo(unitType, viewModel);
            unitType.CostCenterId = _mCostCenterRepository.Get(formCollection["CostCenterId"]);
            unitType.CreatedDate = DateTime.Now;
            unitType.CreatedBy = User.Identity.Name;

            _mUnitTypeRepository.Save(unitType);

            try
            {
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
