﻿using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface ITRecAccountRepository : INHibernateRepositoryWithTypedId<TRecAccount, string>
    {
        void RunClosing (TRecPeriod recPeriod);
        IList<TRecAccount> GetByAccountType(string accountCatType, string costCenterId, string recPeriodId);

        IList<TRecAccount> GetByAccount(IList<string> listAccount, string costCenterId, string recPeriodId);
    }
}
