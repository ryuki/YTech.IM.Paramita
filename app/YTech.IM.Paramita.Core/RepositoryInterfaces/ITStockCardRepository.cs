using System;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface ITStockCardRepository : INHibernateRepositoryWithTypedId<TStockCard, string>
    {
        IList<TStockCard> GetByDateItemWarehouse(DateTime? dateFrom, DateTime? dateTo, MItem item,MWarehouse warehouse);
    }
}
