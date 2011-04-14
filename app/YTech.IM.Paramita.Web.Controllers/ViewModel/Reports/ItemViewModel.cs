using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel.Reports
{
   public class ItemViewModel : MItem
    {
       public string ItemCatName { get; set; }
       public decimal? ItemUomPurchasePrice { get; set; }
    }
}
