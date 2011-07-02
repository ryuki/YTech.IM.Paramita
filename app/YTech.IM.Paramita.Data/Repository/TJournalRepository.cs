using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction.Accounting;

namespace YTech.IM.Paramita.Data.Repository
{
    public class TJournalRepository : NHibernateRepositoryWithTypedId<TJournal, string>, ITJournalRepository
    {
        public DateTime? GetMinDateJournal()
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TJournal))
            .SetProjection(Projections.Min("JournalDate"));
            object obj = criteria.UniqueResult();
            if (obj != null)
            {
                return Convert.ToDateTime(obj);
            }
            else
            {
                return null;
            }
            DateTime dt = criteria.FutureValue<DateTime>().Value;
            return dt;
            try
            {
               
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<TJournal> GetPagedJournalList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string searchBy, string searchText, string journalType)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TJournal as j
                                where 1=1 ");
            if (!string.IsNullOrEmpty(journalType))
            {
                sql.AppendLine(@"   and j.JournalType = :journalType ");
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                sql.AppendFormat(@"   and j.{0} like :searchText", searchBy).AppendLine();
            }

            string queryCount = string.Format(" select count(j.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);
            if (!string.IsNullOrEmpty(journalType))
            {
                q.SetString("journalType", journalType);
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", searchText);
            }
            totalRows = Convert.ToInt32(q.UniqueResult());

            sql.AppendLine(@"   order by j.JournalDate desc, j.JournalVoucherNo desc  ");
            string query = string.Format(" select j {0}", sql);
            q = Session.CreateQuery(query);
            if (!string.IsNullOrEmpty(journalType))
            {
                q.SetString("journalType", journalType);
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", searchText);
            }
            q.SetMaxResults(maxRows);
            q.SetFirstResult((pageIndex - 1) * maxRows);
            IEnumerable<TJournal> list = q.List<TJournal>();
            return list; 
        }
    }
}
