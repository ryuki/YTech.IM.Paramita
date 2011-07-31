using System;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface ITJournalDetRepository : INHibernateRepositoryWithTypedId<TJournalDet, string>
    {
        IList<TJournalDet> GetForReport(DateTime? dateFrom, DateTime? dateTo, string costCenterId);

        IList<TJournalDet> GetDetailByJournalId(string journalId);
    }
}
