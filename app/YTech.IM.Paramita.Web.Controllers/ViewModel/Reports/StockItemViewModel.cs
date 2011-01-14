using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel.Reports
{
    public class StockItemViewModel : TStockItem
    {
        public string ItemName { get; set; }
        public string WarehouseName { get; set; }
    }
}
