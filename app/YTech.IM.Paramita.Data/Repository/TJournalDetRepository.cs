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
    public class TJournalDetRepository : NHibernateRepositoryWithTypedId<TJournalDet, string>, ITJournalDetRepository
    {
        public IList<TJournalDet> GetForReport(DateTime? dateFrom, DateTime? dateTo, MCostCenter costCenter)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select det
                                from TJournalDet as det
                                    inner join det.JournalId j");
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                sql.AppendLine(@"   where j.JournalDate between :dateFrom and :dateTo");
            }
            if (costCenter != null)
            {
                sql.AppendLine(@"   and j.CostCenterId = :costCenter");
            }

            IQuery q = Session.CreateQuery(sql.ToString());
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                q.SetDateTime("dateFrom", dateFrom.Value);
                q.SetDateTime("dateTo", dateTo.Value);
            }
            if (costCenter != null)
            {
                q.SetEntity("costCenter", costCenter);
            }


            return q.List<TJournalDet>();
        }

        public IList<TJournalDet> GetDetailByJournalId(string journalId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select det
                                from TJournalDet as det
                                    inner join det.JournalId j");
            if (journalId != null)
            {
                sql.AppendLine(@"   where j.Id = :journalId");
            }

            IQuery q = Session.CreateQuery(sql.ToString());
            if (journalId != null)
            {
                q.SetString("journalId", journalId);
            }
            return q.List<TJournalDet>();
        }
    }
}
