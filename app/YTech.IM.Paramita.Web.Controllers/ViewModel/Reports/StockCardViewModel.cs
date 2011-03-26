using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel.Reports
{
    public class StockCardViewModel : TStockCard
    {
        public string ItemName { get; set; }
        public string WarehouseName { get; set; }
        public string TransFactur { get; set; }
    }
}
