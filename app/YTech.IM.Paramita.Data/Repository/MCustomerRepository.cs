using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;

namespace YTech.IM.Paramita.Data.Repository
{
    public class MCustomerRepository : NHibernateRepositoryWithTypedId<MCustomer, string>, IMCustomerRepository
    {
        #region Implementation of IMCustomerRepository

        public IEnumerable<MCustomer> GetPagedCustomerList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MCustomer));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MCustomer))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MCustomer> list = criteria.List<MCustomer>();
            return list;
        }

        #endregion
    }
}
