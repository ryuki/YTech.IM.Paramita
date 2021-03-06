﻿using System;
using System.Collections;
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
    public class TStockRepository : NHibernateRepositoryWithTypedId<TStock, string>, ITStockRepository
    {
        public IList GetSisaStockList(MItem itemId, MWarehouse mWarehouse)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select s, (s.StockQty - isnull(sum(r.StockRefQty),0))
                                from TStock s
                                    left join s.StockRefs r ");
            sql.AppendLine(@"   where s.ItemId = :itemId");
            sql.AppendLine(@"       and s.WarehouseId = :mWarehouse");
            sql.AppendLine(@"   group by s, s.ItemId, s.WarehouseId, s.TransDetId , s.StockDate , s.StockQty , s.StockPrice , s.StockStatus , s.StockDesc , s.DataStatus , s.CreatedBy , s.CreatedDate , s.ModifiedBy , s.ModifiedDate , s.RowVersion   ");
            sql.AppendLine(@"   having (s.StockQty - isnull(sum(r.StockRefQty),0)) > 0 ");
            sql.AppendLine(@"   order by s.StockDate asc");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetEntity("itemId", itemId);
            q.SetEntity("mWarehouse", mWarehouse);


            return q.List();
        }

        public decimal? GetTotalStockBeforeDate(string warehouseId, DateTime dateFrom)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"  select sum(s.StockQty * s.StockPrice) ");
            sql.AppendLine(@"   from TStock s ");
            sql.AppendLine(@"   where s.WarehouseId.Id = :warehouseId");
            sql.AppendLine(@"       and s.StockDate <= :dateFrom");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("warehouseId", warehouseId);
            q.SetDateTime("dateFrom", dateFrom);
            decimal totalIn = Convert.ToDecimal(q.UniqueResult());

            sql = new StringBuilder();
            sql.AppendLine(@"  select sum(r.StockRefQty * r.StockRefPrice) ");
            sql.AppendLine(@"   from TStock s inner join s.StockRefs r  ");
            sql.AppendLine(@"   where s.WarehouseId.Id = :warehouseId");
            sql.AppendLine(@"       and r.StockRefDate <= :dateFrom");

            q = Session.CreateQuery(sql.ToString());
            q.SetString("warehouseId", warehouseId);
            q.SetDateTime("dateFrom", dateFrom);
            decimal totalOut = Convert.ToDecimal(q.UniqueResult());
            return totalIn - totalOut;
        }
    }
}
