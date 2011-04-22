﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Web.Controllers.Master
{
    [HandleError]
    public class ItemController : Controller
    {
        public ItemController()
            : this(new MItemRepository(), new MItemCatRepository(), new MBrandRepository(), new TTransDetRepository(), new MWarehouseRepository())
        { }

        private readonly IMItemRepository _mItemRepository;
        private readonly IMItemCatRepository _mItemCatRepository;
        private readonly IMBrandRepository _mBrandRepository;
        private readonly ITTransDetRepository _tTransDetRepository;
        private readonly IMWarehouseRepository _mWarehouseRepository;
        public ItemController(IMItemRepository mItemRepository, IMItemCatRepository mItemCatRepository, IMBrandRepository mBrandRepository, ITTransDetRepository tTransDetRepository, IMWarehouseRepository mWarehouseRepository)
        {
            Check.Require(mItemRepository != null, "mItemRepository may not be null");
            Check.Require(mItemCatRepository != null, "mItemCatRepository may not be null");
            Check.Require(mBrandRepository != null, "mBrandRepository may not be null");
            Check.Require(tTransDetRepository != null, "tTransDetRepository may not be null");
            Check.Require(mWarehouseRepository != null, "mWarehouseRepository may not be null");

            this._mItemRepository = mItemRepository;
            this._mItemCatRepository = mItemCatRepository;
            this._mBrandRepository = mBrandRepository;
            this._tTransDetRepository = tTransDetRepository;
            this._mWarehouseRepository = mWarehouseRepository;
        }


        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var itemCats = _mItemRepository.GetPagedItemList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from itemCat in itemCats
                    select new
                    {
                        i = itemCat.Id.ToString(),
                        cell = new string[] {
                            itemCat.Id, 
                            itemCat.ItemName, 
                           itemCat.ItemCatId != null ? itemCat.ItemCatId.Id : null,
                           itemCat.ItemCatId != null ? itemCat.ItemCatId.ItemCatName : null,
                           itemCat.BrandId != null ? itemCat.BrandId.Id : null,
                           itemCat.BrandId != null ? itemCat.BrandId.BrandName : null,
                         itemCat.ItemUoms.Count > 0 ?   itemCat.ItemUoms[0].Id : null,
                           itemCat.ItemUoms.Count > 0 ? itemCat.ItemUoms[0].ItemUomName : null,
                       itemCat.ItemUoms.Count > 0 ?    itemCat.ItemUoms[0].ItemUomPurchasePrice.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                            itemCat.ItemDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MItem viewModel, FormCollection formCollection)
        {

            MItem mItemToInsert = new MItem();
            TransferFormValuesTo(mItemToInsert, viewModel);
            mItemToInsert.ItemCatId = _mItemCatRepository.Get(formCollection["ItemCatId"]);
            mItemToInsert.BrandId = _mBrandRepository.Get(formCollection["BrandId"]);

            mItemToInsert.SetAssignedIdTo(viewModel.Id);
            mItemToInsert.CreatedDate = DateTime.Now;
            mItemToInsert.CreatedBy = User.Identity.Name;
            mItemToInsert.DataStatus = EnumDataStatus.New.ToString();

            //IList<MItemUom> listItemUom = new List<MItemUom>();

            mItemToInsert.ItemUoms.Clear();

            MItemUom itemUom = new MItemUom(mItemToInsert);
            itemUom.ItemUomName = formCollection["ItemUomName"];
            UpdateNumericData(itemUom, formCollection);
            //itemUom.ItemUomPurchasePrice = Convert.ToDecimal(formCollection["ItemUomPurchasePrice"]);
            itemUom.SetAssignedIdTo(Guid.NewGuid().ToString());

            mItemToInsert.ItemUoms.Add(itemUom);

            //mItemToInsert.ItemUoms = listItemUom;

            _mItemRepository.Save(mItemToInsert);

            try
            {
                _mItemRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mItemRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void UpdateNumericData(MItemUom itemUom, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["ItemUomPurchasePrice"]))
            {
                string wide = formCollection["ItemUomPurchasePrice"].Replace(",", "");
                decimal? budget = Convert.ToDecimal(wide);
                itemUom.ItemUomPurchasePrice = budget;
            }
            else
            {
                itemUom.ItemUomPurchasePrice = null;
            }
        }

        [Transaction]
        public ActionResult Delete(MItem viewModel, FormCollection formCollection)
        {
            MItem mItemToDelete = _mItemRepository.Get(viewModel.Id);

            if (mItemToDelete != null)
            {
                _mItemRepository.Delete(mItemToDelete);
            }

            try
            {
                _mItemRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mItemRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MItem viewModel, FormCollection formCollection)
        {
            MItem mItemToUpdate = _mItemRepository.Get(viewModel.Id);
            mItemToUpdate.ItemCatId = _mItemCatRepository.Get(formCollection["ItemCatId"]);
            mItemToUpdate.BrandId = _mBrandRepository.Get(formCollection["BrandId"]);
            TransferFormValuesTo(mItemToUpdate, viewModel);
            mItemToUpdate.ModifiedDate = DateTime.Now;
            mItemToUpdate.ModifiedBy = User.Identity.Name;
            mItemToUpdate.DataStatus = EnumDataStatus.Updated.ToString();

            //IList<MItemUom> listItemUom = new List<MItemUom>();

            //MItemUom itemUom = new MItemUom(mItemToUpdate);
            //itemUom.ItemUomName = formCollection["ItemUomName"];
            //itemUom.ItemUomPurchasePrice = Convert.ToDecimal(formCollection["ItemUomPurchasePrice"]);
            //itemUom.SetAssignedIdTo(Guid.NewGuid().ToString());

            //listItemUom.Add(itemUom);

            //mItemToUpdate.ItemUoms = listItemUom;


            MItemUom itemUom;
            if (mItemToUpdate.ItemUoms.Count == 0)
            {
                itemUom = new MItemUom(mItemToUpdate);
                itemUom.SetAssignedIdTo(Guid.NewGuid().ToString());
            }
            else
            {
                itemUom = mItemToUpdate.ItemUoms[0];
            }
            itemUom.ItemUomName = formCollection["ItemUomName"];
            UpdateNumericData(itemUom, formCollection);
            //itemUom.ItemUomPurchasePrice = Convert.ToDecimal(formCollection["ItemUomPurchasePrice"]);

            mItemToUpdate.ItemUoms.Clear();
            mItemToUpdate.ItemUoms.Add(itemUom);

            _mItemRepository.Update(mItemToUpdate);

            try
            {
                _mItemRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mItemRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void TransferFormValuesTo(MItem mItemToUpdate, MItem mItemFromForm)
        {
            mItemToUpdate.ItemName = mItemFromForm.ItemName;
            mItemToUpdate.ItemDesc = mItemFromForm.ItemDesc;
            mItemToUpdate.ItemStatus = mItemFromForm.ItemStatus;
            mItemToUpdate.ItemPhoto = mItemFromForm.ItemPhoto;
        }

        [Transaction]
        public virtual ActionResult GetList()
        {
            var items = _mItemRepository.GetAll();
            StringBuilder sb = new StringBuilder();
            MItem mItem = new MItem();
            sb.AppendFormat("{0}:{1};", string.Empty, "-Pilih Produk-");
            for (int i = 0; i < items.Count; i++)
            {
                mItem = items[i];
                sb.AppendFormat("{0}:{1}", mItem.Id, mItem.ItemName);
                if (i < items.Count - 1)
                    sb.Append(";");
            }
            return Content(sb.ToString());
        }

        [Transaction]
        public virtual ActionResult Get(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                MItem mItem = _mItemRepository.Get(id);
                if (mItem.ItemUoms.Count > 0)
                {
                    if (mItem.ItemUoms[0].ItemUomPurchasePrice.HasValue)
                        return Content(mItem.ItemUoms[0].ItemUomPurchasePrice.Value.ToString(Helper.CommonHelper.NumberFormat).Replace(",", ""));
                }
            }
            return Content("0");
        }

        [Transaction]
        public virtual ActionResult GetTotalBudget(string itemId, string warehouseId)
        {
            return GetTotalQty(itemId, warehouseId, EnumTransactionStatus.Budgeting);
        }

        [Transaction]
        public virtual ActionResult GetTotalUsed(string itemId, string warehouseId)
        {
            return GetTotalQty(itemId, warehouseId, EnumTransactionStatus.Using);
        }

        private ActionResult GetTotalQty(string itemId, string warehouseId, EnumTransactionStatus transactionStatus)
        {
            if (!string.IsNullOrEmpty(itemId) && !string.IsNullOrEmpty(warehouseId))
            {
                MItem mItem = _mItemRepository.Get(itemId);
                MWarehouse warehouse = _mWarehouseRepository.Get(warehouseId);
                decimal? totalUsed = _tTransDetRepository.GetTotalUsed(mItem, warehouse, DateTime.MinValue, DateTime.MaxValue, transactionStatus.ToString());
                if (totalUsed.HasValue)
                {
                    return Content(totalUsed.Value.ToString(Helper.CommonHelper.NumberFormat));
                }
            }

            return Content("0");
        }
    }
}