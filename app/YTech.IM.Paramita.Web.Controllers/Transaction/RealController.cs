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
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Web.Controllers.Helper;
using YTech.IM.Paramita.Web.Controllers.ViewModel;

namespace YTech.IM.Paramita.Web.Controllers.Transaction
{
    [HandleError]
    public class RealController : Controller
    {
        private readonly ITRealRepository _tRealRepository;
        private readonly IMCostCenterRepository _mCostCenterRepository;

        public RealController(ITRealRepository tRealRepository, IMCostCenterRepository mCostCenterRepository)
        {
            Check.Require(tRealRepository != null, "tRealRepository may not be null");
            this._tRealRepository = tRealRepository;
            Check.Require(mCostCenterRepository != null, "mCostCenterRepository may not be null");
            this._mCostCenterRepository = mCostCenterRepository;
        }
        
        public ActionResult Index()
        {
            IList<MCostCenter> list = _mCostCenterRepository.GetAll();
            ViewData["CostCenterList"] = new SelectList(list, "Id", "CostCenterName");
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string CostCenterId)
        {
            MCostCenter costCenter = _mCostCenterRepository.Get(CostCenterId);

            int totalRecords = 0;
            var reals = _tRealRepository.GetPagedList(sidx, sord, page, 0, ref totalRecords, costCenter);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from real in reals
                    select new
                    {
                        i = real.Id.ToString(),
                        cell = new string[] {
                            real.Id, 
                          real.RealDate.HasValue ? real.RealDate.Value.ToString(CommonHelper.PeriodFormat) : null,
                            real.RealPercentValue.HasValue ? real.RealPercentValue.Value.ToString(CommonHelper.NumberFormat) : null,
                            real.RealDesc
                        }
                    }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(TReal viewModel, FormCollection formCollection)
        {

            TReal real = new TReal();
            TransferFormValuesTo(real, viewModel);
            //real.CostCenterId = _mCostCenterRepository.Get(formCollection["CostCenterId"]);
            real.SetAssignedIdTo(Guid.NewGuid().ToString());
            real.CreatedDate = DateTime.Now;
            real.CreatedBy = User.Identity.Name;
            real.DataStatus = EnumDataStatus.New.ToString();
            _tRealRepository.Save(real);

            try
            {
                _tRealRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _tRealRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("Data berhasil disimpan");
        }


        [Transaction]
        public ActionResult Update(TReal viewModel, FormCollection formCollection)
        {
            TReal real = _tRealRepository.Get(viewModel.Id);
            TransferFormValuesTo(real, viewModel);
            real.ModifiedDate = DateTime.Now;
            real.ModifiedBy = User.Identity.Name;
            real.DataStatus = EnumDataStatus.Updated.ToString();
            _tRealRepository.Update(real);

            try
            {
                _tRealRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _tRealRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("Data berhasil disimpan");
        }

        private void TransferFormValuesTo(TReal real, TReal viewModel)
        {
            real.CostCenterId = viewModel.CostCenterId;
            real.RealDate = viewModel.RealDate;
            real.RealPercentValue = viewModel.RealPercentValue;
            real.RealDesc = viewModel.RealDesc;
        }

        [Transaction]
        public ActionResult Delete(TReal viewModel, FormCollection formCollection)
        {
            _tRealRepository.DbContext.BeginTransaction();
            TReal real = _tRealRepository.Get(viewModel.Id);

            if (real != null)
            {
                _tRealRepository.Delete(real);
            }

            try
            {
                _tRealRepository.DbContext.CommitTransaction();
            }
            catch (Exception e)
            {
                _tRealRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Realisasi berhasil dihapus");
        }
    }
}
