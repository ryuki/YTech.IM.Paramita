using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.View;

namespace YTech.IM.Paramita.Data.Repository
{
    public class VJournalDetFlowRepository : NHibernateRepositoryWithTypedId<VJournalDetFlow, string>, IVJournalDetFlowRepository
    {
        public IList<VJournalDetFlow> GetForReport(DateTime? dateFrom, DateTime? dateTo, string costCenterId, string accountId, string accountIdTo)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select det
                                from VJournalDetFlow as det
                                    inner join det.JournalId j");
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                sql.AppendLine(@"   where j.JournalDate between :dateFrom and :dateTo");
            }
            if (!string.IsNullOrEmpty(costCenterId))
                sql.AppendLine(@"   and j.CostCenterId.Id = :costCenterId");

            if (!string.IsNullOrEmpty(accountId))
                sql.AppendLine(@"   and det.AccountId.Id >= :accountId");
            if (!string.IsNullOrEmpty(accountIdTo))
                sql.AppendLine(@"   and det.AccountId.Id <= :accountIdTo");

            sql.AppendLine(@"  order by det.AccountId.Id, j.JournalDate, det.RowNumber, j.JournalVoucherNo ");

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
            if (!string.IsNullOrEmpty(accountIdTo))
                q.SetString("accountIdTo", accountIdTo);

            return q.List<VJournalDetFlow>();
        }
    }
}
