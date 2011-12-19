using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Core.Transaction.Payment;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel
{
    public class PaymentViewModel
    {
        public static PaymentViewModel Create(EnumPaymentType paymentType, ITPaymentRepository tPaymentRepository, ITPaymentDetRepository tPaymentDetRepository, IMSupplierRepository mSupplierRepository, IMCustomerRepository mCustomerRepository, IMCostCenterRepository mCostCenterRepository)
        {
            PaymentViewModel viewModel = new PaymentViewModel();

            TPayment p = new TPayment();
            p.SetAssignedIdTo(Guid.NewGuid().ToString());
            p.PaymentDate = DateTime.Today;
            p.PaymentDesc = string.Empty;
            p.PaymentType = paymentType.ToString();

            viewModel.Payment = p;

            viewModel.Title = string.Format("Pembayaran {0}", paymentType.ToString());

            IList<MCostCenter> list = mCostCenterRepository.GetAll();
            MCostCenter costCenter = new MCostCenter();
            costCenter.CostCenterName = "-Pilih Cost Center-";
            list.Insert(0, costCenter);
            viewModel.CostCenterList = new SelectList(list, "Id", "CostCenterName");

            //get label text
            switch (paymentType)
            {
                case EnumPaymentType.Piutang:
                    viewModel.CashAccountLabel = "Deposit ke : ";

                    //fill cust
                    var values = from MCustomer cust in mCustomerRepository.GetAll()
                                 select new { Id = cust.Id, Name = cust.PersonId != null ? cust.PersonId.PersonName : "-Pilih Konsumen-" };
                    viewModel.TransByList = new SelectList(values, "Id", "Name");
                    break;
                case EnumPaymentType.Hutang:
                    viewModel.CashAccountLabel = "Deposit dari : ";

                    IList<MSupplier> listAcc = mSupplierRepository.GetAll();
                    MSupplier supplier = new MSupplier();
                    supplier.SupplierName = "-Pilih Supplier-";
                    listAcc.Insert(0, supplier);
                    viewModel.TransByList = new SelectList(listAcc, "Id", "SupplierName");
                    break;
            }
            return viewModel;
        }

        public TPayment Payment { get; internal set; }
        public string CashAccountId { get; internal set; }
        public string CashAccountName { get; internal set; }
        public string CashAccountLabel { get; internal set; }
        public string SelectedTransId { get; internal set; }
        public string CostCenterId { get; internal set; }

        public string Title { get; internal set; }
        public SelectList TransByList { get; internal set; }
        public SelectList CostCenterList { get; internal set; }
    }
}
