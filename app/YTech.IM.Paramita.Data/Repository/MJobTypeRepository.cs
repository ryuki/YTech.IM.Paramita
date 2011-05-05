using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;

namespace YTech.IM.Paramita.Data.Repository
{
   public class MJobTypeRepository : NHibernateRepositoryWithTypedId<MJobType, string>, IMJobTypeRepository
    {
       public IEnumerable<MJobType> GetPagedJobTypeList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MJobType));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MJobType))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MJobType> list = criteria.List<MJobType>();
            return list;
        }
    }
}
