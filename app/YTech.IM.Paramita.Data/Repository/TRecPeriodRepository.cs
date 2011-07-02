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
    public class TRecPeriodRepository : NHibernateRepositoryWithTypedId<TRecPeriod, string>, ITRecPeriodRepository
    {
        public DateTime? GetLastDateClosing()
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TRecPeriod))
            .SetProjection(Projections.Max("PeriodTo"));
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

        public void DeleteByRecPeriodId(string recPeriodId)
        {
            StringBuilder sql = new StringBuilder();
            //delete detail period
            sql.AppendLine(@" delete from TRecAccount as det where det.RecPeriodId.Id = :recPeriodId ");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("recPeriodId", recPeriodId);
            q.ExecuteUpdate();
            //delete period
            sql = new StringBuilder();
            sql.AppendLine(@" delete from TRecPeriod as s where s.Id = :recPeriodId ");
            q = Session.CreateQuery(sql.ToString());
            q.SetString("recPeriodId", recPeriodId);
            q.ExecuteUpdate();
        }
    }
}
