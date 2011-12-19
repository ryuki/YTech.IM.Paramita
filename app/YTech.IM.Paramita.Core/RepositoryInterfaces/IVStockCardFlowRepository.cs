using System;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.Transaction.Unit;
using YTech.IM.Paramita.Core.View;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface IVStockCardFlowRepository : INHibernateRepositoryWithTypedId<VStockCardFlow, string>
    {
        IList<VStockCardFlow> GetByDateItemWarehouse(DateTime? dateFrom, DateTime? dateTo, string itemId, string warehouseId);
    }
}
