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
    public class InventoryController : Controller
    {
        public InventoryController()
            : this(new TTransRepository(), new MWarehouseRepository(), new MSupplierRepository(), new MItemRepository(), new TStockCardRepository(), new TStockItemRepository(), new TTransRefRepository(), new TStockRepository(), new TStockRefRepository(), new MUnitTypeRepository(), new MJobTypeRepository())
        {
        }

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

        public InventoryController(ITTransRepository tTransRepository, IMWarehouseRepository mWarehouseRepository, IMSupplierRepository mSupplierRepository, IMItemRepository mItemRepository, ITStockCardRepository tStockCardRepository, ITStockItemRepository tStockItemRepository, ITTransRefRepository tTransRefRepository, ITStockRepository tStockRepository, ITStockRefRepository tStockRefRepository, IMUnitTypeRepository mUnitTypeRepository, IMJobTypeRepository mJobTypeRepository)
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
        }

        public ActionResult Index()
        {
            TransactionFormViewModel viewModel = TransactionFormViewModel.CreateTransactionFormViewModel(_tTransRepository, _mWarehouseRepository, _mSupplierRepository, _mUnitTypeRepository, _mJobTypeRepository);
            viewModel.Trans = SetNewTrans(EnumTransactionStatus.PurchaseOrder);
            SetViewModelByStatus(viewModel, EnumTransactionStatus.PurchaseOrder);

            ListDetTrans = new List<TTransDet>();

            return View(viewModel);
        }

        private void SetViewModelByStatus(TransactionFormViewModel viewModel, EnumTransactionStatus enumTransactionStatus)
        {
            switch (enumTransactionStatus)
            {
                case EnumTransactionStatus.PurchaseOrder:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = false;
                    viewModel.ViewSupplier = true;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = true;
                    viewModel.ViewPaymentMethod = false;
                    break;
                case EnumTransactionStatus.Purchase:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = false;
                    viewModel.ViewSupplier = true;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = true;
                    viewModel.ViewPaymentMethod = true;
                    break;
                case EnumTransactionStatus.ReturPurchase:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = false;
                    viewModel.ViewSupplier = true;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = true;
                    viewModel.ViewPaymentMethod = true;
                    break;
                case EnumTransactionStatus.Sales:
                    break;
                case EnumTransactionStatus.ReturSales:
                    break;
                case EnumTransactionStatus.Using:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = false;
                    viewModel.ViewSupplier = false;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = false;
                    viewModel.ViewPaymentMethod = false;
                    break;
                case EnumTransactionStatus.Mutation:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = true;
                    viewModel.ViewSupplier = false;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = false;
                    viewModel.ViewPaymentMethod = false;
                    break;
                case EnumTransactionStatus.Adjusment:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = false;
                    viewModel.ViewSupplier = false;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = false;
                    viewModel.ViewPaymentMethod = false;
                    break;
                case EnumTransactionStatus.Received:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = false;
                    viewModel.ViewSupplier = true;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPaymentMethod = false;
                    break;
                case EnumTransactionStatus.Budgeting:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = false;
                    viewModel.ViewSupplier = false;
                    viewModel.ViewDate = false;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = true;
                    viewModel.ViewPaymentMethod = false;
                    viewModel.ViewUnitType = true;
                    viewModel.ViewJobType = true;
                    break;
            }
            viewModel.Title = Helper.CommonHelper.GetStringValue(enumTransactionStatus);

            ViewData["CurrentItem"] = viewModel.Title;
            //ViewData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.NotSaved;
        }

        private TTrans SetNewTrans(EnumTransactionStatus enumTransactionStatus)
        {
            TTrans trans = new TTrans();
            trans.TransDate = DateTime.Today;
            trans.TransFactur = Helper.CommonHelper.GetFacturNo(enumTransactionStatus);
            trans.SetAssignedIdTo(Guid.NewGuid().ToString());
            trans.TransStatus = enumTransactionStatus.ToString();
            return trans;
        }

        public ActionResult Purchase()
        {
            TransactionFormViewModel viewModel = TransactionFormViewModel.CreateTransactionFormViewModel(_tTransRepository, _mWarehouseRepository, _mSupplierRepository, _mUnitTypeRepository, _mJobTypeRepository);
            viewModel.Trans = SetNewTrans(EnumTransactionStatus.Purchase);
            SetViewModelByStatus(viewModel, EnumTransactionStatus.Purchase);

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
            TransactionFormViewModel viewModel = TransactionFormViewModel.CreateTransactionFormViewModel(_tTransRepository, _mWarehouseRepository, _mSupplierRepository, _mUnitTypeRepository, _mJobTypeRepository);
            viewModel.Trans = SetNewTrans(EnumTransactionStatus.ReturPurchase);
            SetViewModelByStatus(viewModel, EnumTransactionStatus.ReturPurchase);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReturPurchase(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        public ActionResult Using()
        {
            TransactionFormViewModel viewModel = TransactionFormViewModel.CreateTransactionFormViewModel(_tTransRepository, _mWarehouseRepository, _mSupplierRepository, _mUnitTypeRepository, _mJobTypeRepository);
            viewModel.Trans = SetNewTrans(EnumTransactionStatus.Using);
            SetViewModelByStatus(viewModel, EnumTransactionStatus.Using);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Using(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        public ActionResult Received()
        {
            TransactionFormViewModel viewModel = TransactionFormViewModel.CreateTransactionFormViewModel(_tTransRepository, _mWarehouseRepository, _mSupplierRepository, _mUnitTypeRepository, _mJobTypeRepository);
            viewModel.Trans = SetNewTrans(EnumTransactionStatus.Received);
            SetViewModelByStatus(viewModel, EnumTransactionStatus.Received);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Received(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        public ActionResult Mutation()
        {
            TransactionFormViewModel viewModel = TransactionFormViewModel.CreateTransactionFormViewModel(_tTransRepository, _mWarehouseRepository, _mSupplierRepository, _mUnitTypeRepository, _mJobTypeRepository);
            viewModel.Trans = SetNewTrans(EnumTransactionStatus.Mutation);
            SetViewModelByStatus(viewModel, EnumTransactionStatus.Mutation);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Mutation(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        [Transaction]
        public ActionResult Adjusment()
        {
            TransactionFormViewModel viewModel = TransactionFormViewModel.CreateTransactionFormViewModel(_tTransRepository, _mWarehouseRepository, _mSupplierRepository, _mUnitTypeRepository, _mJobTypeRepository);
            viewModel.Trans = SetNewTrans(EnumTransactionStatus.Adjusment);
            SetViewModelByStatus(viewModel, EnumTransactionStatus.Adjusment);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Adjusment(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
        }

        private List<TTransDet> ListDetTrans
        {
            get
            {
                if (Session["listDetTrans"] == null)
                {
                    Session["listDetTrans"] = new List<TTransDet>();
                }
                return Session["listDetTrans"] as List<TTransDet>;
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
            int totalRecords = 0;
            var transDets = ListDetTrans;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var result = (
                           from det in transDets
                           select new
                                      {
                                          i = det.Id.ToString(),
                                          cell = new string[]
                                                     {
                                                         det.Id,
                                                         det.ItemId != null ? det.ItemId.Id : null,
                                                         det.ItemId != null ? det.ItemId.ItemName : null,
                                                        det.TransDetPrice.HasValue ?  det.TransDetPrice.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                         det.TransDetQty.HasValue ?  det.TransDetQty.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                        det.TransDetDisc.HasValue ?   det.TransDetDisc.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                        det.TransDetTotal.HasValue ?   det.TransDetTotal.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                         det.TransDetDesc
                                                     }
                                      });
            if (usePrice.Equals(false.ToString()))
            {
                result = (
                           from det in transDets
                           select new
                           {
                               i = det.Id.ToString(),
                               cell = new string[]
                                                     {
                                                         det.Id,
                                                         det.ItemId != null ? det.ItemId.Id : null,
                                                         det.ItemId != null ? det.ItemId.ItemName : null,
                                                       det.TransDetQty.HasValue ?    det.TransDetQty.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                                                         det.TransDetDesc
                                                     }
                           });
            }

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = result.ToArray()
                //userdata: {price:1240.00} 
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
            TTransDet transDetToInsert = new TTransDet();
            TransferFormValuesTo(transDetToInsert, viewModel);
            transDetToInsert.SetAssignedIdTo(viewModel.Id);
            transDetToInsert.CreatedDate = DateTime.Now;
            transDetToInsert.CreatedBy = User.Identity.Name;
            transDetToInsert.DataStatus = EnumDataStatus.New.ToString();

            ListDetTrans.Add(transDetToInsert);
            return Content("success");
        }

        public ActionResult Delete(TTransDet viewModel, FormCollection formCollection)
        {
            ListDetTrans.Remove(viewModel);
            return Content("success");
        }

        public ActionResult DeleteTransRef(TTransRef viewModel, FormCollection formCollection)
        {
            ListTransRef.Remove(viewModel);
            return Content("success");
        }

        public ActionResult Insert(TTransDet viewModel, FormCollection formCollection)
        {
            UpdateNumericData(viewModel, formCollection);

            TTransDet transDetToInsert = new TTransDet();
            TransferFormValuesTo(transDetToInsert, viewModel);
            transDetToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
            transDetToInsert.ItemId = _mItemRepository.Get(formCollection["ItemId"]);
            transDetToInsert.SetAssignedIdTo(viewModel.Id);
            transDetToInsert.CreatedDate = DateTime.Now;
            transDetToInsert.CreatedBy = User.Identity.Name;
            transDetToInsert.DataStatus = EnumDataStatus.New.ToString();

            ListDetTrans.Add(transDetToInsert);
            return Content("success");
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
                decimal? disc= Convert.ToDecimal(wide);
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
            return SaveTransaction(Trans, formCollection);
        }

        private ActionResult SaveTransaction(TTrans Trans, FormCollection formCollection)
        {
            _tTransRepository.DbContext.BeginTransaction();
            if (Trans == null)
            {
                Trans = new TTrans();
            }
            Trans.SetAssignedIdTo(formCollection["Trans.Id"]);
            Trans.WarehouseId = _mWarehouseRepository.Get(formCollection["Trans.WarehouseId"]);
            if (!string.IsNullOrEmpty(formCollection["Trans.WarehouseIdTo"]))
            {
                Trans.WarehouseIdTo = _mWarehouseRepository.Get(formCollection["Trans.WarehouseIdTo"]);
            }
            Trans.UnitTypeId = _mUnitTypeRepository.Get(formCollection["Trans.UnitTypeId"]);
            Trans.CreatedDate = DateTime.Now;
            Trans.CreatedBy = User.Identity.Name;
            Trans.DataStatus = Enums.EnumDataStatus.New.ToString();

            Trans.TransDets.Clear();

            //save stock card
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

            TTransDet detToInsert;
            IList<TTransDet> listDet = new List<TTransDet>();
            decimal total = 0;
            foreach (TTransDet det in ListDetTrans)
            {
                detToInsert = new TTransDet(Trans);
                detToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
                detToInsert.ItemId = det.ItemId;
                detToInsert.ItemUomId = det.ItemUomId;
                detToInsert.JobTypeId = _mJobTypeRepository.Get(formCollection["Trans.JobTypeId"]);
                detToInsert.TransDetQty = det.TransDetQty;
                detToInsert.TransDetPrice = det.TransDetPrice;
                detToInsert.TransDetDisc = det.TransDetDisc;
                detToInsert.TransDetTotal = det.TransDetTotal;
                detToInsert.CreatedBy = User.Identity.Name;
                detToInsert.CreatedDate = DateTime.Now;
                detToInsert.DataStatus = Enums.EnumDataStatus.New.ToString();
                Trans.TransDets.Add(detToInsert);
                total += det.TransDetTotal.HasValue ? det.TransDetTotal.Value : 0;
                listDet.Add(detToInsert);
            }
            Trans.TransSubTotal = total;
            _tTransRepository.Save(Trans);
            //_tTransRepository.DbContext.CommitTransaction();

            //_tStockCardRepository.DbContext.BeginTransaction();
            if (calculateStock)
            {
                foreach (TTransDet det in listDet)
                {
                    //save stock
                    if (Trans.TransStatus.Equals(EnumTransactionStatus.Mutation.ToString()))
                    {
                        SaveStock(Trans, det, false, Trans.WarehouseId);
                        SaveStock(Trans, det, true, Trans.WarehouseIdTo);
                        UpdateStockDetail(Trans, det, false, Trans.WarehouseId);
                        UpdateStockDetail(Trans, det, true, Trans.WarehouseIdTo);
                    }
                    else
                    {
                        SaveStock(Trans, det, addStock, Trans.WarehouseId);
                        UpdateStockDetail(Trans, det, addStock, Trans.WarehouseId);
                    }
                }
            }

            try
            {
                _tTransRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
            }
            catch (Exception)
            {
                _tTransRepository.DbContext.RollbackTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
            }
            //if (!Trans.TransStatus.Equals(EnumTransactionStatus.PurchaseOrder.ToString()))
            //{
            //    return RedirectToAction(Trans.TransStatus.ToString());
            //}
            return View("Status");
        }

        private void SaveStock(TTrans Trans, TTransDet det, bool addStock, MWarehouse mWarehouse)
        {
            TStockCard stockCard;
            TStockItem stockItem;
            //foreach (TTransDet det in ListDetTrans)
            {
                stockItem = _tStockItemRepository.GetByItemAndWarehouse(det.ItemId, mWarehouse);
                bool isSave = false;
                if (stockItem == null)
                {
                    isSave = true;
                    stockItem = new TStockItem();
                    stockItem.SetAssignedIdTo(Guid.NewGuid().ToString());
                    stockItem.ItemId = det.ItemId;
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
                    stockItem.ItemStock = stockItem.ItemStock + det.TransDetQty.Value;
                }
                else
                {
                    stockItem.ItemStock = stockItem.ItemStock - det.TransDetQty.Value;
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
                stockCard.ItemId = det.ItemId;
                stockCard.StockCardDate = Trans.TransDate;
                stockCard.StockCardDesc = Trans.TransDesc;
                stockCard.StockCardQty = det.TransDetQty;
                stockCard.StockCardSaldo = stockItem.ItemStock;
                stockCard.StockCardStatus = addStock;
                stockCard.TransDetId = det;
                stockCard.WarehouseId = mWarehouse;
                _tStockCardRepository.Save(stockCard);
            }
        }

        private void UpdateStockDetail(TTrans Trans, TTransDet det, bool addStock, MWarehouse mWarehouse)
        {
            if (addStock)
            {
                TStock stock = new TStock();
                stock.SetAssignedIdTo(Guid.NewGuid().ToString());
                stock.ItemId = det.ItemId;
                stock.TransDetId = det;
                stock.StockDate = Trans.TransDate;
                stock.StockDesc = det.TransDetDesc;
                stock.StockPrice = det.TransDetPrice;
                stock.StockQty = det.TransDetQty;
                stock.StockStatus = Trans.TransStatus;
                stock.WarehouseId = mWarehouse;
                stock.DataStatus = EnumDataStatus.New.ToString();
                stock.CreatedBy = User.Identity.Name;
                stock.CreatedDate = DateTime.Now;
                _tStockRepository.Save(stock);
            }
            else
            {
                IList list = _tStockRepository.GetSisaStockList(det.ItemId, mWarehouse);
                TStock stock;
                decimal? qty = det.TransDetQty;
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
                    //stockRef.StockId = stock;
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
                    stockRef.StockRefDate = Trans.TransDate;
                    stockRef.StockRefStatus = Trans.TransStatus;
                    stockRef.StockRefDesc = det.TransDetDesc;
                    stockRef.CreatedBy = User.Identity.Name;
                    stockRef.CreatedDate = DateTime.Now;
                    stockRef.DataStatus = EnumDataStatus.New.ToString();
                    _tStockRefRepository.Save(stockRef);

                    qty = qty - sisa;
                    if (qty <= 0)
                    {
                        break;
                    }
                }


            }

        }

        public ActionResult Budgeting()
        {
            TransactionFormViewModel viewModel = TransactionFormViewModel.CreateTransactionFormViewModel(_tTransRepository, _mWarehouseRepository, _mSupplierRepository, _mUnitTypeRepository, _mJobTypeRepository);
            viewModel.Trans = SetNewTrans(EnumTransactionStatus.Budgeting);
            SetViewModelByStatus(viewModel, EnumTransactionStatus.Budgeting);


            ListDetTrans = new List<TTransDet>();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Budgeting(TTrans Trans, FormCollection formCollection)
        {
            return SaveTransaction(Trans, formCollection);
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
    }
}
