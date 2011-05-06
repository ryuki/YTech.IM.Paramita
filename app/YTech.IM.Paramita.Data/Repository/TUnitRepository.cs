using System;
using System.Collections.Generic;
using System.Text;
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
        public IEnumerable<TUnit> GetPagedUnitList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string costCenterId)
        {
            //ICriteria criteria = Session.CreateCriteria(typeof(TUnit));

            ////calculate total rows
            //totalRows = Session.CreateCriteria(typeof(TUnit))
            //    .SetProjection(Projections.RowCount())
            //    .FutureValue<int>().Value;

            ////get list results
            //criteria.SetMaxResults(maxRows)
            //  .SetFirstResult((pageIndex - 1) * maxRows)
            //  .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
            //  ;

            //IEnumerable<TUnit> list = criteria.List<TUnit>();
            //return list;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   from TUnit as unit
                                    left outer join unit.CostCenterId cost
                                    where cost.Id = :costCenterId ");

            string queryCount = string.Format(" select count(unit.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);
            q.SetString("costCenterId", costCenterId);
            totalRows = Convert.ToInt32(q.UniqueResult());

            string query = string.Format(" select unit {0}", sql);
            q = Session.CreateQuery(query);
            q.SetString("costCenterId", costCenterId);
            q.SetMaxResults(maxRows);
            q.SetFirstResult((pageIndex - 1) * maxRows);
            IEnumerable<TUnit> list = q.List<TUnit>();
            return list;
        } 
    }
}
