using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.IM.Paramita.Core.Transaction.Accounting;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel.Reports
{
    public class CashJournalViewModel : TJournalDet
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string JournalDate { get; set; }
        public string CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public string JournalVoucherNo { get; set; }
        public string JournalId { get; set; }
        public string JournalPic { get; set; }
        public string JournalPic2 { get; set; }
        public string JournalType { get; set; }
        public string DetAccountId { get; set; }
        public string DetAccountName { get; set; }
    }
}
