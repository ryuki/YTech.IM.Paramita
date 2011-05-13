using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface ITJournalRepository : INHibernateRepositoryWithTypedId<TJournal, string>
    {
        System.DateTime? GetMinDateJournal();

        IEnumerable<TJournal> GetPagedJournalList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string searchBy, string searchText, string journalType);
    }
}
