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
using YTech.IM.Paramita.Core.View;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Web.Controllers.Helper;
using YTech.IM.Paramita.Web.Controllers.ViewModel;
using Microsoft.Reporting.WebForms;

namespace YTech.IM.Paramita.Web.Controllers.Transaction
{
    [HandleError]
    public class ReportController : Controller
    {
        private readonly ITJournalRepository _tJournalRepository;
        private readonly ITJournalDetRepository _tJournalDetRepository;
        private readonly IMCostCenterRepository _mCostCenterRepository;
        private readonly IMAccountRepository _mAccountRepository;
        private readonly ITRecAccountRepository _tRecAccountRepository;
        private readonly ITRecPeriodRepository _tRecPeriodRepository;
        private readonly IMBrandRepository _mBrandRepository;
        private readonly IMSupplierRepository _mSupplierRepository;
        private readonly IMWarehouseRepository _mWarehouseRepository;
        private readonly IMItemRepository _mItemRepository;
        private readonly ITStockCardRepository _tStockCardRepository;
        private readonly ITStockItemRepository _tStockItemRepository;
        private readonly ITTransDetRepository _tTransDetRepository;
        private readonly ITRealRepository _tRealRepository;
        private readonly IVJournalDetFlowRepository _vJournalDetFlowRepository;
        private readonly IVStockCardFlowRepository _vStockCardFlowRepository;

        public ReportController(ITJournalRepository tJournalRepository, ITJournalDetRepository tJournalDetRepository, IMCostCenterRepository mCostCenterRepository, IMAccountRepository mAccountRepository, ITRecAccountRepository tRecAccountRepository, ITRecPeriodRepository tRecPeriodRepository, IMBrandRepository mBrandRepository, IMSupplierRepository mSupplierRepository, IMWarehouseRepository mWarehouseRepository, IMItemRepository mItemRepository, ITStockCardRepository tStockCardRepository, ITStockItemRepository tStockItemRepository, ITTransDetRepository tTransDetRepository, ITRealRepository tRealRepository, IVJournalDetFlowRepository vJournalDetFlowRepository, IVStockCardFlowRepository vStockCardFlowRepository)
        {
            Check.Require(tJournalRepository != null, "tJournalRepository may not be null");
            Check.Require(tJournalDetRepository != null, "tJournalDetRepository may not be null");
            Check.Require(mCostCenterRepository != null, "mCostCenterRepository may not be null");
            Check.Require(mAccountRepository != null, "mAccountRepository may not be null");
            Check.Require(tRecAccountRepository != null, "tRecAccountRepository may not be null");
            Check.Require(tRecPeriodRepository != null, "tRecPeriodRepository may not be null");
            Check.Require(mBrandRepository != null, "mBrandRepository may not be null");
            Check.Require(mSupplierRepository != null, "mSupplierRepository may not be null");
            Check.Require(mWarehouseRepository != null, "mBrandRepository may not be null");
            Check.Require(mItemRepository != null, "mItemRepository may not be null");
            Check.Require(tStockCardRepository != null, "tStockCardRepository may not be null");
            Check.Require(tStockItemRepository != null, "tStockItemRepository may not be null");
            Check.Require(tTransDetRepository != null, "tTransDetRepository may not be null");
            Check.Require(tRealRepository != null, "tRealRepository may not be null");
            Check.Require(vJournalDetFlowRepository != null, "vJournalDetFlowRepository may not be null");
            Check.Require(vStockCardFlowRepository != null, "vStockCardFlowRepository may not be null");

            this._tJournalRepository = tJournalRepository;
            this._tJournalDetRepository = tJournalDetRepository;
            this._mCostCenterRepository = mCostCenterRepository;
            this._mAccountRepository = mAccountRepository;
            this._tRecAccountRepository = tRecAccountRepository;
            this._tRecPeriodRepository = tRecPeriodRepository;
            this._mBrandRepository = mBrandRepository;
            this._mSupplierRepository = mSupplierRepository;
            this._mWarehouseRepository = mWarehouseRepository;
            this._mItemRepository = mItemRepository;
            this._tStockCardRepository = tStockCardRepository;
            this._tStockItemRepository = tStockItemRepository;
            this._tTransDetRepository = tTransDetRepository;
            this._tRealRepository = tRealRepository;
            this._vJournalDetFlowRepository = vJournalDetFlowRepository;
            this._vStockCardFlowRepository = vStockCardFlowRepository;
        }

        [Transaction]
        public ActionResult ReportTrans(EnumReports reports, EnumTransactionStatus TransStatus, EnumReportGroupBy? groupBy = null)
        {
            ReportParamViewModel viewModel = ReportParamViewModel.CreateReportParamViewModel(_mCostCenterRepository, _mWarehouseRepository, _mSupplierRepository, _tRecPeriodRepository, _mItemRepository);
            if (TransStatus != EnumTransactionStatus.None)
            {
                viewModel.TransStatus = TransStatus;
            }
            string title = Helper.CommonHelper.GetStringValue(reports);
            switch (reports)
            {
                case EnumReports.RptBrand:

                    break;
                case EnumReports.RptCostCenter:
                    break;
                case EnumReports.RptNeraca:
                    viewModel.ShowCostCenter = true;
                    viewModel.ShowRecPeriod = true;
                    viewModel.ShowGenerateDetail = true;
                    break;
                case EnumReports.RptLR:
                    viewModel.ShowCostCenter = true;
                    viewModel.ShowRecPeriod = true;
                    viewModel.ShowGenerateDetail = true;
                    break;
                case EnumReports.RptNeracaSum:
                    viewModel.ShowRecPeriod = true;
                    viewModel.ShowGenerateDetail = true;
                    break;
                case EnumReports.RptLRSum:
                    viewModel.ShowRecPeriod = true;
                    viewModel.ShowGenerateDetail = true;
                    break;
                case EnumReports.RptStockCard:
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowItem = true;
                    viewModel.ShowWarehouse = true;
                    break;
                case EnumReports.RptStockItem:
                    viewModel.ShowItem = true;
                    viewModel.ShowWarehouse = true;
                    break;
                case EnumReports.RptAnalyzeBudgetDetail:
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowItem = true;
                    viewModel.ShowWarehouse = true;
                    break;
                case EnumReports.RptTransDetail:
                    title = string.Format(title, Helper.CommonHelper.GetStringValue(viewModel.TransStatus));
                    switch (viewModel.TransStatus)
                    {
                        case EnumTransactionStatus.PurchaseOrder:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            viewModel.ShowSupplier = true;
                            break;
                        case EnumTransactionStatus.Received:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            viewModel.ShowSupplier = true;
                            break;
                        case EnumTransactionStatus.Purchase:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            viewModel.ShowSupplier = true;
                            break;
                        case EnumTransactionStatus.ReturPurchase:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            viewModel.ShowSupplier = true;
                            break;
                        case EnumTransactionStatus.Using:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            break;
                        case EnumTransactionStatus.Mutation:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            break;
                        case EnumTransactionStatus.Budgeting:
                            //viewModel.ShowDateFrom = true;
                            //viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            break;
                    }

                    break;
                case EnumReports.RptTransRecap:

                    string groupByTitle = groupBy != null ? Helper.CommonHelper.GetStringValue(groupBy) : string.Empty;
                    title = string.Format(title, Helper.CommonHelper.GetStringValue(viewModel.TransStatus), groupByTitle);
                    switch (viewModel.TransStatus)
                    {
                        case EnumTransactionStatus.PurchaseOrder:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            viewModel.ShowSupplier = true;
                            break;
                        case EnumTransactionStatus.Received:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            viewModel.ShowSupplier = true;
                            break;
                        case EnumTransactionStatus.Purchase:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            viewModel.ShowSupplier = true;
                            break;
                        case EnumTransactionStatus.ReturPurchase:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            viewModel.ShowSupplier = true;
                            break;
                        case EnumTransactionStatus.Using:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            break;
                        case EnumTransactionStatus.Mutation:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            break;
                        case EnumTransactionStatus.Budgeting:
                            //viewModel.ShowDateFrom = true;
                            //viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            break;
                    }

                    break;
                case EnumReports.RptItem:

                    break;
                case EnumReports.RptJournal:
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowAccount = false;
                    viewModel.ShowAccountTo = false;
                    break;
                case EnumReports.RptBukuBesar:
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowAccount = true;
                    viewModel.ShowAccountTo = true;
                    break;
                case EnumReports.RptJournalByCostCenter:
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowCostCenter = true;
                    viewModel.ShowAccount = false;
                    viewModel.ShowAccountTo = false;
                    break;
                case EnumReports.RptBukuBesarByCostCenter:
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowCostCenter = true;
                    viewModel.ShowAccount = true;
                    viewModel.ShowAccountTo = true;
                    break;
            }
            ViewData["CurrentItem"] = title;
            ViewData["ExportFormat"] = new SelectList(Enum.GetValues(typeof(EnumExportFormat)));

            return View(viewModel);
        }

        [Transaction]
        public ActionResult Report(EnumReports reports)
        {
            return ReportTrans(reports, EnumTransactionStatus.None);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult ReportTrans(EnumReports reports, ReportParamViewModel viewModel, FormCollection formCollection, EnumReportGroupBy? groupBy = null)
        {
            return Report(reports, viewModel, formCollection, groupBy);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Report(EnumReports reports, ReportParamViewModel viewModel, FormCollection formCollection, EnumReportGroupBy? groupBy = null)
        {
            //LocalReport localReport = new LocalReport();
            //localReport.ReportPath = Server.MapPath(string.Format("~/Views/Transaction/Report/{0}.rdlc", reports.ToString()));

            ReportDataSource[] repCol = new ReportDataSource[1];
            ReportParameter[] parameters = null;
            switch (reports)
            {
                case EnumReports.RptBrand:
                    repCol[0] = GetBrand();
                    break;
                case EnumReports.RptCostCenter:
                    repCol[0] = GetCostCenter();
                    break;
                case EnumReports.RptNeraca:
                    repCol[0] = GetRecAccount(EnumAccountCatType.NERACA, viewModel.CostCenterId, viewModel.RecPeriodId, viewModel.GenerateDetail);
                    break;
                case EnumReports.RptLR:
                    repCol[0] = GetRecAccount(EnumAccountCatType.LR, viewModel.CostCenterId, viewModel.RecPeriodId, viewModel.GenerateDetail);
                    break;
                case EnumReports.RptNeracaSum:
                    repCol[0] = GetRecAccount(EnumAccountCatType.NERACA, viewModel.CostCenterId, viewModel.RecPeriodId, viewModel.GenerateDetail);
                    break;
                case EnumReports.RptLRSum:
                    repCol[0] = GetRecAccount(EnumAccountCatType.LR, viewModel.CostCenterId, viewModel.RecPeriodId, viewModel.GenerateDetail);
                    break;
                case EnumReports.RptStockCard:
                    repCol[0] = GetStockCard(viewModel.DateFrom, viewModel.DateTo, viewModel.ItemId, viewModel.WarehouseId);
                    break;
                case EnumReports.RptStockItem:
                    repCol[0] = GetStockItem(viewModel.ItemId, viewModel.WarehouseId);
                    break;
                case EnumReports.RptAnalyzeBudgetDetail:
                    repCol[0] = GetTransDetForBudget(viewModel.ItemId, viewModel.WarehouseId, viewModel.DateFrom.Value, viewModel.DateTo.Value);
                    break;
                case EnumReports.RptTransDetail:
                    EnumTransactionStatus stat =
                        (EnumTransactionStatus)Enum.Parse(typeof(EnumTransactionStatus), formCollection["TransStatus"]);
                    repCol[0] = GetTransTotal(viewModel.DateFrom, viewModel.DateTo, viewModel.WarehouseId, stat);
                    break;
                case EnumReports.RptTransRecap:
                    stat =
                        (EnumTransactionStatus)Enum.Parse(typeof(EnumTransactionStatus), formCollection["TransStatus"]);
                    parameters = new ReportParameter[3];
                    parameters[0] = new ReportParameter("ParamTitle", string.Format(CommonHelper.GetStringValue(EnumReports.RptTransRecap), CommonHelper.GetStringValue(stat), CommonHelper.GetStringValue(groupBy)));
                    parameters[1] = new ReportParameter("ParamGroupBy", groupBy.ToString());
                    parameters[2] = new ReportParameter("ParamGroupByTitle", CommonHelper.GetStringValue(groupBy));
                    repCol[0] = GetTransTotal(viewModel.DateFrom, viewModel.DateTo, viewModel.WarehouseId, stat);
                    break;
                case EnumReports.RptItem:
                    repCol[0] = GetItemViewModel();
                    break;
                case EnumReports.RptJournal:
                    repCol[0] = GetJournalDet(viewModel.DateFrom, viewModel.DateTo, viewModel.CostCenterId, viewModel.AccountId);
                    break;
                case EnumReports.RptBukuBesar:
                    repCol[0] = GetJournalDetFlow(viewModel.DateFrom, viewModel.DateTo, viewModel.CostCenterId, viewModel.AccountId, viewModel.AccountIdTo);
                    break;
                case EnumReports.RptJournalByCostCenter:
                    repCol[0] = GetJournalDet(viewModel.DateFrom, viewModel.DateTo, viewModel.CostCenterId, viewModel.AccountId);
                    break;
                case EnumReports.RptBukuBesarByCostCenter:
                    repCol[0] = GetJournalDetFlow(viewModel.DateFrom, viewModel.DateTo, viewModel.CostCenterId, viewModel.AccountId, viewModel.AccountIdTo);
                    break;
            }
            Session["ReportData"] = repCol;
            Session["ReportParam"] = parameters;

            var e = new
            {
                Success = true,
                Message = "redirect",
                UrlReport = string.Format("{0}", reports.ToString())
            };
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        private ReportDataSource GetJournalDetFlow(DateTime? dateFrom, DateTime? dateTo, string costCenterId, string accountId, string accountIdTo)
        {
            IList<VJournalDetFlow> dets = _vJournalDetFlowRepository.GetForReport(dateFrom, dateTo, costCenterId, accountId, accountIdTo);

            var list = from det in dets
                       select new
                       {
                           det.JournalDetAmmount,
                           det.JournalDetStatus,
                           det.JournalDetDesc,
                           det.JournalDetEvidenceNo,
                           JournalVoucherNo = det.JournalId != null ? det.JournalId.JournalVoucherNo : null,
                           CostCenterId = det.JournalId != null ? det.JournalId.CostCenterId != null ? det.JournalId.CostCenterId.Id : null : null,
                           CostCenterName = det.JournalId != null ? det.JournalId.CostCenterId != null ? det.JournalId.CostCenterId.CostCenterName : null : null,
                           JournalDate = det.JournalId != null ? det.JournalId.JournalDate : null,
                           AccountId = det.AccountId != null ? det.AccountId.Id : null,
                           AccountName = det.AccountId != null ? det.AccountId.AccountName : null,
                           det.Saldo,
                           det.RowNumber,
                           JournalPic = det.JournalId != null ? det.JournalId.JournalPic : null,
                           JournalPic2 = det.JournalId != null ? det.JournalId.JournalPic2 : null
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("JournalDetFlowViewModel", list.ToList());
            return reportDataSource;
        }

        private ReportDataSource GetItemViewModel()
        {
            IList<MItem> listItems = _mItemRepository.GetAll();

            var list = from det in listItems
                       select new
                       {
                           det.Id,
                           det.ItemName,
                           det.ItemDesc,
                           ItemCatId = det.ItemCatId.Id,
                           det.ItemCatId.ItemCatName,
                           det.ItemUoms[0].ItemUomPurchasePrice
                       }
         ;

            ReportDataSource reportDataSource = new ReportDataSource("ItemViewModel", list.ToList());
            return reportDataSource;

        }

        private ReportDataSource GetTransTotal(DateTime? dateFrom, DateTime? dateTo, string warehouseId, EnumTransactionStatus transStatus)
        {
            Check.Require(transStatus != EnumTransactionStatus.None, "transStatus may not be None");
            IList<TTransDet> dets = _tTransDetRepository.GetByDateWarehouse(dateFrom, dateTo, warehouseId, transStatus);
            string TransName = Helper.CommonHelper.GetStringValue(transStatus);

            TransactionFormViewModel viewModel = new TransactionFormViewModel();
            Helper.CommonHelper.SetViewModelByStatus(viewModel, transStatus);

            var list = from det in dets
                       select new
                       {
                           det.Id,
                           det.TransDetNo,
                           det.TransDetQty,
                           det.TransDetDesc,
                           det.TransDetTotal,
                           det.TransDetPrice,
                           det.TransDetDisc,
                           ItemId = det.ItemId.Id,
                           det.ItemId.ItemName,
                           ItemUom = det.ItemId.ItemUom.ItemUomName,
                           SupplierName = GetSupplierName(det.TransId.TransBy),
                           det.TransId.TransFactur,
                           det.TransId.TransDate,
                           WarehouseId = det.TransId.WarehouseId.Id,
                           det.TransId.WarehouseId.WarehouseName,
                           WarehouseToName =
                det.TransId.WarehouseIdTo != null ? det.TransId.WarehouseIdTo.WarehouseName : null,
                           det.TransId.TransStatus,
                           det.TransId.TransDesc,
                           det.TransId.TransSubTotal,
                           det.TransId.TransPaymentMethod,
                           TransId = det.TransId.Id,
                           viewModel.ViewWarehouse,
                           viewModel.ViewWarehouseTo,
                           viewModel.ViewSupplier,
                           viewModel.ViewDate,
                           viewModel.ViewFactur,
                           viewModel.ViewPrice,
                           viewModel.ViewPaymentMethod,
                           viewModel.ViewJobType,
                           viewModel.ViewUnitType,
                           TransName,
                           JobTypeId = det.TransId.JobTypeId != null ? det.TransId.JobTypeId.Id : null,
                           JobTypeName = det.TransId.JobTypeId != null ? det.TransId.JobTypeId.JobTypeName : null,
                           UnitTypeId = det.TransId.UnitTypeId != null ? det.TransId.UnitTypeId.Id : null,
                           UnitTypeName = det.TransId.UnitTypeId != null ? det.TransId.UnitTypeId.UnitTypeName : null
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("TransTotalViewModel", list.ToList());
            return reportDataSource;
        }

        private object GetSupplierName(string supplierId)
        {
            IDictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", supplierId);

            MSupplier supplier = _mSupplierRepository.FindOne(param);
            if (supplier != null)
                return supplier.SupplierName;
            else
                return string.Empty;
        }

        private ReportDataSource GetTransDetForBudget(string itemId, string warehouseId, DateTime dateFrom, DateTime dateTo)
        {
            IList<TTransDet> dets;
            MItem item = null;
            MWarehouse warehouse = null;
            if (!string.IsNullOrEmpty(itemId))
                item = _mItemRepository.Get(itemId);
            if (!string.IsNullOrEmpty(warehouseId))
                warehouse = _mWarehouseRepository.Get(warehouseId);
            dets = _tTransDetRepository.GetByItemWarehouse(item, warehouse);

            var list = from det in dets
                       select new
                                  {
                                      det.TransDetNo,
                                      det.TransDetQty,
                                      det.TransDetDesc,
                                      det.TransDetTotal,
                                      det.TransDetPrice,
                                      det.TransDetDisc,
                                      ItemId = det.ItemId.Id,
                                      det.ItemId.ItemName,
                                      WarehouseId = det.TransId.WarehouseId.Id,
                                      det.TransId.WarehouseId.WarehouseName,
                                      TotalUsed = _tTransDetRepository.GetTotalUsed(det.ItemId, det.TransId.WarehouseId, dateFrom, dateTo, EnumTransactionStatus.Using.ToString()),
                                      RealPercentValue = GetRealValue(det.TransId.WarehouseId.CostCenterId, dateFrom, dateTo)
                                  }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("TransDetViewModel", list.ToList());
            return reportDataSource;
        }

        //get real value
        private object GetRealValue(MCostCenter mCostCenter, DateTime dateFrom, DateTime dateTo)
        {
            TReal real = _tRealRepository.GetByCostCenterAndDate(mCostCenter, dateFrom, dateTo);
            if (real != null)
            {
                return real.RealPercentValue;
            }
            return null;
        }

        private ReportDataSource GetStockItem(string itemId, string warehouseId)
        {
            IList<TStockItem> stockItems;
            MItem item = null;
            MWarehouse warehouse = null;
            if (!string.IsNullOrEmpty(itemId))
                item = _mItemRepository.Get(itemId);
            if (!string.IsNullOrEmpty(warehouseId))
                warehouse = _mWarehouseRepository.Get(warehouseId);
            stockItems = _tStockItemRepository.GetByItemWarehouse(item, warehouse);

            var list = from stock in stockItems
                       select new
                       {
                           stock.ItemStock,
                           stock.ItemStockMax,
                           stock.ItemStockMin,
                           ItemId = stock.ItemId.Id,
                           stock.ItemId.ItemName,
                           WarehouseId = stock.WarehouseId.Id,
                           stock.WarehouseId.WarehouseName,
                           stock.ItemStockRack
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("StockItemViewModel", list.ToList());
            return reportDataSource;
        }

        private ReportDataSource GetStockCard(DateTime? dateFrom, DateTime? dateTo, string itemId, string warehouseId)
        {
            IList<VStockCardFlow> cards;
            //MItem item = null;
            //MWarehouse warehouse = null;
            //if (!string.IsNullOrEmpty(itemId))
            //    item = _mItemRepository.Get(itemId);
            //if (!string.IsNullOrEmpty(warehouseId))
            //    warehouse = _mWarehouseRepository.Get(warehouseId);
            cards = _vStockCardFlowRepository.GetByDateItemWarehouse(dateFrom, dateTo, itemId, warehouseId);

            var list = from card in cards
                       select new
                       {
                           card.StockCardQty,
                           card.StockCardDate,
                           card.StockCardStatus,
                           ItemId = card.ItemId.Id,
                           card.ItemId.ItemName,
                           WarehouseId = card.WarehouseId.Id,
                           card.WarehouseId.WarehouseName,
                           StockCardSaldo = card.Saldo,
                           card.StockCardDesc,
                           TransFactur = card.TransDetId != null ? card.TransDetId.TransId.TransFactur : null
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("StockCardViewModel", list.ToList());
            return reportDataSource;
        }

        private ReportDataSource GetRecAccount(EnumAccountCatType accountCatType, string costCenterId, string recPeriodId, bool generateDetail)
        {
            //if generatedetail is false, generate only level 1
            IList<string> listLevel2Account = new List<string>();
            if (!generateDetail)
            {
                //get account to level 2
                listLevel2Account = _mAccountRepository.GetLevelAccounts(0, accountCatType.ToString());
            }

            IList<TRecAccount> dets = _tRecAccountRepository.GetByAccount(listLevel2Account, costCenterId, recPeriodId);
            //IList<TRecAccount> dets = _tRecAccountRepository.GetByAccountType(accountCatType.ToString(), costCenterId, recPeriodId);

            var list = from det in dets
                       select new
                       {
                           det.RecAccountStart,
                           det.RecAccountEnd,
                           det.RecAccountDesc,
                           CostCenterId = det.CostCenterId.Id,
                           det.CostCenterId.CostCenterName,
                           AccountId = det.AccountId.Id,
                           det.AccountId.AccountName,
                           AccountCatId = det.AccountId.AccountCatId.Id,
                           det.AccountId.AccountCatId.AccountCatName,
                           AccountParentId = det.AccountId.AccountParentId != null ? det.AccountId.AccountParentId.Id : ""
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("RecAccountViewModel", list.ToList());
            return reportDataSource;
        }

        private ReportDataSource GetJournalDet(DateTime? dateFrom, DateTime? dateTo, string costCenterId, string accountId)
        {
            IList<TJournalDet> dets = _tJournalDetRepository.GetForReport(dateFrom, dateTo, costCenterId, accountId);

            var list = from det in dets
                       select new
                                  {
                                      det.JournalDetAmmount,
                                      det.JournalDetStatus,
                                      det.JournalDetDesc,
                                      det.JournalDetEvidenceNo,
                                      det.JournalId.JournalVoucherNo,
                                      CostCenterId = det.JournalId.CostCenterId != null ? det.JournalId.CostCenterId.Id : null,
                                      CostCenterName = det.JournalId.CostCenterId != null ? det.JournalId.CostCenterId.CostCenterName : null,
                                      det.JournalId.JournalDate,
                                      AccountId = det.AccountId != null ? det.AccountId.Id : null,
                                      AccountName = det.AccountId != null ? det.AccountId.AccountName : null,
                                      JournalPic = det.JournalId != null ? det.JournalId.JournalPic : null,
                                      JournalPic2 = det.JournalId != null ? det.JournalId.JournalPic2 : null
                                  }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("JournalDetViewModel", list.ToList());
            return reportDataSource;
        }

        private ReportDataSource GetCostCenter()
        {
            ReportDataSource reportDataSource = new ReportDataSource("CostCenter", _mCostCenterRepository.GetAll());
            return reportDataSource;
        }

        private ReportDataSource GetBrand()
        {
            ReportDataSource reportDataSource = new ReportDataSource("Brand", _mBrandRepository.GetAll());
            return reportDataSource;
        }

    }

    public enum EnumViewTrans
    {
        ViewWarehouse,
        ViewWarehouseTo,
        ViewSupplier,
        ViewDate,
        ViewFactur,
        ViewPrice,
        ViewPaymentMethod
    }
}
