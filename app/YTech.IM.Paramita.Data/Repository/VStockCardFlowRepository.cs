using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.View;

namespace YTech.IM.Paramita.Data.Repository
{
    public class VStockCardFlowRepository : NHibernateRepositoryWithTypedId<VStockCardFlow, string>, IVStockCardFlowRepository
    {
        public IList<VStockCardFlow> GetByDateItemWarehouse(DateTime? dateFrom, DateTime? dateTo, string itemId, string warehouseId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select det
                                from VStockCardFlow as det ");
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                sql.AppendLine(@"   where det.StockCardDate between :dateFrom and :dateTo");
            }
            if (!string.IsNullOrEmpty(itemId))
                sql.AppendLine(@"   and det.ItemId.Id = :itemId");

            if (!string.IsNullOrEmpty(warehouseId))
                sql.AppendLine(@"   and det.WarehouseId.Id = :warehouseId");
           
           // sql.AppendLine(@"  order by det.AccountId.Id, j.JournalDate, det.RowNumber, j.JournalVoucherNo ");

            IQuery q = Session.CreateQuery(sql.ToString());
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                q.SetDateTime("dateFrom", dateFrom.Value);
                q.SetDateTime("dateTo", dateTo.Value);
            }

            if (!string.IsNullOrEmpty(itemId))
                q.SetString("itemId", itemId);
            if (!string.IsNullOrEmpty(warehouseId))
                q.SetString("warehouseId", warehouseId);

            return q.List<VStockCardFlow>();
        }
    }
}
