using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Data.Repository
{
    public class TTransDetRepository : NHibernateRepositoryWithTypedId<TTransDet, string>, ITTransDetRepository
    {
        public IList<TTransDet> GetByItemWarehouse(MItem item, MWarehouse warehouse)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select det
                                from TTransDet as det
                                    left outer join det.TransId trans
                                    where trans.TransStatus = :TransStatus ");
            if (item != null)
                sql.AppendLine(@"   and det.ItemId = :item");
            if (warehouse != null)
                sql.AppendLine(@"   and trans.WarehouseId = :warehouse");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("TransStatus", "Budgeting");
            if (item != null)
                q.SetEntity("item", item);
            if (warehouse != null)
                q.SetEntity("warehouse", warehouse);
            return q.List<TTransDet>();
        }

        public decimal? GetTotalUsed(MItem item, MWarehouse warehouse,string transStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select sum(det.TransDetQty)
                                from TTransDet as det
                                    left outer join det.TransId trans
                                    where trans.TransStatus = :TransStatus ");
            if (item != null)
                sql.AppendLine(@"   and det.ItemId = :item");
            if (warehouse != null)
                sql.AppendLine(@"   and trans.WarehouseId = :warehouse");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("TransStatus", transStatus);
            if (item != null)
                q.SetEntity("item", item);
            if (warehouse != null)
                q.SetEntity("warehouse", warehouse);
            if (q.UniqueResult() != null)
                return (decimal) q.UniqueResult();
            return null;
        }

        public IList<TTransDet> GetByDateWarehouse(DateTime? dateFrom, DateTime? dateTo, MWarehouse warehouse, string transStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select det
                                from TTransDet as det
                                    left outer join det.TransId trans
                                    where trans.TransStatus = :TransStatus
                                        and trans.TransDate between :dateFrom and :dateTo ");
            if (warehouse != null)
                sql.AppendLine(@"   and trans.WarehouseId = :warehouse");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("TransStatus", transStatus);
            q.SetDateTime("dateFrom", dateFrom.Value);
            q.SetDateTime("dateTo", dateTo.Value);
            if (warehouse != null)
                q.SetEntity("warehouse", warehouse);
            return q.List<TTransDet>();
        }
    }
}
