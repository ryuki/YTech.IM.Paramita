using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.Transaction.Unit;

namespace YTech.IM.Paramita.Data.Repository
{
    public class TRealRepository : NHibernateRepositoryWithTypedId<TReal, string>, ITRealRepository
    {
        public IEnumerable<TReal> GetPagedList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, MCostCenter costCenter)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TReal));
            criteria.Add(Expression.Eq("CostCenterId", costCenter));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(TReal))
                .Add(Expression.Eq("CostCenterId", costCenter))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            if (maxRows != 0)
            {
                criteria.SetMaxResults(maxRows)
                    .SetFirstResult((pageIndex - 1) * maxRows);
            }

            criteria.AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false));
            return criteria.List<TReal>();
            ;
        }

        public TReal GetByCostCenterAndDate(MCostCenter costCenter, DateTime dateFrom, DateTime dateTo)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TReal));
            criteria.Add(Expression.Eq("CostCenterId", costCenter));
            criteria.Add(Expression.Between("RealDate", dateFrom, dateTo));
            criteria.AddOrder(Order.Desc("RealDate"));
            IList<TReal> list = criteria.List<TReal>();
            if (list.Count > 0)
            {
                return list[0] as TReal;
            }
            return null;
        }
    }
}
