using System;
using System.Collections.Generic;
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
    }
}
