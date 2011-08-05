using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel
{
    public class ReportParamViewModel
    {
        public static ReportParamViewModel CreateReportParamViewModel(IMCostCenterRepository mCostCenterRepository, IMWarehouseRepository mWarehouseRepository, IMSupplierRepository mSupplierRepository,ITRecPeriodRepository tRecPeriodRepository,IMItemRepository mItemRepository)
        {
            ReportParamViewModel viewModel = new ReportParamViewModel();

            IList<MCostCenter> list = mCostCenterRepository.GetAll();
            MCostCenter costCenter = new MCostCenter();
            costCenter.CostCenterName = "-Semua Cost Center-";
            list.Insert(0, costCenter);
            viewModel.CostCenterList = new SelectList(list, "Id", "CostCenterName");

            IList<MWarehouse> listWarehouse = mWarehouseRepository.GetAll();
            MWarehouse account = new MWarehouse();
            account.WarehouseName = "-Semua Gudang-";
            listWarehouse.Insert(0, account);
            viewModel.WarehouseList = new SelectList(listWarehouse, "Id", "WarehouseName");

            IList<MSupplier> listSupplier = mSupplierRepository.GetAll();
            MSupplier supplier = new MSupplier();
            supplier.SupplierName = "-Semua Supplier-";
            listSupplier.Insert(0, supplier);
            viewModel.SupplierList = new SelectList(listSupplier, "Id", "SupplierName");

            IList<TRecPeriod> listRecPeriod = tRecPeriodRepository.GetAll();
            TRecPeriod recPeriod = new TRecPeriod();
            recPeriod.PeriodDesc = "-Pilih Period-";
            listRecPeriod.Insert(0, recPeriod);
            viewModel.RecPeriodList = new SelectList(listRecPeriod, "Id", "PeriodDesc");

            IList<MItem> listItem = mItemRepository.GetAll();
            MItem item = new MItem();
            item.ItemName = "-Semua Item-";
            listItem.Insert(0, item);
            viewModel.ItemList = new SelectList(listItem, "Id", "ItemName");

            viewModel.DateFrom = DateTime.Today;
            viewModel.DateTo = DateTime.Today;
            return viewModel;
        }

        public bool ShowReport { get; internal set; }
        public string ExportFormat { get; internal set; }
        public string Title { get; internal set; }


        public bool ShowDateFrom { get; internal set; }
        public bool ShowDateTo { get; internal set; }
        public bool ShowCostCenter { get; internal set; }
        public bool ShowWarehouse { get; internal set; }
        public bool ShowSupplier { get; internal set; }
        public bool ShowRecPeriod { get; internal set; }
        public bool ShowItem { get; internal set; }
        public bool ShowAccount { get; internal set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string CostCenterId { get; set; }
        public string WarehouseId { get; set; }
        public string SupplierId { get; set; }
        public string RecPeriodId { get; set; }
        public string ItemId { get; set; }
        public string AccountId { get; set; }

        public SelectList CostCenterList { get; internal set; }
        public SelectList WarehouseList { get; internal set; }
        public SelectList SupplierList { get; internal set; }
        public SelectList RecPeriodList { get; internal set; }
        public SelectList ItemList { get; internal set; }

        public EnumTransactionStatus TransStatus { get; internal set; }

        
    }
}
