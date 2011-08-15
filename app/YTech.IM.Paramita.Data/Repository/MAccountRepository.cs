using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;

namespace YTech.IM.Paramita.Data.Repository
{
    public class MAccountRepository : NHibernateRepositoryWithTypedId<MAccount, string>, IMAccountRepository
    {
        public IEnumerable<MAccount> GetPagedAccountList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, MAccountCat accountCat)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MAccount));

            ////calculate total rows
            //totalRows = Session.CreateCriteria(typeof(MAccount))
            //    .SetProjection(Projections.RowCount())
            //    .FutureValue<int>().Value;

            ////get list results
            //if (maxRows != 0)
            //{
            //    criteria.SetMaxResults(maxRows)
            //        .SetFirstResult((pageIndex - 1)*maxRows);
            //}

            //  criteria.AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
            //  ;
            criteria.Add(Expression.Eq("AccountCatId", accountCat));
            criteria.Add(Expression.IsNull("AccountParentId"));
            criteria.SetCacheable(true);
            IEnumerable<MAccount> list = criteria.List<MAccount>();
            return list;

            IQuery q = Session.CreateQuery(
                     @"
            select distinct acc
            from MAccount as acc
                left outer join fetch acc.Children
                where acc.AccountCatId = :accountCatType
               
");
            q.SetEntity("AccountCatId", accountCat);
            return q.List<MAccount>();
        }

        public IList<MAccount> GetByAccountCat(MAccountCat accountCat)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MAccount));
            criteria.Add(Expression.Eq("AccountCatId", accountCat));
            criteria.SetCacheable(true);
            IList<MAccount> list = criteria.List<MAccount>();
            return list;
        }

        public IList<string> GetLevel2Accounts(string accountCatType)
        {
            IQuery q = Session.CreateQuery(@"
                select distinct acc.Id
                from MAccount as acc
                where acc.AccountParentId.Id in (
                        select distinct acc.Id
                        from MAccount as acc
                        where acc.AccountCatId.AccountCatType = :accountCatType
                            and acc.AccountParentId is null
                        )
                    or ( acc.AccountCatId.AccountCatType = :accountCatType
                    and acc.AccountParentId is null )
            ");
            if (!string.IsNullOrEmpty(accountCatType))
                q.SetString("accountCatType", accountCatType);
            return q.List<string>();
        }
    }
}
