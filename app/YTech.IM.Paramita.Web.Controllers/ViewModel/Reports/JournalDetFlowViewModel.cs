using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Core.View;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel.Reports
{
    public class JournalDetFlowViewModel : VJournalDetFlow
    {
        public string AccountName { get; set; }
        public string JournalDate { get; set; }
        public string CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public string JournalVoucherNo { get; set; }
    }
}
