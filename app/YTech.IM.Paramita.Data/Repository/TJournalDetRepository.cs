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
        public IList<TJournalDet> GetForReport(DateTime? dateFrom, DateTime? dateTo, string costCenterId, string accountId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select det
                                from TJournalDet as det
                                    inner join det.JournalId j
                                where 1=1 ");
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                sql.AppendLine(@"   and j.JournalDate between :dateFrom and :dateTo");
            }
            if (!string.IsNullOrEmpty(costCenterId))
                sql.AppendLine(@"   and j.CostCenterId.Id = :costCenterId");

            if (!string.IsNullOrEmpty(accountId))
                sql.AppendLine(@"   and det.AccountId.Id = :accountId");

            sql.AppendLine(@"   order by j.JournalDate, j.JournalVoucherNo, det.JournalDetStatus ");

            IQuery q = Session.CreateQuery(sql.ToString());
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                q.SetDateTime("dateFrom", dateFrom.Value);
                q.SetDateTime("dateTo", dateTo.Value);
            }
            if (!string.IsNullOrEmpty(costCenterId))
                q.SetString("costCenterId", costCenterId);

            if (!string.IsNullOrEmpty(accountId))
                q.SetString("accountId", accountId);

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
