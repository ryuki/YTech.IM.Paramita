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
using YTech.IM.Paramita.Web.Controllers.Helper;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel
{
    public class CashFormViewModel
    {
        public static CashFormViewModel CreateCashFormViewModel(ITJournalRepository tJournalRepository, IMCostCenterRepository mCostCenterRepository, IMAccountRepository mAccountRepository)
        {
            CashFormViewModel viewModel = new CashFormViewModel();

            viewModel.CostCenterList = FillHelper.GetCostCenters(mCostCenterRepository, "-Pilih Cost Center-");
            viewModel.AccountList = FillHelper.GetAccounts(mAccountRepository, "-Pilih Akun Kas-");

            return viewModel;
        }

        public TJournal Journal { get; internal set; }
        public IList<TJournalDet> ListOfJournalDet { get; internal set; }

        public SelectList CostCenterList { get; internal set; }
        public SelectList AccountList { get; internal set; }
        public string Title { get; internal set; }
    }
}
