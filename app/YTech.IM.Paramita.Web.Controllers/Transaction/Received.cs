﻿using System;
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
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Web.Controllers.ViewModel;

namespace YTech.IM.Paramita.Web.Controllers.Transaction
{
    public class Received : AbstractTransaction
    {
        #region Overrides of AbstractTransaction

        public override void SaveJournal(TTrans trans, decimal totalHPP)
        {
            string desc = string.Format("Penerimaan Stok dari {0}", trans.TransBy);
            string newVoucher = Helper.CommonHelper.GetVoucherNo(false);
            //delete journal first
            DeleteJournal(EnumReferenceTable.Transaction, trans.TransStatus, trans.Id);
            //save header of journal
            TJournal journal = SaveJournalHeader(trans.WarehouseId.CostCenterId, newVoucher, trans.TransBy, trans.TransDate, trans.TransFactur, desc);
            MAccountRef accountRef = null;

            ////save persediaan
            //accountRef = AccountRefRepository.GetByRefTableId(EnumReferenceTable.Warehouse, trans.WarehouseId.Id);
            //SaveJournalDet(journal, newVoucher, accountRef.AccountId, EnumJournalStatus.D, totalHPP, trans.TransFactur, desc);

            ////save ikhtiar LR
            //SaveJournalDet(journal, newVoucher, Helper.AccountHelper.GetHppAccount(), EnumJournalStatus.K, totalHPP, trans.TransFactur, desc);

            JournalRepository.Save(journal);

            //save journal ref
            SaveJournalRef(journal, trans.Id, trans.TransStatus, trans.TransDesc);
        }

        #endregion
    }
}
