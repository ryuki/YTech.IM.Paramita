using System;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.Transaction.Unit;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface ITRealRepository : INHibernateRepositoryWithTypedId<TReal, string>
    {
        IEnumerable<TReal> GetPagedList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, MCostCenter costCenter);

        TReal GetByCostCenterAndDate(MCostCenter costCenter, DateTime dateFrom, DateTime dateTo);
    }
}
