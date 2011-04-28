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
    public class CostCenterController : Controller
    {
         public CostCenterController() : this(new MCostCenterRepository())
         {}

        private readonly IMCostCenterRepository _mCostCenterRepository;
        public CostCenterController(IMCostCenterRepository mCostCenterRepository)
        {
            Check.Require(mCostCenterRepository != null, "mCostCenterRepository may not be null");

            this._mCostCenterRepository = mCostCenterRepository;
        }


        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var itemCats = _mCostCenterRepository.GetPagedCostCenterList(sidx, sord, page, rows, ref totalRecords);
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
                            string.Empty,
                            itemCat.Id, 
                            itemCat.CostCenterName, 
                          itemCat.CostCenterTotalBudget.HasValue ?  itemCat.CostCenterTotalBudget.Value.ToString(Helper.CommonHelper.NumberFormat) : null, 
                            itemCat.CostCenterStatus, 
                             itemCat.CostCenterStartDate.HasValue ?  itemCat.CostCenterStartDate.Value.ToString(Helper.CommonHelper.DateFormat) : null,
                              itemCat.CostCenterEndDate.HasValue ?  itemCat.CostCenterEndDate.Value.ToString(Helper.CommonHelper.DateFormat) : null, 
                            itemCat.CostCenterDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MCostCenter viewModel, FormCollection formCollection)
        {
            UpdateNumericData(viewModel, formCollection);

            MCostCenter mCompanyToInsert = new MCostCenter();
            TransferFormValuesTo(mCompanyToInsert, viewModel);
            mCompanyToInsert.SetAssignedIdTo(viewModel.Id);
            mCompanyToInsert.CreatedDate = DateTime.Now;
            mCompanyToInsert.CreatedBy = User.Identity.Name;
            mCompanyToInsert.DataStatus = EnumDataStatus.New.ToString();
            _mCostCenterRepository.Save(mCompanyToInsert);

            try
            {
                _mCostCenterRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCostCenterRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(MCostCenter viewModel, FormCollection formCollection)
        {
            MCostCenter mCompanyToDelete = _mCostCenterRepository.Get(viewModel.Id);

            if (mCompanyToDelete != null)
            {
                _mCostCenterRepository.Delete(mCompanyToDelete);
            }

            try
            {
                _mCostCenterRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCostCenterRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MCostCenter viewModel, FormCollection formCollection)
        {
            UpdateNumericData(viewModel, formCollection);
            MCostCenter mCompanyToUpdate = _mCostCenterRepository.Get(viewModel.Id);
            TransferFormValuesTo(mCompanyToUpdate, viewModel);
            mCompanyToUpdate.ModifiedDate = DateTime.Now;
            mCompanyToUpdate.ModifiedBy = User.Identity.Name;
            mCompanyToUpdate.DataStatus = EnumDataStatus.Updated.ToString();
            _mCostCenterRepository.Update(mCompanyToUpdate);

            try
            {
                _mCostCenterRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCostCenterRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void UpdateNumericData(MCostCenter viewModel, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["CostCenterTotalBudget"]))
            {
                string wide = formCollection["CostCenterTotalBudget"].Replace(",", "");
                decimal? budget = Convert.ToDecimal(wide);
                viewModel.CostCenterTotalBudget = budget;
            }
        }

        private void TransferFormValuesTo(MCostCenter mCompanyToUpdate, MCostCenter mCompanyFromForm)
        {
            mCompanyToUpdate.CostCenterName = mCompanyFromForm.CostCenterName;
            mCompanyToUpdate.CostCenterDesc = mCompanyFromForm.CostCenterDesc;
            mCompanyToUpdate.CostCenterTotalBudget = mCompanyFromForm.CostCenterTotalBudget;
            mCompanyToUpdate.CostCenterStatus = mCompanyFromForm.CostCenterStatus;
            mCompanyToUpdate.CostCenterStartDate = mCompanyFromForm.CostCenterStartDate;
            mCompanyToUpdate.CostCenterEndDate = mCompanyFromForm.CostCenterEndDate;
        }


        [Transaction]
        public virtual ActionResult GetList()
        {
            var brands = _mCostCenterRepository.GetAll();
            StringBuilder sb = new StringBuilder();
            MCostCenter mCostCenter;
            sb.AppendFormat("{0}:{1};", string.Empty, "-Pilih Cost Center-");
            for (int i = 0; i < brands.Count; i++)
            {
                mCostCenter = brands[i];
                sb.AppendFormat("{0}:{1}", mCostCenter.Id, mCostCenter.CostCenterName);
                if (i < brands.Count - 1)
                    sb.Append(";");
            }
            return Content(sb.ToString());
        }
    }
}
