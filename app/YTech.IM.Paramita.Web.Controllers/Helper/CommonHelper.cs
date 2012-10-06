using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core;
using YTech.IM.Paramita.Web.Controllers.ViewModel;

namespace YTech.IM.Paramita.Web.Controllers.Helper
{
    public class CommonHelper
    {
        public static string DateFormat
        {
            get { return "dd-MMM-yyyy"; }
        }
        public static string DateTimeFormat
        {
            get { return "dd-MMM-yyyy HH:mm"; }
        }
        public static string PeriodFormat
        {
            get { return "MMM-yyyy"; }
        }
        public static string NumberFormat
        {
            get { return "N2"; }
        }
        public static string IntegerFormat
        {
            get { return "N0"; }
        }

        public static TReference GetReference(EnumReferenceType referenceType)
        {
            ITReferenceRepository referenceRepository = new TReferenceRepository();
            TReference reference = referenceRepository.GetByReferenceType(referenceType);
            if (reference == null)
            {
                reference = new TReference();
                reference.SetAssignedIdTo(Guid.NewGuid().ToString());
                reference.ReferenceType = referenceType.ToString();
                reference.ReferenceValue = "0";
                reference.CreatedDate = DateTime.Now;
                reference.DataStatus = EnumDataStatus.New.ToString();
                referenceRepository.Save(reference);
                referenceRepository.DbContext.CommitChanges();
            }
            return reference;
        }

        public static string GetFacturNo(EnumTransactionStatus transactionStatus)
        {
            return GetFacturNo(transactionStatus, true);
        }

        public static string GetFacturNo(EnumTransactionStatus transactionStatus, bool automatedIncrease)
        {
            TReference refer = GetReference((EnumReferenceType)Enum.Parse(typeof(EnumReferenceType), transactionStatus.ToString()));
            decimal no = Convert.ToDecimal(refer.ReferenceValue) + 1;
            refer.ReferenceValue = no.ToString();
            if (automatedIncrease)
            {
                ITReferenceRepository referenceRepository = new TReferenceRepository();
                referenceRepository.Update(refer);
                referenceRepository.DbContext.CommitChanges();
            }

            string tipeTrans = string.Empty;
            char[] charTransArray = transactionStatus.ToString().ToCharArray();
            char charTrans;

            for (int i = 0; i < transactionStatus.ToString().Length; i++)
            {
                charTrans = charTransArray[i];
                if (char.IsUpper(transactionStatus.ToString(), i))
                    tipeTrans += transactionStatus.ToString().Substring(i, 1);
            }
            string formatFactur = "GRAHA/[TRANS]/[XXX]/[MONTH]/[YEAR]";
            StringBuilder result = new StringBuilder();
            result.Append(formatFactur);
            result.Replace("[TRANS]", tipeTrans);
            result.Replace("[XXX]", GetFactur(5, no));
            result.Replace("[MONTH]", DateTime.Today.ToString("MMM").ToUpper());
            result.Replace("[YEAR]", DateTime.Now.Year.ToString());
            return result.ToString();
        }

        public static string GetVoucherNo()
        {
            return GetVoucherNo(false);
        }

        public static string GetVoucherNo(bool automatedIncrease)
        {
            return GetVoucherNo(false, EnumJournalType.GeneralLedger);
        }

        public static string GetVoucherNo(bool automatedIncrease, EnumJournalType journalType)
        {
            TReference refer = GetReference(EnumReferenceType.VoucherNo);
            decimal no = Convert.ToDecimal(refer.ReferenceValue) + 1;
            refer.ReferenceValue = no.ToString();
            if (automatedIncrease)
            {
                ITReferenceRepository referenceRepository = new TReferenceRepository();
                referenceRepository.DbContext.BeginTransaction();
                referenceRepository.Update(refer);
                referenceRepository.DbContext.CommitTransaction();
            }

            string formatFactur = "GRAHA/[TYPE]/[XXX]/[MONTH]/[YEAR]";
            StringBuilder result = new StringBuilder();
            result.Append(formatFactur);
            result.Replace("[TYPE]", GetStringValue(journalType));
            result.Replace("[XXX]", GetFactur(5, no));
            result.Replace("[MONTH]", DateTime.Today.ToString("MMM").ToUpper());
            result.Replace("[YEAR]", DateTime.Now.Year.ToString());
            return result.ToString();
        }

        private static string GetFactur(int maxLength, decimal no)
        {
            int len = maxLength - no.ToString().Length;
            string factur = no.ToString();
            for (int i = 0; i < len; i++)
            {
                factur = "0" + factur;
            }
            return factur;
        }
        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : value.ToString();
        }

        public static void SetViewModelByStatus(TransactionFormViewModel viewModel, EnumTransactionStatus enumTransactionStatus)
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
                    viewModel.IsGenerateFactur = true;
                    viewModel.IsCalculateStock = false;
                    viewModel.IsAddStock = true;
                    break;
                case EnumTransactionStatus.Purchase:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = false;
                    viewModel.ViewSupplier = true;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = true;
                    viewModel.ViewPaymentMethod = true;
                    viewModel.IsGenerateFactur = false;
                    viewModel.IsCalculateStock = false;
                    viewModel.IsAddStock = true;
                    break;
                case EnumTransactionStatus.ReturPurchase:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = false;
                    viewModel.ViewSupplier = true;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = true;
                    viewModel.ViewPaymentMethod = true;
                    viewModel.IsGenerateFactur = true;
                    viewModel.IsCalculateStock = false;
                    viewModel.IsAddStock = true;
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
                    viewModel.ViewUnitType = true;
                    viewModel.ViewJobType = true;
                    viewModel.IsGenerateFactur = true;
                    viewModel.IsCalculateStock = true;
                    viewModel.IsAddStock = false;
                    break;
                case EnumTransactionStatus.Mutation:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = true;
                    viewModel.ViewSupplier = false;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = false;
                    viewModel.ViewPaymentMethod = false;
                    viewModel.IsGenerateFactur = true;
                    viewModel.IsCalculateStock = true;
                    viewModel.IsAddStock = false;
                    break;
                case EnumTransactionStatus.Adjusment:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = false;
                    viewModel.ViewSupplier = false;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = false;
                    viewModel.ViewPaymentMethod = false;
                    viewModel.IsGenerateFactur = true;
                    viewModel.IsCalculateStock = true;
                    viewModel.IsAddStock = false;
                    break;
                case EnumTransactionStatus.Received:
                    viewModel.ViewWarehouse = true;
                    viewModel.ViewWarehouseTo = false;
                    viewModel.ViewSupplier = true;
                    viewModel.ViewDate = true;
                    viewModel.ViewFactur = true;
                    viewModel.ViewPrice = true;
                    viewModel.ViewPaymentMethod = false;
                    viewModel.IsGenerateFactur = true;
                    viewModel.IsCalculateStock = true;
                    viewModel.IsAddStock = true;
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
                    viewModel.IsGenerateFactur = true;
                    viewModel.IsCalculateStock = false;
                    viewModel.IsAddStock = true;
                    break;
            }
            viewModel.Title = GetStringValue(enumTransactionStatus);
        }

        internal static string GetItemUomName(IMItemUomRepository mItemUomRepository, MItem mItem)
        {
            if (mItem != null)
            {
                MItemUom itemUom = mItemUomRepository.GetByItem(mItem);
                if (itemUom != null)
                {
                    return itemUom.ItemUomName;
                }
            }
            return string.Empty;
        }

        internal static bool CheckStock(MWarehouse mWarehouse, MItem item, decimal? qty)
        {
            ITStockItemRepository stockItemRepository = new TStockItemRepository();
            TStockItem stockItem = stockItemRepository.GetByItemAndWarehouse(item, mWarehouse);
            if (stockItem != null)
            {
                if (stockItem.ItemStock >= qty)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
