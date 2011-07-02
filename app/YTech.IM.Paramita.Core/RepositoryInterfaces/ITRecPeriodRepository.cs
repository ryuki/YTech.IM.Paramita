using System;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface ITRecPeriodRepository : INHibernateRepositoryWithTypedId<TRecPeriod, string>
    {
        DateTime? GetLastDateClosing();

        void DeleteByRecPeriodId(string recPeriodId);
    }
}
