using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction.Accounting;


namespace YTech.IM.Paramita.Data.Repository
{
    public class TRecAccountRepository : NHibernateRepositoryWithTypedId<TRecAccount, string>, ITRecAccountRepository
    {
        #region Implementation of ITRecAccountRepository

        public void RunClosing(TRecPeriod recPeriod)
        {
            Session
                .CreateSQLQuery(@" EXECUTE [SP_CLOSING]
                      @periodId	= :periodId,
                      @periodType = :periodType,
                      @periodFrom = :periodFrom,
                      @periodTo = :periodTo")
              .SetString("periodId", recPeriod.Id)
              .SetString("periodType", recPeriod.PeriodType)
              .SetDateTime("periodFrom", recPeriod.PeriodFrom)
              .SetDateTime("periodTo", recPeriod.PeriodTo)
              .UniqueResult();
        }

        public IList<TRecAccount> GetByAccountType(string accountCatType, string costCenterId, string recPeriodId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select rec
                                from TRecAccount as rec
                                    left outer join rec.AccountId acc, MAccountCat cat
                                    where acc.AccountCatId = cat.Id");
             if (!string.IsNullOrEmpty(accountCatType))
             {
                  sql.AppendLine(@"   and cat.AccountCatType = :accountCatType");
             }
             if (!string.IsNullOrEmpty(costCenterId))
             {
                 sql.AppendLine(@"   and rec.CostCenterId.Id = :costCenterId");
             }
             if (!string.IsNullOrEmpty(recPeriodId))
             {
                 sql.AppendLine(@"   and rec.RecPeriodId.Id = :recPeriodId");
             }
             sql.AppendLine(@"   order by  rec.CostCenterId.Id, cat.Id");
             IQuery q = Session.CreateQuery(sql.ToString());
             if (!string.IsNullOrEmpty(accountCatType))
             {
                 q.SetString("accountCatType", accountCatType);
             }
             if (!string.IsNullOrEmpty(costCenterId))
             {
                 q.SetString("costCenterId", costCenterId);
             }
             if (!string.IsNullOrEmpty(recPeriodId))
             {
                 q.SetString("recPeriodId", recPeriodId);
             }
            return q.List<TRecAccount>();

        }

        public IList<TRecAccount> GetByAccount(IList<string> listAccount, string costCenterId, string recPeriodId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select rec
                                from TRecAccount as rec
                                    left outer join rec.AccountId acc
                                    where 1=1 ");
            if (listAccount.Count > 0)
            {
                sql.AppendLine(@"   and acc.Id in (:listAccount) ");
            }
            if (!string.IsNullOrEmpty(costCenterId))
            {
                sql.AppendLine(@"   and rec.CostCenterId.Id = :costCenterId");
            }
            if (!string.IsNullOrEmpty(recPeriodId))
            {
                sql.AppendLine(@"   and rec.RecPeriodId.Id = :recPeriodId");
            }
            sql.AppendLine(@"   order by  rec.CostCenterId.Id, acc.AccountCatId.Id ");
            IQuery q = Session.CreateQuery(sql.ToString());
            if (listAccount.Count > 0)
            {
                q.SetParameterList("listAccount", listAccount.ToArray());
            }
            if (!string.IsNullOrEmpty(costCenterId))
            {
                q.SetString("costCenterId", costCenterId);
            }
            if (!string.IsNullOrEmpty(recPeriodId))
            {
                q.SetString("recPeriodId", recPeriodId);
            }
            return q.List<TRecAccount>();
        }

        #endregion
    }
}
