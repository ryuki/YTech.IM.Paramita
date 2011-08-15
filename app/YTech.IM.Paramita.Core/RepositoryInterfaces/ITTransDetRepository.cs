using System;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface ITTransDetRepository : INHibernateRepositoryWithTypedId<TTransDet, string>
    {
        IList<TTransDet> GetByItemWarehouse(MItem item, MWarehouse warehouse);

        decimal? GetTotalUsed(MItem item, MWarehouse warehouse, DateTime? dateFrom, DateTime? dateTo, string transStatus);

        IList<TTransDet> GetByDateWarehouse(System.DateTime? dateFrom, System.DateTime? dateTo, string warehouseId, EnumTransactionStatus transStatus);

        IList<TTransDet> GetListById(object[] detailIdList);

        void DeleteById(object[] detailIdList);
    }
}
