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

        public IList<TRecAccount> GetByAccountType(string accountCatType, MCostCenter costCenter, TRecPeriod recPeriod)
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
             if (costCenter != null)
             {
                 sql.AppendLine(@"   and rec.CostCenterId = :costCenter");
             }
             if (recPeriod != null)
             {
                 sql.AppendLine(@"   and rec.RecPeriodId = :recPeriod");
             }
             sql.AppendLine(@"   order by  rec.CostCenterId, cat.Id");
             IQuery q = Session.CreateQuery(sql.ToString());
             if (!string.IsNullOrEmpty(accountCatType))
             {
                 q.SetString("accountCatType", accountCatType);
             }
             if (costCenter != null)
             {
                 q.SetEntity("costCenter", costCenter);
             }
             if (recPeriod != null)
             {
                 q.SetEntity("recPeriod", recPeriod);
             }
            return q.List<TRecAccount>();

        }

        #endregion
    }
}
