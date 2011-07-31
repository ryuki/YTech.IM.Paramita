using System;
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
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Web.Controllers.ViewModel;

namespace YTech.IM.Paramita.Web.Controllers.Transaction
{
    [HandleError]
    public class AccountingController : Controller
    {
        //public AccountingController()
        //    : this(new TJournalRepository(), new TJournalDetRepository(), new MCostCenterRepository(), new MAccountRepository(), new TRecAccountRepository(), new TRecPeriodRepository(), new MAccountCatRepository())
        //{ }

        private readonly ITJournalRepository _tJournalRepository;
        private readonly ITJournalDetRepository _tJournalDetRepository;
        private readonly IMCostCenterRepository _mCostCenterRepository;
        private readonly IMAccountRepository _mAccountRepository;
        private readonly ITRecAccountRepository _tRecAccountRepository;
        private readonly ITRecPeriodRepository _tRecPeriodRepository;
        private readonly IMAccountCatRepository _mAccountCatRepository;

        public AccountingController(ITJournalRepository tJournalRepository, ITJournalDetRepository tJournalDetRepository, IMCostCenterRepository mCostCenterRepository, IMAccountRepository mAccountRepository, ITRecAccountRepository tRecAccountRepository, ITRecPeriodRepository tRecPeriodRepository, IMAccountCatRepository mAccountCatRepository)
        {
            Check.Require(tJournalRepository != null, "tJournalRepository may not be null");
            Check.Require(tJournalDetRepository != null, "tJournalDetRepository may not be null");
            Check.Require(mCostCenterRepository != null, "mCostCenterRepository may not be null");
            Check.Require(mAccountRepository != null, "mAccountRepository may not be null");
            Check.Require(tRecAccountRepository != null, "tRecAccountRepository may not be null");
            Check.Require(tRecPeriodRepository != null, "tRecPeriodRepository may not be null");
            Check.Require(mAccountCatRepository != null, "mAccountCatRepository may not be null");

            this._tJournalRepository = tJournalRepository;
            this._tJournalDetRepository = tJournalDetRepository;
            this._mCostCenterRepository = mCostCenterRepository;
            this._mAccountRepository = mAccountRepository;
            this._tRecAccountRepository = tRecAccountRepository;
            this._tRecPeriodRepository = tRecPeriodRepository;
            this._mAccountCatRepository = mAccountCatRepository;
        }


        [Transaction]
        public ActionResult GeneralLedger()
        {
            GeneralLedgerViewModel viewModel = GeneralLedgerViewModel.CreateGeneralLedgerViewModel(_tJournalRepository, _mCostCenterRepository);
            viewModel.Journal = SetNewJournal(EnumJournalType.GeneralLedger);
            viewModel.Title = "General Ledger";

            ListJournalDet = new List<TJournalDet>();
            ViewData["CurrentItem"] = viewModel.Title;
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GeneralLedger(TJournal journal, FormCollection formCollection)
        {
            return SaveJournal(journal, formCollection);
        }

        [Transaction]
        public ActionResult CashIn()
        {
            CashFormViewModel viewModel = CashFormViewModel.CreateCashFormViewModel(_tJournalRepository, _mCostCenterRepository, _mAccountRepository);
            viewModel.Journal = SetNewJournal(EnumJournalType.CashIn);
            viewModel.Title = "Kas Masuk";

            ListJournalDet = new List<TJournalDet>();
            ViewData["CurrentItem"] = viewModel.Title;
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CashIn(TJournal journal, FormCollection formCollection)
        {
            return SaveJournalInterface(journal, formCollection);
        }

        private ActionResult SaveJournalInterface(TJournal journal, FormCollection formCollection)
        {
            if (formCollection["btnSave"] != null)
                return SaveJournal(journal, formCollection);
            else if (formCollection["btnPrint"] != null || formCollection["btnPrintKwitansi"] != null)
            {
                //save data to session
                SetDataForPrint(journal.Id);
                string reportUrl = string.Empty;
                if (formCollection["btnPrint"] != null)
                {
                    reportUrl = Url.Content("~/ReportViewer.aspx?rpt=RptPrintCash");
                }
                else if (formCollection["btnPrintKwitansi"] != null)
                {
                    reportUrl = Url.Content("~/ReportViewer.aspx?rpt=RptPrintKwitansi");
                }

                var e = new
                {
                    Success = false,
                    Message = "redirect",
                    Data = reportUrl
                };
                return Json(e, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        [Transaction]
        public ActionResult CashOut()
        {
            CashFormViewModel viewModel = CashFormViewModel.CreateCashFormViewModel(_tJournalRepository, _mCostCenterRepository, _mAccountRepository);
            viewModel.Journal = SetNewJournal(EnumJournalType.CashOut);
            viewModel.Title = "Kas Keluar";

            ListJournalDet = new List<TJournalDet>();
            ViewData["CurrentItem"] = viewModel.Title;
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CashOut(TJournal journal, FormCollection formCollection)
        {
            return SaveJournalInterface(journal, formCollection);
        }

        private void SetDataForPrint(string journalId)
        {
            ReportDataSource[] repCol = new ReportDataSource[1];
            IList<TJournalDet> listDets = _tJournalDetRepository.GetDetailByJournalId(journalId);
            var detailHeaders = from det in listDets
                                where det.JournalDetNo.Value == 0
                                select det
            ;
            TJournalDet detailHeader = new TJournalDet();
            if (detailHeaders != null)
            {
                detailHeader = (detailHeaders.ToList() as IList<TJournalDet>)[0];
            }

            var list = from det in listDets
                       where det.JournalDetNo != 0
                       select new
                       {
                           AccountId = detailHeader.AccountId.Id,
                           detailHeader.AccountId.AccountName,
                           JournalDate = det.JournalId != null ? det.JournalId.JournalDate : null,
                           CostCenterId = det.JournalId.CostCenterId.Id,
                           det.JournalId.CostCenterId.CostCenterName,
                           JournalVoucherNo = det.JournalId.JournalVoucherNo,
                           JournalId = det.JournalId.Id,
                           det.JournalId.JournalPic,
                           det.JournalId.JournalPic2,
                           det.JournalDetAmmount,
                           det.JournalDetDesc,
                           det.JournalDetEvidenceNo,
                           det.JournalDetNo,
                           det.JournalDetStatus,
                           DetAccountId = det.AccountId.Id,
                           DetAccountName = det.AccountId.AccountName,
                           JournalType = det.JournalId.JournalType
                       }
     ;
            ReportDataSource reportDataSource = new ReportDataSource("CashJournalViewModel", list.ToList());
            repCol[0] = reportDataSource;
            Session["ReportData"] = repCol;
        }

        private ActionResult SaveJournal(TJournal journal, FormCollection formCollection)
        {
            string Message = string.Empty;
            bool Success = true;
            string voucherNo = string.Empty;
            try
            {
                _tJournalRepository.DbContext.BeginTransaction();

                //check first
                TJournal journal1 = _tJournalRepository.Get(formCollection["Journal.Id"]);
                voucherNo = journal.JournalVoucherNo;
                if (journal1 != null)
                {
                    _tJournalRepository.Delete(journal1);
                }
                else
                {
                    voucherNo = Helper.CommonHelper.GetVoucherNo(false);
                }

                if (journal == null)
                {
                    journal = new TJournal();
                }
                journal.SetAssignedIdTo(formCollection["Journal.Id"]);
                journal.CostCenterId = _mCostCenterRepository.Get(formCollection["Journal.CostCenterId"]);
                journal.JournalVoucherNo = voucherNo;
                journal.CreatedDate = DateTime.Now;
                journal.CreatedBy = User.Identity.Name;
                journal.DataStatus = Enums.EnumDataStatus.New.ToString();
                journal.JournalDets.Clear();

                TJournalDet detToInsert;
                decimal total = 0;
                foreach (TJournalDet det in ListJournalDet)
                {
                    detToInsert = new TJournalDet(journal);
                    detToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
                    detToInsert.AccountId = det.AccountId;

                    if (journal.JournalType == EnumJournalType.CashIn.ToString())
                    {
                        detToInsert.JournalDetStatus = EnumJournalStatus.K.ToString();
                    }
                    else if (journal.JournalType == EnumJournalType.CashOut.ToString())
                    {
                        detToInsert.JournalDetStatus = EnumJournalStatus.D.ToString();
                    }
                    else if (journal.JournalType == EnumJournalType.GeneralLedger.ToString())
                    {
                        detToInsert.JournalDetStatus = det.JournalDetStatus;
                    }

                    detToInsert.JournalDetAmmount = det.JournalDetAmmount;
                    detToInsert.JournalDetNo = det.JournalDetNo;
                    detToInsert.JournalDetDesc = det.JournalDetDesc;
                    detToInsert.CreatedBy = User.Identity.Name;
                    detToInsert.CreatedDate = DateTime.Now;
                    detToInsert.DataStatus = Enums.EnumDataStatus.New.ToString();
                    journal.JournalDets.Add(detToInsert);

                    total += det.JournalDetAmmount.Value;
                }

                //add new detail for cash in / out 
                if (journal.JournalType == EnumJournalType.CashIn.ToString() || journal.JournalType == EnumJournalType.CashOut.ToString())
                {
                    detToInsert = new TJournalDet(journal);
                    detToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
                    detToInsert.AccountId = _mAccountRepository.Get(formCollection["CashAccountId"]);

                    if (journal.JournalType == EnumJournalType.CashIn.ToString())
                    {
                        detToInsert.JournalDetStatus = EnumJournalStatus.D.ToString();
                    }
                    else if (journal.JournalType == EnumJournalType.CashOut.ToString())
                    {
                        detToInsert.JournalDetStatus = EnumJournalStatus.K.ToString();
                    }

                    detToInsert.JournalDetAmmount = total;
                    detToInsert.JournalDetNo = 0;
                    detToInsert.JournalDetDesc = journal.JournalDesc;
                    detToInsert.CreatedBy = User.Identity.Name;
                    detToInsert.CreatedDate = DateTime.Now;
                    detToInsert.DataStatus = Enums.EnumDataStatus.New.ToString();
                    journal.JournalDets.Add(detToInsert);
                }
                _tJournalRepository.Save(journal);

                _tJournalRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
                Message = "Data berhasil disimpan.";
            }
            catch (Exception ex)
            {
                _tJournalRepository.DbContext.RollbackTransaction();
                Success = false;
                Message = ex.GetBaseException().Message;
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
            }
            var e = new
            {
                Success,
                Message,
                voucherNo
            };
            return
                Json(e, JsonRequestBehavior.AllowGet);
            //View("Status");
            //return RedirectToAction(journal.JournalType);
        }

        private TJournal SetNewJournal(EnumJournalType journalType)
        {
            TJournal journal = new TJournal();
            journal.SetAssignedIdTo(Guid.NewGuid().ToString());
            journal.JournalDate = DateTime.Today;
            journal.JournalType = journalType.ToString();
            // journal.JournalVoucherNo = "<Otomatis, dikosongkan saja.>"; //Helper.CommonHelper.GetVoucherNo();
            return journal;
        }

        private List<TJournalDet> ListJournalDet
        {
            get
            {
                //if (Session["ListJournalDet"] == null)
                //{
                //    Session["ListJournalDet"] = new List<TJournalDet>();
                //}
                return Session["ListJournalDet"] as List<TJournalDet>;
            }
            set
            {
                Session["ListJournalDet"] = value;
            }
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var journalDets = ListJournalDet;
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            //var totalDebet = from det in journalDets
            //                 where det.JournalDetStatus == "D"
            //                 select det.JournalDetAmmount;
            //var totalKredit = from det in journalDets
            //                  where det.JournalDetStatus == "K"
            //                  select det.JournalDetAmmount;
            var jsonData = new
             {
                 total = totalPages,
                 page = page,
                 records = totalRecords,
                 rows = (
                     from det in journalDets
                     select new
                     {
                         i = det.Id.ToString(),
                         cell = new string[] {
                             det.Id,
                            det.AccountId != null ? det.AccountId.Id : null, 
                            det.AccountId != null ? det.AccountId.AccountName : null,
                         det.JournalDetEvidenceNo,
                         det.JournalDetStatus,
                         det.JournalDetAmmount.Value.ToString(Helper.CommonHelper.NumberFormat) ,
                        det.JournalDetStatus == "D" ? det.JournalDetAmmount.Value.ToString(Helper.CommonHelper.NumberFormat) : "",
                           det.JournalDetStatus == "K" ?   det.JournalDetAmmount.Value.ToString(Helper.CommonHelper.NumberFormat) : "",
                            det.JournalDetDesc
                        }
                     }).ToArray(),
                 userdata = new
                                {
                                    JournalDetEvidenceNo = "Total",
                                    JournalDetAmmount = journalDets.Sum(det => det.JournalDetAmmount).HasValue ? journalDets.Sum(det => det.JournalDetAmmount).Value.ToString(Helper.CommonHelper.NumberFormat) : "0",
                                    JournalDetAmmountDebet = journalDets.Where(d => d.JournalDetStatus == "D").Sum(det => det.JournalDetAmmount).HasValue ? journalDets.Where(d => d.JournalDetStatus == "D").Sum(det => det.JournalDetAmmount).Value.ToString(Helper.CommonHelper.NumberFormat) : "0",
                                    //totalDebet.Sum().HasValue ? totalDebet.Sum().Value.ToString(Helper.CommonHelper.NumberFormat) : "0",
                                    JournalDetAmmountKredit = journalDets.Where(d => d.JournalDetStatus == "K").Sum(det => det.JournalDetAmmount).HasValue ? journalDets.Where(d => d.JournalDetStatus == "K").Sum(det => det.JournalDetAmmount).Value.ToString(Helper.CommonHelper.NumberFormat) : "0"
                                    //totalKredit.Sum().HasValue ? totalKredit.Sum().Value.ToString(Helper.CommonHelper.NumberFormat) : "0"
                                }
             };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Insert(TJournalDet viewModel, FormCollection formCollection)
        {
            UpdateNumericData(viewModel, formCollection);
            TJournalDet journalDet = new TJournalDet();

            TransferFormValuesTo(journalDet, viewModel);
            journalDet.SetAssignedIdTo(Guid.NewGuid().ToString());
            journalDet.AccountId = _mAccountRepository.Get(formCollection["AccountId"]);
            journalDet.SetAssignedIdTo(viewModel.Id);
            journalDet.CreatedDate = DateTime.Now;
            journalDet.CreatedBy = User.Identity.Name;
            journalDet.DataStatus = EnumDataStatus.New.ToString();


            ListJournalDet.Add(journalDet);
            return Content("Detail transaksi berhasil disimpan");
        }

        private void UpdateNumericData(TJournalDet viewModel, FormCollection formCollection)
        {
            if (!string.IsNullOrEmpty(formCollection["JournalDetAmmount"]))
            {
                string amm = formCollection["JournalDetAmmount"].Replace(",", "");
                decimal? ammount = Convert.ToDecimal(amm);
                viewModel.JournalDetAmmount = ammount;
            }
        }

        private void TransferFormValuesTo(TJournalDet journalDet, TJournalDet viewModel)
        {
            journalDet.JournalDetNo = ListJournalDet.Count + 1;
            journalDet.JournalDetStatus = viewModel.JournalDetStatus;
            journalDet.JournalDetAmmount = viewModel.JournalDetAmmount;
            journalDet.JournalDetDesc = viewModel.JournalDetDesc;
            journalDet.JournalDetEvidenceNo = viewModel.JournalDetEvidenceNo;
        }

        public ActionResult Delete(TJournalDet viewModel, FormCollection formCollection)
        {
            ListJournalDet.Remove(viewModel);
            return Content("Detail transaksi berhasil dihapus");
        }

        public ActionResult Update(TJournalDet viewModel, FormCollection formCollection)
        {
            UpdateNumericData(viewModel, formCollection);
            TJournalDet journalDet = new TJournalDet();
            TransferFormValuesTo(journalDet, viewModel);
            journalDet.SetAssignedIdTo(Guid.NewGuid().ToString());
            journalDet.AccountId = _mAccountRepository.Get(formCollection["AccountId"]);
            journalDet.SetAssignedIdTo(viewModel.Id);
            journalDet.CreatedDate = DateTime.Now;
            journalDet.CreatedBy = User.Identity.Name;
            journalDet.DataStatus = EnumDataStatus.New.ToString();

            ListJournalDet.Add(journalDet);
            return Content("Detail transaksi berhasil disimpan");
        }

        [Transaction]
        public ActionResult Closing()
        {
            ClosingViewModel viewModel = ClosingViewModel.CreateClosingViewModel(_tRecPeriodRepository, _tJournalRepository);
            viewModel.DateTo = DateTime.Today;

            ListJournalDet = new List<TJournalDet>();
            ViewData["CurrentItem"] = "Tutup Buku";
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Closing(ClosingViewModel viewModel, FormCollection formCollection)
        {
            TRecPeriod recPeriod = new TRecPeriod();
            recPeriod.SetAssignedIdTo(Guid.NewGuid().ToString());
            recPeriod.PeriodFrom = viewModel.DateFrom.Value;
            recPeriod.PeriodTo = viewModel.DateTo.Value;
            recPeriod.PeriodType = EnumPeriodType.Custom.ToString();
            recPeriod.PeriodDesc = string.Format("{0:dd-MMM-yyyy} s/d {1:dd-MMM-yyyy}", viewModel.DateFrom.Value, viewModel.DateTo.Value);
            recPeriod.CreatedBy = User.Identity.Name;
            recPeriod.CreatedDate = DateTime.Now;

            try
            {
                _tRecPeriodRepository.Save(recPeriod);
                _tRecPeriodRepository.DbContext.CommitChanges();
                _tRecAccountRepository.RunClosing(recPeriod);
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
            }
            catch (Exception)
            {
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
            }
            return RedirectToAction("Closing");
        }

        [Transaction]
        public ActionResult Opening()
        {
            OpeningViewModel viewModel = OpeningViewModel.Create(_tRecPeriodRepository);
            ViewData["CurrentItem"] = "Buka Buku";
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Opening(OpeningViewModel viewModel, FormCollection formCollection)
        {
            _tRecPeriodRepository.DbContext.BeginTransaction();
            try
            {
                if (!string.IsNullOrEmpty(viewModel.RecPeriodId))
                {
                    _tRecPeriodRepository.DeleteByRecPeriodId(viewModel.RecPeriodId);
                    _tRecPeriodRepository.DbContext.CommitChanges();
                }

                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
            }
            catch (Exception)
            {
                _tRecPeriodRepository.DbContext.RollbackTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
            }
            return RedirectToAction("Opening");
        }

        [Transaction]
        public ActionResult ListCash()
        {
            return View();
        }

        [Transaction]
        public ActionResult ListSearchCash(string sidx, string sord, int page, int rows, string searchBy, string searchText, string journalType)
        {
            int totalRecords = 0;
            var journalList = _tJournalRepository.GetPagedJournalList(sidx, sord, page, rows, ref totalRecords, searchBy, searchText, journalType);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from journal in journalList
                    select new
                    {
                        i = journal.Id.ToString(),
                        cell = new string[] {
                            journal.Id, 
                            journal.JournalVoucherNo, 
                            journal.JournalDate.HasValue ? journal.JournalDate.Value.ToString(Helper.CommonHelper.DateFormat): null,
                            journal.JournalEvidenceNo,
                            journal.JournalPic,
                            journal.JournalDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult GetJsonJournal(string journalId)
        {
            TJournal journal = _tJournalRepository.Get(journalId);

            MAccount cashAccount = null;
            string detailStatus = journal.JournalType == EnumJournalType.CashIn.ToString()
                                      ? EnumJournalStatus.D.ToString()
                                      : EnumJournalStatus.K.ToString();
            cashAccount = (from s in journal.JournalDets
                           where s.JournalDetStatus == detailStatus
                           select s.AccountId).First();

            IList<TJournalDet> others = (from s in journal.JournalDets
                                         where s.JournalDetStatus != detailStatus
                                         select s).ToList();
            ListJournalDet = others.ToList();
            var t = new
            {
                JournalId = journal.Id,
                journal.JournalDate,
                journal.JournalVoucherNo,
                CostCenterId = journal.CostCenterId != null ? journal.CostCenterId.Id : null,
                CashAccountId = cashAccount != null ? cashAccount.Id : null,
                CashAccountName = cashAccount != null ? cashAccount.AccountName : null,
                journal.JournalPic,
                journal.JournalPic2,
                journal.JournalDesc
            };
            return Json(t, JsonRequestBehavior.AllowGet);
        }
    }
}
