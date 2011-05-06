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

        public ReportController(ITJournalRepository tJournalRepository, ITJournalDetRepository tJournalDetRepository, IMCostCenterRepository mCostCenterRepository, IMAccountRepository mAccountRepository, ITRecAccountRepository tRecAccountRepository, ITRecPeriodRepository tRecPeriodRepository, IMBrandRepository mBrandRepository, IMSupplierRepository mSupplierRepository, IMWarehouseRepository mWarehouseRepository, IMItemRepository mItemRepository, ITStockCardRepository tStockCardRepository, ITStockItemRepository tStockItemRepository, ITTransDetRepository tTransDetRepository, ITRealRepository tRealRepository)
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
        }

        [Transaction]
        public ActionResult ReportTrans(EnumReports reports, EnumTransactionStatus TransStatus)
        {
            ReportParamViewModel viewModel = ReportParamViewModel.CreateReportParamViewModel(_mCostCenterRepository, _mWarehouseRepository, _mSupplierRepository, _tRecPeriodRepository, _mItemRepository);
            if (TransStatus != EnumTransactionStatus.None)
            {
                viewModel.TransStatus = TransStatus;
            }
            string title = string.Empty;
            switch (reports)
            {
                case EnumReports.RptBrand:
                    title = "Daftar Master Merek";

                    break;
                case EnumReports.RptCostCenter:
                    title = "Daftar Master Cost Center";
                    break;
                case EnumReports.RptJournal:
                    title = "Daftar Jurnal";
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowCostCenter = true;
                    break;
                case EnumReports.RptNeraca:
                    title = "Laporan Neraca";
                    viewModel.ShowCostCenter = true;
                    viewModel.ShowRecPeriod = true;
                    break;
                case EnumReports.RptLR:
                    title = "Laporan Laba / Rugi";
                    viewModel.ShowCostCenter = true;
                    viewModel.ShowRecPeriod = true;
                    break;
                case EnumReports.RptStockCard:
                    title = "Kartu Stok";
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowItem = true;
                    viewModel.ShowWarehouse = true;
                    break;
                case EnumReports.RptStockItem:
                    title = "Lap. Stok Per Gudang";
                    viewModel.ShowItem = true;
                    viewModel.ShowWarehouse = true;
                    break;
                case EnumReports.RptAnalyzeBudgetDetail:
                    title = "Lap. Analisa Budget";
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowItem = true;
                    viewModel.ShowWarehouse = true;
                    break;
                case EnumReports.RptTransDetail:
                    //switch (viewModel.TransStatus)
                    //{
                    //    case EnumTransactionStatus.PurchaseOrder:
                    title = "Lap. Detail";
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowWarehouse = true;
                    //        break;
                    //}

                    break;
                case EnumReports.RptItem:
                    title = "Daftar Master Produk";

                    break;
                case EnumReports.RptBukuBesar:
                    title = "Lap. Buku Besar";
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
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(string.Format("~/Views/Transaction/Report/{0}.rdlc", reports.ToString()));

            switch (reports)
            {
                case EnumReports.RptBrand:
                    localReport.DataSources.Add(GetBrand());
                    break;
                case EnumReports.RptCostCenter:
                    localReport.DataSources.Add(GetCostCenter());
                    break;
                case EnumReports.RptJournal:
                    localReport.DataSources.Add(GetJournalDet(viewModel.DateFrom, viewModel.DateTo, viewModel.CostCenterId));
                    break;
                case EnumReports.RptNeraca:
                    localReport.DataSources.Add(GetRecAccount(EnumAccountCatType.NERACA, viewModel.CostCenterId, viewModel.RecPeriodId));
                    break;
                case EnumReports.RptLR:
                    localReport.DataSources.Add(GetRecAccount(EnumAccountCatType.LR, viewModel.CostCenterId, viewModel.RecPeriodId));
                    break;
                case EnumReports.RptStockCard:
                    localReport.DataSources.Add(GetStockCard(viewModel.DateFrom, viewModel.DateTo, viewModel.ItemId, viewModel.WarehouseId));
                    break;
                case EnumReports.RptStockItem:
                    localReport.DataSources.Add(GetStockItem(viewModel.ItemId, viewModel.WarehouseId));
                    break;
                case EnumReports.RptAnalyzeBudgetDetail:
                    localReport.DataSources.Add(GetTransDetForBudget(viewModel.ItemId, viewModel.WarehouseId, viewModel.DateFrom.Value, viewModel.DateTo.Value));
                    break;
                case EnumReports.RptTransDetail:
                    EnumTransactionStatus stat =
                        (EnumTransactionStatus)Enum.Parse(typeof(EnumTransactionStatus), formCollection["TransStatus"]);
                    localReport.DataSources.Add(GetTransTotal(viewModel.DateFrom, viewModel.DateTo, viewModel.WarehouseId, stat));
                    break;
                case EnumReports.RptItem:
                    localReport.DataSources.Add(GetItemViewModel());
                    break;
                case EnumReports.RptBukuBesar:
                    localReport.DataSources.Add(GetJournalDet(viewModel.DateFrom, viewModel.DateTo, viewModel.CostCenterId));
                    break;
            }

            string reportType = formCollection["ExportFormat"];
            string mimeType, encoding, fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx

            string deviceInfo =
            "<DeviceInfo>" +
            string.Format("  <OutputFormat>{0}</OutputFormat>", formCollection["ExportFormat"]) +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.{1}", reports.ToString(), fileNameExtension));

            return File(renderedBytes, mimeType);
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

            ReportDataSource reportDataSource = new ReportDataSource("ItemViewModel", list);
            return reportDataSource;

        }

        private ReportDataSource GetTransTotal(DateTime? dateFrom, DateTime? dateTo, string warehouseId, EnumTransactionStatus transStatus)
        {
            Check.Require(transStatus != EnumTransactionStatus.None, "transStatus may not be None");
            IList<TTransDet> dets;
            MWarehouse warehouse = null;
            if (!string.IsNullOrEmpty(warehouseId))
                warehouse = _mWarehouseRepository.Get(warehouseId);
            dets = _tTransDetRepository.GetByDateWarehouse(dateFrom, dateTo, warehouse, transStatus.ToString());

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
                                      TransName = Helper.CommonHelper.GetStringValue(transStatus)
                                  }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("TransTotalViewModel", list);
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

            ReportDataSource reportDataSource = new ReportDataSource("TransDetViewModel", list);
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

            ReportDataSource reportDataSource = new ReportDataSource("StockItemViewModel", list);
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
                           card.TransDetId.TransId.TransFactur
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("StockCardViewModel", list);
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

            ReportDataSource reportDataSource = new ReportDataSource("RecAccountViewModel", list);
            return reportDataSource;
        }

        private ReportDataSource GetJournalDet(DateTime? dateFrom, DateTime? dateTo, string costCenterId)
        {
            IList<TJournalDet> dets;
            if (!string.IsNullOrEmpty(costCenterId))
            {
                MCostCenter costCenter = _mCostCenterRepository.Get(costCenterId);
                dets = _tJournalDetRepository.GetForReport(dateFrom, dateTo, costCenter);
            }
            else
            {
                dets = _tJournalDetRepository.GetForReport(dateFrom, dateTo, null);
            }

            var list = from det in dets
                       select new
                                  {
                                      det.JournalDetAmmount,
                                      det.JournalDetStatus,
                                      det.JournalDetDesc,
                                      det.JournalDetEvidenceNo,
                                      det.JournalId.JournalVoucherNo,
                                      CostCenterId = det.JournalId.CostCenterId.Id,
                                      det.JournalId.CostCenterId.CostCenterName,
                                      det.JournalId.JournalDate,
                                      AccountId = det.AccountId.Id,
                                      det.AccountId.AccountName
                                  }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("JournalDetViewModel", list);
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
