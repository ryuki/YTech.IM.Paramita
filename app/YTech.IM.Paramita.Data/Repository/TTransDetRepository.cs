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
using YTech.IM.Paramita.Enums;

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

        public decimal? GetTotalUsed(MItem item, MWarehouse warehouse, DateTime? dateFrom, DateTime? dateTo, string transStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(
                @"   select sum(det.TransDetQty)
                                from TTransDet as det
                                    left outer join det.TransId trans
                                    where trans.TransStatus = :TransStatus ");
            if (item != null)
                sql.AppendLine(@"   and det.ItemId = :item");
            if (warehouse != null)
                sql.AppendLine(@"   and trans.WarehouseId = :warehouse");
            if (dateFrom.HasValue && dateTo.HasValue)
                sql.AppendLine(@"   and trans.TransDate between :dateFrom and :dateTo ");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("TransStatus", transStatus);
            if (item != null)
                q.SetEntity("item", item);
            if (warehouse != null)
                q.SetEntity("warehouse", warehouse);
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                q.SetDateTime("dateFrom", dateFrom.Value);
                q.SetDateTime("dateTo", dateTo.Value);
            }

            if (q.UniqueResult() != null)
                return (decimal)q.UniqueResult();
            return null;
        }

        public IList<TTransDet> GetByDateWarehouse(DateTime? dateFrom, DateTime? dateTo, string warehouseId, EnumTransactionStatus transStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(
                @"   select det
                        from TTransDet as det
                            left outer join det.TransId trans
                        where trans.TransStatus = :TransStatus ");
            if (dateFrom.HasValue && dateTo.HasValue)
                sql.AppendLine(@"   and trans.TransDate between :dateFrom and :dateTo");
            if (!string.IsNullOrEmpty(warehouseId))
                sql.AppendLine(@"   and trans.WarehouseId.Id = :warehouseId");

            sql.AppendLine(@"   order by trans.TransDate, trans.TransFactur, det.TransDetNo ");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("TransStatus", transStatus.ToString());
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                q.SetDateTime("dateFrom", dateFrom.Value);
                q.SetDateTime("dateTo", dateTo.Value);
            }

            if (!string.IsNullOrEmpty(warehouseId))
                q.SetString("warehouseId", warehouseId);
            return q.List<TTransDet>();
        }

        public IList<TTransDet> GetListById(object[] detailIdList)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select det
                                from TTransDet det
                                        where det.Id in (:detailIdList) ");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetParameterList("detailIdList", detailIdList);
            return q.List<TTransDet>();
        }

        public void DeleteById(object[] detailIdList)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   delete
                                from TTransDet det
                                        where det.Id in (:detailIdList) ");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetParameterList("detailIdList", detailIdList);
            q.ExecuteUpdate();
        }
    }
}
