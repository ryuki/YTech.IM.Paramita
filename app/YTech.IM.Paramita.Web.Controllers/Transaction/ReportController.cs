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
using YTech.IM.Paramita.Web.Controllers.ViewModel;
using Microsoft.Reporting.WebForms;

namespace YTech.IM.Paramita.Web.Controllers.Transaction
{
    [HandleError]
    public class ReportController : Controller
    {
        //public ReportController()
        //    : this(new TJournalRepository(), new TJournalDetRepository(), new MCostCenterRepository(), new MAccountRepository(), new TRecAccountRepository(), new TRecPeriodRepository(), new MBrandRepository(), new MSupplierRepository(), new MWarehouseRepository(), new MItemRepository(), new TStockCardRepository(), new TStockItemRepository(), new TTransDetRepository())
        //{ }

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

        public ReportController(ITJournalRepository tJournalRepository, ITJournalDetRepository tJournalDetRepository, IMCostCenterRepository mCostCenterRepository, IMAccountRepository mAccountRepository, ITRecAccountRepository tRecAccountRepository, ITRecPeriodRepository tRecPeriodRepository, IMBrandRepository mBrandRepository, IMSupplierRepository mSupplierRepository, IMWarehouseRepository mWarehouseRepository, IMItemRepository mItemRepository, ITStockCardRepository tStockCardRepository, ITStockItemRepository tStockItemRepository, ITTransDetRepository tTransDetRepository, ITRealRepository tRealRepository, IVJournalDetFlowRepository vJournalDetFlowRepository)
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
        }

        [Transaction]
        public ActionResult ReportTrans(EnumReports reports, EnumTransactionStatus TransStatus)
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
                case EnumReports.RptJournal:
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    break;
                case EnumReports.RptNeraca:
                    viewModel.ShowCostCenter = true;
                    viewModel.ShowRecPeriod = true;
                    break;
                case EnumReports.RptLR:
                    viewModel.ShowCostCenter = true;
                    viewModel.ShowRecPeriod = true;
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
                            break;
                        case EnumTransactionStatus.Received:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            break;
                        case EnumTransactionStatus.Purchase:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
                            break;
                        case EnumTransactionStatus.ReturPurchase:
                            viewModel.ShowDateFrom = true;
                            viewModel.ShowDateTo = true;
                            viewModel.ShowWarehouse = true;
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
                case EnumReports.RptBukuBesar:
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    break;
                case EnumReports.RptJournalByCostCenter:
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowCostCenter = true;
                    break;
                case EnumReports.RptBukuBesarByCostCenter:
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowCostCenter = true;
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
        public ActionResult ReportTrans(EnumReports reports, ReportParamViewModel viewModel, FormCollection formCollection)
        {
            return Report(reports, viewModel, formCollection);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Report(EnumReports reports, ReportParamViewModel viewModel, FormCollection formCollection)
        {
            //LocalReport localReport = new LocalReport();
            //localReport.ReportPath = Server.MapPath(string.Format("~/Views/Transaction/Report/{0}.rdlc", reports.ToString()));

            ReportDataSource[] repCol = new ReportDataSource[1];
            switch (reports)
            {
                case EnumReports.RptBrand:
                    repCol[0] = GetBrand();
                    break;
                case EnumReports.RptCostCenter:
                    repCol[0] = GetCostCenter();
                    break;
                case EnumReports.RptJournal:
                    repCol[0] = GetJournalDet(viewModel.DateFrom, viewModel.DateTo, viewModel.CostCenterId);
                    break;
                case EnumReports.RptNeraca:
                    repCol[0] = GetRecAccount(EnumAccountCatType.NERACA, viewModel.CostCenterId, viewModel.RecPeriodId);
                    break;
                case EnumReports.RptLR:
                    repCol[0] = GetRecAccount(EnumAccountCatType.LR, viewModel.CostCenterId, viewModel.RecPeriodId);
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
                case EnumReports.RptItem:
                    repCol[0] = GetItemViewModel();
                    break;
                case EnumReports.RptBukuBesar:
                    repCol[0] = GetJournalDetFlow(viewModel.DateFrom, viewModel.DateTo, viewModel.CostCenterId);
                    break;
                case EnumReports.RptJournalByCostCenter:
                    repCol[0] = GetJournalDet(viewModel.DateFrom, viewModel.DateTo, viewModel.CostCenterId);
                    break;
                case EnumReports.RptBukuBesarByCostCenter:
                    repCol[0] = GetJournalDetFlow(viewModel.DateFrom, viewModel.DateTo, viewModel.CostCenterId);
                    break;
            }
            Session["ReportData"] = repCol;

            var e = new
            {
                Success = true,
                Message = "redirect",
                UrlReport = string.Format("{0}", reports.ToString())
            };
            return Json(e, JsonRequestBehavior.AllowGet);

            //string reportType = formCollection["ExportFormat"];
            //string mimeType, encoding, fileNameExtension;

            ////The DeviceInfo settings should be changed based on the reportType
            ////http://msdn2.microsoft.com/en-us/library/ms155397.aspx

            //string deviceInfo =
            //"<DeviceInfo>" +
            //string.Format("  <OutputFormat>{0}</OutputFormat>", formCollection["ExportFormat"]) +
            //"  <PageWidth>8.5in</PageWidth>" +
            //"  <PageHeight>11in</PageHeight>" +
            //"  <MarginTop>0.5in</MarginTop>" +
            //"  <MarginLeft>0.5in</MarginLeft>" +
            //"  <MarginRight>0.5in</MarginRight>" +
            //"  <MarginBottom>0.5in</MarginBottom>" +
            //"</DeviceInfo>";

            //Warning[] warnings;
            //string[] streams;
            //byte[] renderedBytes;

            ////Render the report
            //renderedBytes = localReport.Render(
            //    reportType,
            //    deviceInfo,
            //    out mimeType,
            //    out encoding,
            //    out fileNameExtension,
            //    out streams,
            //    out warnings);

            //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.{1}", reports.ToString(), fileNameExtension));

            //return File(renderedBytes, mimeType);
        }

        private ReportDataSource GetJournalDetFlow(DateTime? dateFrom, DateTime? dateTo, string costCenterId)
        {
            IList<VJournalDetFlow> dets = _vJournalDetFlowRepository.GetForReport(dateFrom, dateTo, costCenterId);

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
                           det.RowNumber
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
                                      SupplierName = det.TransId.TransBy,
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
                                      ViewWarehouse = SetView(det.TransId.TransStatus, EnumViewTrans.ViewWarehouse),
                                      ViewWarehouseTo = SetView(det.TransId.TransStatus, EnumViewTrans.ViewWarehouseTo),
                                      ViewSupplier = SetView(det.TransId.TransStatus, EnumViewTrans.ViewSupplier),
                                      ViewDate = SetView(det.TransId.TransStatus, EnumViewTrans.ViewDate),
                                      ViewFactur = SetView(det.TransId.TransStatus, EnumViewTrans.ViewFactur),
                                      ViewPrice = SetView(det.TransId.TransStatus, EnumViewTrans.ViewPrice),
                                      ViewPaymentMethod =
                           SetView(det.TransId.TransStatus, EnumViewTrans.ViewPaymentMethod),
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

        private bool SetView(string TransStatus, EnumViewTrans viewTrans)
        {
            return true;
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
            IList<TStockCard> cards;
            MItem item = null;
            MWarehouse warehouse = null;
            if (!string.IsNullOrEmpty(itemId))
                item = _mItemRepository.Get(itemId);
            if (!string.IsNullOrEmpty(warehouseId))
                warehouse = _mWarehouseRepository.Get(warehouseId);
            cards = _tStockCardRepository.GetByDateItemWarehouse(dateFrom, dateTo, item, warehouse);

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
                           card.StockCardSaldo,
                           card.StockCardDesc,
                           TransFactur = card.TransDetId != null ? card.TransDetId.TransId.TransFactur : null
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("StockCardViewModel", list.ToList());
            return reportDataSource;
        }

        private ReportDataSource GetRecAccount(EnumAccountCatType accountCatType, string costCenterId, string recPeriodId)
        {
            IList<TRecAccount> dets;
            TRecPeriod recPeriod = _tRecPeriodRepository.Get(recPeriodId);
            if (!string.IsNullOrEmpty(costCenterId))
            {
                MCostCenter costCenter = _mCostCenterRepository.Get(costCenterId);
                dets = _tRecAccountRepository.GetByAccountType(accountCatType.ToString(), costCenter, recPeriod);
            }
            else
            {
                dets = _tRecAccountRepository.GetByAccountType(accountCatType.ToString(), null, recPeriod);
            }

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

        private ReportDataSource GetJournalDet(DateTime? dateFrom, DateTime? dateTo, string costCenterId)
        {
            IList<TJournalDet> dets = _tJournalDetRepository.GetForReport(dateFrom, dateTo, costCenterId);

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
                                      AccountName = det.AccountId != null ? det.AccountId.AccountName : null
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
