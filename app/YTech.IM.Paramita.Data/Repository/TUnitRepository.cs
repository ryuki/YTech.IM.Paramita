using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction.Unit;

namespace YTech.IM.Paramita.Data.Repository
{
    public class TUnitRepository : NHibernateRepositoryWithTypedId<TUnit, string>, ITUnitRepository
    {
        public IEnumerable<TUnit> GetPagedUnitList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TUnit));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(TUnit))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<TUnit> list = criteria.List<TUnit>();
            return list;
        }
    }
}
