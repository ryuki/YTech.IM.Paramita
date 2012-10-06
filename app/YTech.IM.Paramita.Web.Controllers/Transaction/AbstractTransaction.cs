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
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.Transaction.Payment;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Web.Controllers.ViewModel;

namespace YTech.IM.Paramita.Web.Controllers.Transaction
{
    public abstract class AbstractTransaction
    {
        public abstract void SaveJournal(TTrans trans, decimal totalHPP);

        //public abstract void SaveJournal(TPayment payment);

        public string UserName;
        public IMAccountRefRepository AccountRefRepository;
        public ITJournalRepository JournalRepository;
        public ITJournalRefRepository JournalRefRepository;

        protected string Desc = string.Empty;
        protected string NewVoucher = string.Empty;

        protected TJournal SaveJournalHeader(MCostCenter costCenterId, string newVoucher, string journalPic, DateTime? journalDate, string journalEvidenceNo, string desc)
        {

            TJournal j = new TJournal();
            j.SetAssignedIdTo(Guid.NewGuid().ToString());
            j.CostCenterId = costCenterId;
            j.JournalType = EnumJournalType.GeneralLedger.ToString();
            j.JournalVoucherNo = newVoucher;
            j.JournalPic = journalPic;
            j.JournalDate = journalDate;
            j.JournalEvidenceNo = journalEvidenceNo;
            //j.JournalAmmount = ammount;
            j.JournalDesc = desc;

            j.DataStatus = EnumDataStatus.New.ToString();
            j.CreatedBy = UserName;
            j.CreatedDate = DateTime.Now;
            j.JournalDets.Clear();

            return j;
        }

        public void DeleteJournal(EnumReferenceTable referenceTable, string referenceType, string referenceId)
        {
            TJournalRef journalRef = JournalRefRepository.GetByReference(referenceTable,
                                                                         referenceType, referenceId);
            if (journalRef != null)
            {
                TJournal journalToDelete = journalRef.JournalId;
                JournalRefRepository.Delete(journalRef);
                JournalRepository.Delete(journalToDelete);
            }
        }

        protected void SaveJournalRef(TJournal j, string referenceId, string referenceType, string desc)
        {
            TJournalRef journalRef = new TJournalRef();
            journalRef.SetAssignedIdTo(Guid.NewGuid().ToString());
            journalRef.JournalId = j; ;
            journalRef.ReferenceTable = EnumReferenceTable.Transaction.ToString();
            journalRef.ReferenceType = referenceType;
            journalRef.ReferenceId = referenceId;
            journalRef.JournalRefDesc = desc;
            journalRef.CreatedBy = UserName;
            journalRef.CreatedDate = DateTime.Now;
            journalRef.DataStatus = EnumDataStatus.New.ToString();
            JournalRefRepository.Save(journalRef);
        }

        protected void SaveJournalDet(TJournal journal, string newVoucher, MAccount accountId, EnumJournalStatus journalStatus, decimal ammount, string evidenceNo, string desc)
        {
            TJournalDet detToInsert = new TJournalDet(journal);
            detToInsert.SetAssignedIdTo(Guid.NewGuid().ToString());
            detToInsert.AccountId = accountId;
            detToInsert.JournalDetStatus = journalStatus.ToString();
            detToInsert.JournalDetEvidenceNo = evidenceNo;
            detToInsert.JournalDetAmmount = ammount;
            detToInsert.JournalDetNo = 0;
            detToInsert.JournalDetDesc = desc;
            detToInsert.CreatedBy = UserName;
            detToInsert.CreatedDate = DateTime.Now;
            detToInsert.DataStatus = Enums.EnumDataStatus.New.ToString();
            journal.JournalDets.Add(detToInsert);
        }
    }
}
