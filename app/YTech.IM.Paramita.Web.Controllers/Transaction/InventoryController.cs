using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
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
    [HandleError]
    public partial class InventoryController : Controller
    {
        private readonly ITTransRepository _tTransRepository;
        private readonly IMWarehouseRepository _mWarehouseRepository;
        private readonly IMSupplierRepository _mSupplierRepository;
        private readonly IMItemRepository _mItemRepository;
        private readonly ITStockCardRepository _tStockCardRepository;
        private readonly ITStockItemRepository _tStockItemRepository;
        private readonly ITTransRefRepository _tTransRefRepository;
        private readonly ITStockRepository _tStockRepository;
        private readonly ITStockRefRepository _tStockRefRepository;
        private readonly IMUnitTypeRepository _mUnitTypeRepository;
        private readonly IMJobTypeRepository _mJobTypeRepository;
        private readonly IMItemUomRepository _mItemUomRepository;
        private readonly IMAccountRefRepository _mAccountRefRepository;
        private readonly ITJournalRepository _tJournalRepository;
        private readonly ITJournalDetRepository _tJournalDetRepository;
        private readonly IMAccountRepository _mAccountRepository;
        private readonly ITJournalRefRepository _tJournalRefRepository;
        private readonly ITTransDetRepository _tTransDetRepository;

        public InventoryController(ITTransRepository tTransRepository, IMWarehouseRepository mWarehouseRepository, IMSupplierRepository mSupplierRepository, IMItemRepository mItemRepository, ITStockCardRepository tStockCardRepository, ITStockItemRepository tStockItemRepository, ITTransRefRepository tTransRefRepository, ITStockRepository tStockRepository, ITStockRefRepository tStockRefRepository, IMUnitTypeRepository mUnitTypeRepository, IMJobTypeRepository mJobTypeRepository, IMItemUomRepository mItemUomRepository, IMAccountRefRepository mAccountRefRepository, ITJournalRepository tJournalRepository, ITJournalDetRepository tJournalDetRepository, IMAccountRepository mAccountRepository, ITJournalRefRepository tJournalRefRepository, ITTransDetRepository tTransDetRepository)
        {
            Check.Require(tTransRepository != null, "tTransRepository may not be null");
            Check.Require(mWarehouseRepository != null, "mWarehouseRepository may not be null");
            Check.Require(mSupplierRepository != null, "mSupplierRepository may not be null");
            Check.Require(mItemRepository != null, "mItemRepository may not be null");
            Check.Require(tStockCardRepository != null, "tStockCardRepository may not be null");
            Check.Require(tStockItemRepository != null, "tStockItemRepository may not be null");
            Check.Require(tTransRefRepository != null, "tTransRefRepository may not be null");
            Check.Require(tStockRepository != null, "tStockRepository may not be null");
            Check.Require(tStockRefRepository != null, "tStockRefRepository may not be null");
            Check.Require(mUnitTypeRepository != null, "mUnitTypeRepository may not be null");
            Check.Require(mJobTypeRepository != null, "mJobTypeRepository may not be null");
            Check.Require(mItemUomRepository != null, "mItemUomRepository may not be null");
            Check.Require(mAccountRefRepository != null, "mAccountRefRepository may not be null");
            Check.Require(tJournalRepository != null, "tJournalRepository may not be null");
            Check.Require(tJournalDetRepository != null, "tJournalDetRepository may not be null");
            Check.Require(mAccountRepository != null, "mAccountRepository may not be null");
            Check.Require(tJournalRefRepository != null, "tJournalRefRepository may not be null");
            Check.Require(tTransDetRepository != null, "tTransDetRepository may not be null");

            this._tTransRepository = tTransRepository;
            this._mWarehouseRepository = mWarehouseRepository;
            this._mSupplierRepository = mSupplierRepository;
            this._mItemRepository = mItemRepository;
            this._tStockCardRepository = tStockCardRepository;
            this._tStockItemRepository = tStockItemRepository;
            this._tTransRefRepository = tTransRefRepository;
            this._tStockRepository = tStockRepository;
            this._tStockRefRepository = tStockRefRepository;
            _mUnitTypeRepository = mUnitTypeRepository;
            _mJobTypeRepository = mJobTypeRepository;
            this._mItemUomRepository = mItemUomRepository;
            this._mAccountRefRepository = mAccountRefRepository;
            this._tJournalRepository = tJournalRepository;
            this._tJournalDetRepository = tJournalDetRepository;
            this._mAccountRepository = mAccountRepository;
            this._tJournalRefRepository = tJournalRefRepository;
            this._tTransDetRepository = tTransDetRepository;
        }

        public ActionResult Index()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.PurchaseOrder);

            return View(viewModel);
        }

        protected TransactionFormViewModel SetViewModelByStatus(EnumTransactionStatus enumTransactionStatus)
        {
            TransactionFormViewModel viewModel = TransactionFormViewModel.Create(enumTransactionStatus, _tTransRepository, _mWarehouseRepository, _mSupplierRepository, _mUnitTypeRepository, _mJobTypeRepository);

            ViewData["CurrentItem"] = viewModel.Title;
            //ViewData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.NotSaved;

            ListDetTrans = new List<TransDetViewModel>();
            ListDeleteDetailTrans = new ArrayList();

            return viewModel;
        }

        public ActionResult Purchase()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Purchase);

            ListTransRef = new List<TTransRef>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Purchase(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransactionRef(Trans, formCollection);
        }

        private ActionResult SaveTransactionRef(TTrans Trans, FormCollection formCollection)
        {
            _tTransRepository.DbContext.BeginTransaction();
            if (Trans == null)
            {
                Trans = new TTrans();
            }
            Trans.SetAssignedIdTo(formCollection["Trans.Id"]);
            Trans.CreatedDate = DateTime.Now;
            Trans.CreatedBy = User.Identity.Name;
            Trans.DataStatus = Enums.EnumDataStatus.New.ToString();
            Trans.TransSubTotal = ListTransRef.Sum(x => x.TransIdRef.TransSubTotal);
            _tTransRepository.Save(Trans);
            _tTransRepository.DbContext.CommitTransaction();

            _tTransRefRepository.DbContext.BeginTransaction();
            TTransRef detToInsert;
            foreach (TTransRef det in ListTransRef)
            {
                detToInsert = new TTransRef();
                detToInsert.SetAssignedIdTo(det.Id);
                detToInsert.TransId = Trans;
                detToInsert.TransIdRef = det.TransIdRef;
                detToInsert.TransRefDesc = det.TransRefDesc;
                detToInsert.TransRefStatus = det.TransRefStatus;

                detToInsert.CreatedBy = User.Identity.Name;
                detToInsert.CreatedDate = DateTime.Now;
                detToInsert.DataStatus = EnumDataStatus.New.ToString();
                _tTransRefRepository.Save(detToInsert);
            }
            try
            {
                _tTransRefRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
            }
            catch (Exception)
            {
                _tTransRefRepository.DbContext.RollbackTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
            }
            if (!Trans.TransStatus.Equals(EnumTransactionStatus.PurchaseOrder.ToString()))
            {
                return RedirectToAction(Trans.TransStatus);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ReturPurchase()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.ReturPurchase);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReturPurchase(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransactionInterface(Trans, formCollection);
        }

        public ActionResult Using()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Using);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Using(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransactionInterface(Trans, formCollection);
        }

        public ActionResult Received()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Received);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Received(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransactionInterface(Trans, formCollection);
        }

        public ActionResult Mutation()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Mutation);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Mutation(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransactionInterface(Trans, formCollection);
        }


        public ActionResult Adjusment()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Adjusment);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Adjusment(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransactionInterface(Trans, formCollection);
        }

        private ArrayList ListDeleteDetailTrans
        {
            get
            {
                return Session["ListDeleteDetailTrans"] as ArrayList;
            }
            set
            {
                Session["ListDeleteDetailTrans"] = value;
            }
        }

        private List<TransDetViewModel> ListDetTrans
        {
            get
            {
                //if (Session["listDetTrans"] == null)
                //{
                //    Session["listDetTrans"] = new List<TTransDet>();
                //}
                return Session["listDetTrans"] as List<TransDetViewModel>;
            }
            set
            {
                Session["listDetTrans"] = value;
            }
        }

        private List<TTransRef> ListTransRef
        {
            get
            {
                if (Session["ListTransRef"] == null)
                {
                    Session["ListTransRef"] = new List<TTransRef>();
                }
                return Session["ListTransRef"] as List<TTransRef>;
            }
            set
            {
                Session["ListTransRef"] = value;
            }
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string usePrice)
        {
            _tTransRepository.DbContext.BeginTransaction();

            int totalRecords = 0;
            var transDets = ListDetTrans;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var result = (
                           from det in transDets
                           select new
                                      {
                                          i = det.TransDet.Id.ToString(),
                                          cell = new string[]
                                                     {
                                                         det.TransDet.Id,
                                                         det.TransDet.ItemId != null ? det.TransDet.ItemId.Id : null,
                                                         det.TransDet.ItemId != null ? det.TransDet.ItemId.ItemName : null,
                                                        //  det.ItemId != null ? det.ItemId.ItemUom.ItemUomName : null,
                                                        Helper.CommonHelper.GetItemUomName(_mItemUomRepository,det.TransDet.ItemId),
                                                         det.TransDet.TransDetQty.HasValue ?  det.TransDet.TransDetQty.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                        det.TransDet.TransDetPrice.HasValue ?  det.TransDet.TransDetPrice.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                        det.TransDet.TransDetDisc.HasValue ?   det.TransDet.TransDetDisc.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                        det.TransDet.TransDetTotal.HasValue ?   det.TransDet.TransDetTotal.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                         det.TransDet.TransDetDesc
                                                     }
                                      });
            if (usePrice.Equals(false.ToString()))
            {
                result = (
                           from det in transDets
                           select new
                           {
                               i = det.TransDet.Id.ToString(),
                               cell = new string[]
                                                     {
                                                         det.TransDet.Id,
                                                         det.TransDet.ItemId != null ? det.TransDet.ItemId.Id : null,
                                                         det.TransDet.ItemId != null ? det.TransDet.ItemId.ItemName : null,
                                                        Helper.CommonHelper.GetItemUomName(_mItemUomRepository,det.TransDet.ItemId),
                                                       det.TransDet.TransDetQty.HasValue ?    det.TransDet.TransDetQty.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                         det.TransDet.TransDetDesc
                                                     }
                           });
            }

            var footer = new
                             {
                                 ItemName = "Total",
                                 TransDetQty =
                                     transDets.Sum(det => det.TransDet.TransDetQty).HasValue
                                         ? transDets.Sum(det => det.TransDet.TransDetQty).Value.ToString(
                                             Helper.CommonHelper.NumberFormat)
                                         : "0",
                                 TransDetTotal =
                                     transDets.Sum(det => det.TransDet.TransDetTotal).HasValue
                                         ? transDets.Sum(det => det.TransDet.TransDetTotal).Value.ToString(
                                             Helper.CommonHelper.NumberFormat)
                                         : "0"
                             };

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = result.ToArray(),
                userdata = footer
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public virtual ActionResult GetListTransRef(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var transRefs = ListTransRef;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from det in transRefs
                    select new
                    {
                        i = det.Id.ToString(),
                        cell = new string[] {
                             det.TransIdRef.Id,
                             det.TransIdRef.Id,
                            det.TransIdRef.TransFactur, 
                            det.TransIdRef.TransDate.HasValue ? det.TransIdRef.TransDate.Value.ToString(Helper.CommonHelper.DateFormat) : null,
                           det.TransIdRef.TransSubTotal.HasValue ?  det.TransIdRef.TransSubTotal.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                            det.TransIdRef.TransDesc
                        }
                    }).ToArray()
                //userdata: {price:1240.00} 
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(TTransDet viewModel, FormCollection formCollection)
        {
            //remove use native predicate function from list, awesome, no need foreach anymore
            ListDetTrans.Remove(ListDetTrans.Find(ByTransDetId(viewModel.Id)));
            ListDeleteDetailTrans.Add(viewModel.Id);

            UpdateNumericData(viewModel, formCollection);
            TTransDet transDetToInsert = new TTransDet();
            TransferFormValuesTo(transDetToInsert, viewModel);
            transDetToInsert.ItemId = _mItemRepository.Get(formCollection["ItemId"]);
            transDetToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
            transDetToInsert.ModifiedDate = DateTime.Now;
            transDetToInsert.ModifiedBy = User.Identity.Name;
            transDetToInsert.DataStatus = EnumDataStatus.Updated.ToString();


            TransDetViewModel detViewModel = new TransDetViewModel();
            detViewModel.TransDet = transDetToInsert;
            detViewModel.IsNew = false;
            ListDetTrans.Add(detViewModel);
            return Content("Detail transaksi berhasil disimpan");
        }

        public ActionResult Delete(TTransDet viewModel, FormCollection formCollection)
        {
            //remove use native predicate function from list, awesome, no need foreach anymore
            ListDetTrans.Remove(ListDetTrans.Find(ByTransDetId(viewModel.Id)));
            ListDeleteDetailTrans.Add(viewModel.Id);
            return Content("Detail transaksi berhasil dihapus");
        }

        static Predicate<TransDetViewModel> ByTransDetId(string detId)
        {
            return delegate(TransDetViewModel detViewModel)
            {
                return detViewModel.TransDet.Id == detId;
            };
        }

        public ActionResult DeleteTransRef(TTransRef viewModel, FormCollection formCollection)
        {
            ListTransRef.Remove(viewModel);
            return Content("Detail transaksi berhasil dihapus");
        }

        public ActionResult Insert(TTransDet viewModel, FormCollection formCollection, bool IsCalculateStock, bool IsAddStock, string warehouseId)
        {
            UpdateNumericData(viewModel, formCollection);
            //
            MItem item = _mItemRepository.Get(formCollection["ItemId"]);


            if (IsCalculateStock)
            {
                //check stock is enough or not if no add stock 
                //return Content(IsAddStock.ToString());
                if (!IsAddStock)
                {
                    MWarehouse warehouse = _mWarehouseRepository.Get(warehouseId);
                    bool isStockValid = Helper.CommonHelper.CheckStock(warehouse, item, viewModel.TransDetQty);
                    if (!isStockValid)
                    {
                        return Content("Kuantitas barang tidak cukup");
                    }
                }
            }

            TTransDet transDetToInsert = new TTransDet();
            TransferFormValuesTo(transDetToInsert, viewModel);
            transDetToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
            transDetToInsert.ItemId = _mItemRepository.Get(formCollection["ItemId"]);
            transDetToInsert.CreatedDate = DateTime.Now;
            transDetToInsert.CreatedBy = User.Identity.Name;
            transDetToInsert.DataStatus = EnumDataStatus.New.ToString();

            TransDetViewModel detViewModel = new TransDetViewModel();
            detViewModel.TransDet = transDetToInsert;
            detViewModel.IsNew = true;
            ListDetTrans.Add(detViewModel);
            return Content("Detail transaksi berhasil disimpan");
        }

        private static void UpdateNumericData(TTransDet viewModel, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["TransDetQty"]))
            {
                string wide = formCollection["TransDetQty"].Replace(",", "");
                decimal? qty = Convert.ToDecimal(wide);
                viewModel.TransDetQty = qty;
            }
            if (!string.IsNullOrEmpty(formCollection["TransDetPrice"]))
            {
                string wide = formCollection["TransDetPrice"].Replace(",", "");
                decimal? price = Convert.ToDecimal(wide);
                viewModel.TransDetPrice = price;
            }
            if (!string.IsNullOrEmpty(formCollection["TransDetDisc"]))
            {
                string wide = formCollection["TransDetDisc"].Replace(",", "");
                decimal? disc = Convert.ToDecimal(wide);
                viewModel.TransDetDisc = disc;
            }
            if (!string.IsNullOrEmpty(formCollection["TransDetTotal"]))
            {
                string wide = formCollection["TransDetTotal"].Replace(",", "");
                decimal? total = Convert.ToDecimal(wide);
                viewModel.TransDetTotal = total;
            }
        }

        public ActionResult InsertTransRef(TTransRef viewModel, FormCollection formCollection)
        {
            TTransRef transDetToInsert = new TTransRef();

            transDetToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
            transDetToInsert.TransIdRef = _tTransRepository.Get(formCollection["TransIdRef"]);
            //transDetToInsert.TransId = _tTransRepository.Get(formCollection["TransId"]);
            transDetToInsert.CreatedDate = DateTime.Now;
            transDetToInsert.CreatedBy = User.Identity.Name;
            transDetToInsert.DataStatus = EnumDataStatus.New.ToString();

            ListTransRef.Add(transDetToInsert);
            return Content("success");
        }



        private void TransferFormValuesTo(TTransDet transDet, TTransDet viewModel)
        {
            transDet.TransDetNo = ListDetTrans.Count + 1;
            transDet.TransDetQty = viewModel.TransDetQty;
            transDet.TransDetPrice = viewModel.TransDetPrice;
            transDet.TransDetDisc = viewModel.TransDetDisc;
            transDet.TransDetTotal = viewModel.TransDetTotal;
            transDet.TransDetDesc = viewModel.TransDetDesc;
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransactionInterface(Trans, formCollection);
        }

        private ActionResult SaveTransactionInterface(TTrans Trans, FormCollection formCollection)
        {
            if (formCollection["btnSave"] != null)
            {
                string msg = string.Empty;
                if (!ValidateTrans(Trans, out msg))
                {
                    var e = new
                                {
                                    Success = false,
                                    Message = msg
                                };
                    return Json(e, JsonRequestBehavior.AllowGet);
                }
                return SaveTransaction(Trans, formCollection, false);
            }
            else if (formCollection["btnDelete"] != null)
                return SaveTransaction(Trans, formCollection, true);

            return View();
        }

        private bool ValidateTrans(TTrans trans, out string msg)
        {
            msg = string.Empty;
            //check if used in trans ref
            TTransRef transRef = _tTransRefRepository.GetByRefId(trans.Id);
            if (transRef != null)
            {
                if (trans.TransStatus.Equals(EnumTransactionStatus.Received.ToString()))
                    msg = "Transaksi penerimaan barang telah diinput ke pembelian";
                return false;
            }
            return true;
        }

        private ActionResult SaveTransaction(TTrans Trans, FormCollection formCollection, bool isDelete)
        {
            string Message = string.Empty;
            bool Success = true;
            string FacturNo = string.Empty;
            try
            {
                _tTransRepository.DbContext.BeginTransaction();

                //get stock add or calculated
                bool addStock = true;
                bool calculateStock = false;
                EnumTransactionStatus status = (EnumTransactionStatus)Enum.Parse(typeof(EnumTransactionStatus), Trans.TransStatus);
                switch (status)
                {
                    case EnumTransactionStatus.Received:
                        addStock = true;
                        calculateStock = true;
                        break;
                    case EnumTransactionStatus.Adjusment:
                        addStock = true;
                        calculateStock = true;
                        break;
                    case EnumTransactionStatus.ReturPurchase:
                        addStock = false;
                        calculateStock = true;
                        break;
                    case EnumTransactionStatus.ReturSales:
                        addStock = true;
                        calculateStock = true;
                        break;
                    case EnumTransactionStatus.Sales:
                        addStock = false;
                        calculateStock = true;
                        break;
                    case EnumTransactionStatus.Using:
                        addStock = false;
                        calculateStock = true;
                        break;
                    case EnumTransactionStatus.Mutation:
                        addStock = false;
                        calculateStock = true;
                        break;
                }

                //check first
                TTrans tr = _tTransRepository.Get(formCollection["Trans.Id"]);
                //if (tr != null)
                //{
                //    //call back stock for deleted
                //    DeleteTransaction(tr, !addStock, calculateStock);
                //    if (bool.Parse(formCollection["IsGenerateFactur"]))
                //        FacturNo = tr.TransFactur;
                //}
                //else
                //{
                //    //set factur no after saved
                //    if (bool.Parse(formCollection["IsGenerateFactur"]))
                //    {
                //        //if (string.IsNullOrEmpty(Trans.TransFactur))
                //        {
                //            FacturNo = Helper.CommonHelper.GetFacturNo(status, false);
                //            Trans.TransFactur = FacturNo;
                //        }
                //    }
                //}
                if (!isDelete)
                {
                    bool isEdit = false;
                    if (tr == null)
                    {
                        isEdit = false;
                        //if 
                        tr = new TTrans();
                        tr.SetAssignedIdTo(Guid.NewGuid().ToString());
                        tr.CreatedDate = DateTime.Now;
                        tr.CreatedBy = User.Identity.Name;
                        tr.DataStatus = Enums.EnumDataStatus.New.ToString();
                        //set factur no after saved
                        if (bool.Parse(formCollection["IsGenerateFactur"]))
                        {
                            //if (string.IsNullOrEmpty(Trans.TransFactur))
                            {
                                FacturNo = Helper.CommonHelper.GetFacturNo(status, false);
                                Trans.TransFactur = FacturNo;
                            }
                        }
                    }
                    else
                    {
                        isEdit = true;
                        tr.ModifiedDate = DateTime.Now;
                        tr.ModifiedBy = User.Identity.Name;
                        tr.DataStatus = Enums.EnumDataStatus.Updated.ToString();
                        if (bool.Parse(formCollection["IsGenerateFactur"]))
                            FacturNo = tr.TransFactur;
                    }
                    //if (!string.IsNullOrEmpty(formCollection["Trans.WarehouseId"]))
                    //    tr.WarehouseId = _mWarehouseRepository.Get(formCollection["Trans.WarehouseId"]);
                    //else
                    //    tr.WarehouseId = null;

                    //if (!string.IsNullOrEmpty(formCollection["Trans.WarehouseIdTo"]))
                    //    tr.WarehouseIdTo = _mWarehouseRepository.Get(formCollection["Trans.WarehouseIdTo"]);
                    //else
                    //    tr.WarehouseIdTo = null;

                    //if (!string.IsNullOrEmpty(formCollection["Trans.UnitTypeId"]))
                    //    tr.UnitTypeId = _mUnitTypeRepository.Get(formCollection["Trans.UnitTypeId"]);
                    //else
                    //    tr.UnitTypeId = null;

                    //if (!string.IsNullOrEmpty(formCollection["Trans.JobTypeId"]))
                    //    tr.JobTypeId = _mJobTypeRepository.Get(formCollection["Trans.JobTypeId"]);
                    //else
                    //    tr.JobTypeId = null;

                    tr.WarehouseId = Trans.WarehouseId;
                    tr.WarehouseIdTo = Trans.WarehouseIdTo;
                    tr.UnitTypeId = Trans.UnitTypeId;
                    tr.JobTypeId = Trans.JobTypeId;

                    tr.TransStatus = Trans.TransStatus;
                    tr.TransFactur = Trans.TransFactur;
                    tr.TransDate = Trans.TransDate;
                    tr.TransDueDate = Trans.TransDueDate;
                    tr.TransBy = Trans.TransBy;
                    tr.TransPaymentMethod = Trans.TransPaymentMethod;
                    SaveTransaction(tr, formCollection, addStock, calculateStock, isEdit);
                }
                else
                {
                    if (tr != null)
                    {
                        //call back stock for deleted
                        DeleteTransaction(tr, !addStock, calculateStock);
                    }
                }

                _tTransRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
                if (!isDelete)
                    Message = "Data berhasil disimpan.";
                else
                    Message = "Data berhasil dihapus.";
            }
            catch (Exception ex)
            {
                Success = false;
                if (!isDelete)
                    Message = "Data gagal disimpan.";
                else
                    Message = "Data gagal dihapus.";
                Message += "Error : " + ex.GetBaseException().Message;
                _tTransRepository.DbContext.RollbackTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
            }
            var e = new
            {
                Success,
                Message,
                FacturNo
            };
            return Json(e, JsonRequestBehavior.AllowGet);
            //if (!Trans.TransStatus.Equals(EnumTransactionStatus.PurchaseOrder.ToString()))
            //{
            //    return RedirectToAction(Trans.TransStatus.ToString());
            //}
            //return View("Status");
        }

        private void DeleteTransaction(TTrans tr, bool addStock, bool calculateStock)
        {
            IList<TTransDet> listDet = tr.TransDets;

            IEnumerable<string> listDetailId = from det in listDet
                                               select det.Id;

            DeleteTransactionDetail(tr, addStock, calculateStock, listDetailId.ToArray());

            _tTransRepository.Delete(tr);
        }

        private void SaveTransaction(TTrans Trans, FormCollection formCollection, bool addStock, bool calculateStock, bool isEdit)
        {
            if (isEdit)
            {
                if (ListDeleteDetailTrans != null)
                    if (ListDeleteDetailTrans.Count > 0)
                        DeleteTransactionDetail(Trans, addStock, calculateStock, ListDeleteDetailTrans.ToArray());
            }
            //Trans.SetAssignedIdTo(formCollection["Trans.Id"]);
            ////Trans.WarehouseId = _mWarehouseRepository.Get(formCollection["Trans.WarehouseId"]);
            //if (!string.IsNullOrEmpty(formCollection["Trans.WarehouseIdTo"]))
            //{
            //    Trans.WarehouseIdTo = _mWarehouseRepository.Get(formCollection["Trans.WarehouseIdTo"]);
            //}
            //if (!string.IsNullOrEmpty(formCollection["Trans.UnitTypeId"]))
            //    Trans.UnitTypeId = _mUnitTypeRepository.Get(formCollection["Trans.UnitTypeId"]);
            //if (!string.IsNullOrEmpty(formCollection["Trans.JobTypeId"]))
            //    Trans.JobTypeId = _mJobTypeRepository.Get(formCollection["Trans.JobTypeId"]);
            //Trans.CreatedDate = DateTime.Now;
            //Trans.CreatedBy = User.Identity.Name;
            //Trans.DataStatus = Enums.EnumDataStatus.New.ToString();

            Trans.TransDets.Clear();

            //save stock card

            TTransDet detToInsert;
            IList<TTransDet> listDet = new List<TTransDet>();
            decimal total = 0;
            IList<TransDetViewModel> listDetToSave = new List<TransDetViewModel>();
            TransDetViewModel detToSave = new TransDetViewModel();

            //MJobType jobType = null;
            //if (!string.IsNullOrEmpty(formCollection["Trans.JobTypeId"]))
            //    jobType = _mJobTypeRepository.Get(formCollection["Trans.JobTypeId"]);

            foreach (TransDetViewModel det in ListDetTrans)
            {
                detToSave = new TransDetViewModel();
                detToSave.IsNew = det.IsNew;

                if (det.IsNew)
                {

                    detToInsert = new TTransDet(Trans);
                    detToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
                    detToInsert.ItemId = det.TransDet.ItemId;
                    detToInsert.ItemUomId = det.TransDet.ItemUomId;
                    detToInsert.TransDetQty = det.TransDet.TransDetQty;
                    detToInsert.TransDetPrice = det.TransDet.TransDetPrice;
                    detToInsert.TransDetDisc = det.TransDet.TransDetDisc;
                    detToInsert.TransDetTotal = det.TransDet.TransDetTotal;
                    //if (jobType != null)
                    //    detToInsert.JobTypeId = jobType;
                    //else
                    //    detToInsert.JobTypeId = det.JobTypeId;
                    detToInsert.CreatedBy = User.Identity.Name;
                    detToInsert.CreatedDate = DateTime.Now;
                    detToInsert.DataStatus = Enums.EnumDataStatus.New.ToString();
                    Trans.TransDets.Add(detToInsert);
                    detToSave.TransDet = detToInsert;
                }
                else
                {
                    detToSave.TransDet = det.TransDet;
                }
                listDetToSave.Add(detToSave);
                total += det.TransDet.TransDetTotal.HasValue ? det.TransDet.TransDetTotal.Value : 0;
            }
            Trans.TransSubTotal = total;
            _tTransRepository.Save(Trans);
            //_tTransRepository.DbContext.CommitTransaction();

            //_tStockCardRepository.DbContext.BeginTransaction();
            if (calculateStock)
            {
                decimal totalHpp = 0;
                foreach (TransDetViewModel det in listDetToSave)
                {
                    if (det.IsNew)
                    {
                        //save stock
                        if (Trans.TransStatus.Equals(EnumTransactionStatus.Mutation.ToString()))
                        {
                            SaveStockItem(Trans.TransDate, Trans.TransDesc, det.TransDet.ItemId, det.TransDet.TransDetQty, false, Trans.WarehouseId);
                            SaveStockItem(Trans.TransDate, Trans.TransDesc, det.TransDet.ItemId, det.TransDet.TransDetQty, true, Trans.WarehouseIdTo);
                            //still to do, for mutation, price of stock must recalculate per stock, 
                            //sum hpp for each stock for stock out
                            totalHpp += UpdateStock(Trans.TransDate, Trans.TransDesc, Trans.TransStatus, det.TransDet.ItemId, det.TransDet.TransDetPrice, det.TransDet.TransDetQty, det.TransDet, false, Trans.WarehouseId);
                            UpdateStock(Trans.TransDate, Trans.TransDesc, Trans.TransStatus, det.TransDet.ItemId, det.TransDet.TransDetPrice, det.TransDet.TransDetQty, det.TransDet, true, Trans.WarehouseIdTo);
                        }
                        else
                        {
                            SaveStockItem(Trans.TransDate, Trans.TransDesc, det.TransDet.ItemId, det.TransDet.TransDetQty, addStock, Trans.WarehouseId);
                            //sum hpp for each stock
                            totalHpp += UpdateStock(Trans.TransDate, Trans.TransDesc, Trans.TransStatus, det.TransDet.ItemId, det.TransDet.TransDetPrice, det.TransDet.TransDetQty, det.TransDet, addStock, Trans.WarehouseId);
                        }
                    }
                    else
                    {
                        //get HPP from existing detail
                        if (addStock)
                        {
                            decimal qty = det.TransDet.TransDetQty.HasValue ? det.TransDet.TransDetQty.Value : 0;
                            decimal price = det.TransDet.TransDetPrice.HasValue ? det.TransDet.TransDetPrice.Value : 0;
                            totalHpp += qty * price;
                        }

                    }
                }
                //save journal
                SaveJournal(Trans, totalHpp);
            }
        }

        private void DeleteTransactionDetail(TTrans Trans, bool addStock, bool calculateStock, object[] detailIdList)
        {
            if (calculateStock)
            {
                IList<TTransDet> listDet = _tTransDetRepository.GetListById(detailIdList);
                foreach (TTransDet det in listDet)
                {
                    //undo stock
                    if (Trans.TransStatus.Equals(EnumTransactionStatus.Mutation.ToString()))
                    {
                        SaveStockItem(Trans.TransDate, "Hapus Detail" + Trans.TransFactur, det.ItemId, det.TransDetQty, !false, Trans.WarehouseId);
                        SaveStockItem(Trans.TransDate, "Hapus Detail" + Trans.TransFactur, det.ItemId, det.TransDetQty, !true, Trans.WarehouseIdTo);

                        ////still to do, for mutation, price of stock must recalculate per stock, 
                        ////sum hpp for each stock for stock out
                        //UpdateStock(Trans.TransDate, Trans.TransDesc, Trans.TransStatus, det.ItemId, det.TransDetPrice, det.TransDetQty, det, false, Trans.WarehouseId);
                        //UpdateStock(Trans.TransDate, Trans.TransDesc, Trans.TransStatus, det.ItemId, det.TransDetPrice, det.TransDetQty, det, true, Trans.WarehouseIdTo);
                    }
                    else
                    {
                        SaveStockItem(Trans.TransDate, "Hapus Detail" + Trans.TransFactur, det.ItemId, det.TransDetQty, !addStock, Trans.WarehouseId);

                        if (addStock)
                        {
                            //set stock as out stock
                            UpdateStock(Trans.TransDate, "Hapus Detail" + Trans.TransFactur, Trans.TransStatus, det.ItemId, det.TransDetPrice, det.TransDetQty, det, false, Trans.WarehouseId);
                        }
                    }
                }

                //delete stock ref for not add stock
                if (!addStock)
                {
                    _tStockRefRepository.DeleteByTransDetId(detailIdList);
                }
            }

            //delete detail
            _tTransDetRepository.DeleteById(detailIdList);
        }

        public ActionResult Budgeting()
        {
            TransactionFormViewModel viewModel = SetViewModelByStatus(EnumTransactionStatus.Budgeting);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Budgeting(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransactionInterface(Trans, formCollection);
        }

        [Transaction]
        public virtual ActionResult GetListTrans(string transStatus, string warehouseId, string transBy)
        {
            IList<TTrans> transes;
            //if (!string.IsNullOrEmpty(transStatus))
            MWarehouse warehouse = _mWarehouseRepository.Get(warehouseId);
            {
                transes = _tTransRepository.GetByWarehouseStatusTransBy(warehouse, transStatus, transBy);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:{1}", string.Empty, "-Pilih Faktur-");
            foreach (TTrans trans in transes)
            {
                sb.AppendFormat(";{0}:{1}", trans.Id, trans.TransFactur);
            }
            return Content(sb.ToString());
        }

        [Transaction]
        public ActionResult ListTransaction()
        {
            return View();
        }

        [Transaction]
        public ActionResult ListSearchTrans(string sidx, string sord, int page, int rows, string searchBy, string searchText, string transStatus)
        {
            int totalRecords = 0;
            var transList = _tTransRepository.GetPagedTransList(sidx, sord, page, rows, ref totalRecords, searchBy, searchText, transStatus);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from trans in transList
                    select new
                    {
                        i = trans.Id.ToString(),
                        cell = new string[] {
                            trans.Id, 
                            trans.TransFactur, 
                            trans.TransDate.HasValue ? trans.TransDate.Value.ToString(Helper.CommonHelper.DateFormat): null,
                            trans.TransDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult GetJsonTrans(string transId)
        {
            TTrans trans = _tTransRepository.Get(transId);
            ConvertListDet(trans.TransDets.ToList());

            //set new array list for delete id 
            //to prevent deleted id before
            ListDeleteDetailTrans = new ArrayList();

            var t = new
            {
                TransId = trans.Id,
                trans.TransDate,
                trans.TransFactur,
                WarehouseId = trans.WarehouseId != null ? trans.WarehouseId.Id : null,
                trans.TransPaymentMethod,
                trans.TransBy,
                WarehouseIdTo = trans.WarehouseIdTo != null ? trans.WarehouseIdTo.Id : null,
                UnitTypeId = trans.UnitTypeId != null ? trans.UnitTypeId.Id : null,
                JobTypeId = trans.JobTypeId != null ? trans.JobTypeId.Id : null

            };
            return Json(t, JsonRequestBehavior.AllowGet);
        }

        private void ConvertListDet(List<TTransDet> listDet)
        {
            TTransDet det;
            TransDetViewModel detViewModel;
            ListDetTrans = new List<TransDetViewModel>();
            for (int i = 0; i < listDet.Count; i++)
            {
                det = listDet[i];
                detViewModel = new TransDetViewModel();
                detViewModel.TransDet = det;
                detViewModel.IsNew = false;
                ListDetTrans.Add(detViewModel);
            }
        }

    }
}
