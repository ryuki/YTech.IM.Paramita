using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel
{
    public class TransDetViewModel
    {
        public TTransDet TransDet { get; internal set; }
        public bool IsNew { get; internal set; }
    }
}
