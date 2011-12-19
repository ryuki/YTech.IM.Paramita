using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core;

namespace YTech.IM.Paramita.Web.Controllers.Helper
{
    public class AccountHelper
    {
        private static MAccount GetDefaultAccount(EnumReferenceType referenceType)
        {
            IMAccountRepository mAccountRepository = new MAccountRepository();
            string accountId = CommonHelper.GetReference(referenceType).ReferenceValue;
            MAccount account = mAccountRepository.Get(accountId);
            return account;
        }
        private static MAccount GetDefaultAccount(DefaultAccount defaultAccount, EnumReferenceType referenceType)
        {
            //check in cache first
            object obj = System.Web.HttpContext.Current.Cache[defaultAccount.ToString()];
            //if not available, set it first
            if (obj == null)
            {
                MAccount account = GetDefaultAccount(referenceType);
                //save to cache
                System.Web.HttpContext.Current.Cache[defaultAccount.ToString()] = account;
            }
            //return cache
            return System.Web.HttpContext.Current.Cache[defaultAccount.ToString()] as MAccount;
        }

        public static MAccount GetPurchaseAccount()
        {
            return GetDefaultAccount(DefaultAccount.PurchaseAccount,EnumReferenceType.PurchaseAccountId);
        }

        public static MAccount GetCashAccount()
        {
            return GetDefaultAccount(DefaultAccount.CashAccount, EnumReferenceType.CashAccountId);
        }

        public static MAccount GetHppAccount()
        {
            return GetDefaultAccount(DefaultAccount.HppAccount, EnumReferenceType.HppAccountId);
        }

        internal static MAccount GetSalesAccount()
        {
            return GetDefaultAccount(DefaultAccount.SalesAccount, EnumReferenceType.SalesAccountId);
        }

        internal static MAccount GetReturPurchaseAccount()
        {
            return GetDefaultAccount(DefaultAccount.ReturPurchaseAccount, EnumReferenceType.ReturPurchaseAccountId);
        }

        internal static MAccount GetReturSalesAccount()
        {
            return GetDefaultAccount(DefaultAccount.ReturSalesAccount, EnumReferenceType.ReturSalesAccountId);
        }
    }

    internal enum DefaultAccount
    {
        PurchaseAccount,
        CashAccount,
        HppAccount,
        SalesAccount,
        ReturPurchaseAccount,
        ReturSalesAccount
    }
}
