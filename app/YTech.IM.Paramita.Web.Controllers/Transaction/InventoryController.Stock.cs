using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Reporting.WebForms;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Web.Controllers.ViewModel;

namespace YTech.IM.Paramita.Web.Controllers.Transaction
{
    public partial class InventoryController
    {
        private void SaveStockItem(DateTime? transDate, string transDesc, MItem itemId, decimal? qty, bool addStock, MWarehouse mWarehouse)
        {
            TStockCard stockCard;
            TStockItem stockItem;
            //foreach (TTransDet det in ListDetTrans)
            {
                stockItem = _tStockItemRepository.GetByItemAndWarehouse(itemId, mWarehouse);
                bool isSave = false;
                if (stockItem == null)
                {
                    isSave = true;
                    stockItem = new TStockItem();
                    stockItem.SetAssignedIdTo(Guid.NewGuid().ToString());
                    stockItem.ItemId = itemId;
                    stockItem.WarehouseId = mWarehouse;
                    stockItem.CreatedBy = User.Identity.Name;
                    stockItem.CreatedDate = DateTime.Now;
                    stockItem.DataStatus = EnumDataStatus.New.ToString();
                }
                else
                {
                    stockItem.ModifiedBy = User.Identity.Name;
                    stockItem.ModifiedDate = DateTime.Now;
                    stockItem.DataStatus = EnumDataStatus.Updated.ToString();
                }
                if (addStock)
                {
                    stockItem.ItemStock = stockItem.ItemStock + qty.Value;
                }
                else
                {
                    stockItem.ItemStock = stockItem.ItemStock - qty.Value;
                }

                if (isSave)
                {
                    _tStockItemRepository.Save(stockItem);
                }
                else
                {
                    _tStockItemRepository.Update(stockItem);
                }

                //save stock card
                stockCard = new TStockCard();
                //stockCard.SetAssignedIdTo(Guid.NewGuid().ToString());
                stockCard.CreatedBy = User.Identity.Name;
                stockCard.CreatedDate = DateTime.Now;
                stockCard.DataStatus = EnumDataStatus.New.ToString();
                stockCard.ItemId = itemId;
                stockCard.StockCardDate = transDate;
                stockCard.StockCardDesc = transDesc;
                stockCard.StockCardQty = qty;
                stockCard.StockCardSaldo = stockItem.ItemStock;
                stockCard.StockCardStatus = addStock;
                //if (!isDelete)
                //    stockCard.TransDetId = det;
                stockCard.WarehouseId = mWarehouse;
                _tStockCardRepository.Save(stockCard);
            }
        }

        private decimal UpdateStock(DateTime? transDate, string transDesc, string transStatus, MItem itemId, decimal? price, decimal? qty, TTransDet det, bool addStock, MWarehouse mWarehouse)
        {
            decimal hpp = 0;
            if (addStock)
            {
                TStock stock = new TStock();
                stock.SetAssignedIdTo(Guid.NewGuid().ToString());
                stock.ItemId = det.ItemId;
                stock.TransDetId = det;
                stock.StockDate = transDate;
                stock.StockDesc = det.TransDetDesc;
                stock.StockPrice = det.TransDetPrice;
                stock.StockQty = det.TransDetQty;
                stock.StockStatus = transStatus;
                stock.WarehouseId = mWarehouse;
                stock.DataStatus = EnumDataStatus.New.ToString();
                stock.CreatedBy = User.Identity.Name;
                stock.CreatedDate = DateTime.Now;
                _tStockRepository.Save(stock);

                //set hpp equals price * quantity
                hpp = price.Value * qty.Value;
            }
            else
            {
                IList list = _tStockRepository.GetSisaStockList(det.ItemId, mWarehouse);
                TStock stock;
                object[] arr;
                decimal? sisa;
                TStockRef stockRef;
                for (int i = 0; i < list.Count; i++)
                {
                    arr = (object[])list[i];
                    stock = arr[0] as TStock;
                    sisa = (decimal)arr[1];

                    stockRef = new TStockRef(stock);
                    stockRef.SetAssignedIdTo(Guid.NewGuid().ToString());
                    stockRef.StockId = stock;
                    if (sisa >= qty)
                    {
                        stockRef.StockRefQty = qty;
                    }
                    else
                    {
                        stockRef.StockRefQty = sisa;
                    }
                    stockRef.TransDetId = det;
                    stockRef.StockRefPrice = det.TransDetPrice;
                    stockRef.StockRefDate = transDate;
                    stockRef.StockRefStatus = transStatus;
                    stockRef.StockRefDesc = det.TransDetDesc;
                    stockRef.CreatedBy = User.Identity.Name;
                    stockRef.CreatedDate = DateTime.Now;
                    stockRef.DataStatus = EnumDataStatus.New.ToString();
                    _tStockRefRepository.Save(stockRef);

                    qty = qty - sisa;
                    hpp += stockRef.StockRefQty.Value * stock.StockPrice.Value;
                    if (qty <= 0)
                    {
                        break;
                    }
                }
            }
            return hpp;
        }
    }
}
