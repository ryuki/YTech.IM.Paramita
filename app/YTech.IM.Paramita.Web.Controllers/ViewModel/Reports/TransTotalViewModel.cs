using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel.Reports
{
    public class TransTotalViewModel : TTransDet
    {
        public string ItemName { get; set; }
        public string ItemUom { get; set; }
        public string SupplierName { get; set; }
        public string TransFactur { get; set; }
        public DateTime? TransDate { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseIdTo { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseToName { get; set; }
        public string TransStatus { get; set; }
        public string TransDesc { get; set; }
        public decimal? TransSubTotal { get; set; }
        public string TransPaymentMethod { get; set; }
        public string TransName { get; set; }
        public string JobTypeName { get; set; }
        public string UnitTypeName { get; set; }
        public string JobTypeId { get; set; }
        public string UnitTypeId { get; set; }

        public bool ViewWarehouse { get;  set; }
        public bool ViewWarehouseTo { get;  set; }
        public bool ViewSupplier { get;  set; }
        public bool ViewDate { get;  set; }
        public bool ViewFactur { get;  set; }
        public bool ViewPrice { get;  set; }
        public bool ViewPaymentMethod { get; set; }
        public bool ViewJobType { get; set; }
        public bool ViewUnitType { get; set; }
    }
}
