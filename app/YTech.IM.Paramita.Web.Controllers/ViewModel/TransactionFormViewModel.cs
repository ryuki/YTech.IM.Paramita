using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel
{
    public class TransactionFormViewModel
    {
        public static TransactionFormViewModel Create(EnumTransactionStatus enumTransactionStatus, ITTransRepository transRepository, IMWarehouseRepository mWarehouseRepository, IMSupplierRepository mSupplierRepository, IMUnitTypeRepository mUnitTypeRepository, IMJobTypeRepository mJobTypeRepository)
        {
            TransactionFormViewModel viewModel = new TransactionFormViewModel();
            TTrans trans = SetNewTrans(enumTransactionStatus);
            viewModel.Trans = trans;
            Helper.CommonHelper.SetViewModelByStatus(viewModel, enumTransactionStatus);

            viewModel.TransFactur = trans.TransFactur;
            viewModel.TransDate = trans.TransDate;
            viewModel.TransId = trans.Id;
            viewModel.TransStatus = trans.TransStatus;
            viewModel.WarehouseId = trans.WarehouseId;
            viewModel.WarehouseIdTo = trans.WarehouseIdTo;
            viewModel.UnitTypeId = trans.UnitTypeId;
            viewModel.JobTypeId = trans.JobTypeId;
            viewModel.TransDesc = trans.TransDesc;

            IList<MWarehouse> list = mWarehouseRepository.GetAll();
            MWarehouse mWarehouse = new MWarehouse();
            mWarehouse.WarehouseName = "-Pilih Gudang-";
            list.Insert(0, mWarehouse);
            viewModel.WarehouseList = new SelectList(list, "Id", "WarehouseName");
            viewModel.WarehouseToList = new SelectList(list, "Id", "WarehouseName");

            IList<MSupplier> listSupplier = mSupplierRepository.GetAll();
            MSupplier mSupplier = new MSupplier();
            mSupplier.SupplierName = "-Pilih Supplier-";
            listSupplier.Insert(0, mSupplier);
            viewModel.SupplierList = new SelectList(listSupplier, "Id", "SupplierName");

            IList<MUnitType> listUnitType = mUnitTypeRepository.GetAll();
            MUnitType mUnitType = new MUnitType();
            mUnitType.UnitTypeName = "-Pilih Unit-";
            listUnitType.Insert(0, mUnitType);
            viewModel.UnitTypeList = new SelectList(listUnitType, "Id", "UnitTypeName");

            IList<MJobType> listJobType = mJobTypeRepository.GetAll();
            MJobType mJobType = new MJobType();
            mJobType.JobTypeName = "-Pilih Jenis Pekerjaan-";
            listJobType.Insert(0, mJobType);
            viewModel.JobTypeList = new SelectList(listJobType, "Id", "JobTypeName");

            //fill payment method
            var values = from EnumPaymentMethod e in Enum.GetValues(typeof(EnumPaymentMethod))
                         select new { ID = e, Name = e.ToString() };

            viewModel.PaymentMethodList = new SelectList(values, "Id", "Name");

            //viewModel.ViewWarehouseTo = false;
            //viewModel.ViewSupplier = false;
            //viewModel.ViewDate = false;
            //viewModel.ViewFactur = false;

            return viewModel;
        }

        private static TTrans SetNewTrans(EnumTransactionStatus enumTransactionStatus)
        {
            TTrans trans = new TTrans();
            trans.TransDate = DateTime.Today;
            if (enumTransactionStatus != EnumTransactionStatus.Purchase)
            {
                 trans.TransFactur = Helper.CommonHelper.GetFacturNo(enumTransactionStatus, false);
            }
           
            trans.SetAssignedIdTo(Guid.NewGuid().ToString());
            trans.TransStatus = enumTransactionStatus.ToString();
            return trans;
        }

        public TTrans Trans { get; internal set; }

        public string TransId { get; set; }
        [Required(ErrorMessage = "No Faktur harus diisi.")]
        public string TransFactur { get; set; }

        [Required(ErrorMessage = "Pilih gudang.")]
        public MWarehouse WarehouseId { get; set; }
        [Required(ErrorMessage = "Pilih gudang tujuan.")]
        public MWarehouse WarehouseIdTo { get; set; }
        [Required(ErrorMessage = "Pilih tipe unit.")]
        public MUnitType UnitTypeId { get; set; }
        [Required(ErrorMessage = "Tanggal harus diisi.")]
        public DateTime? TransDate { get; set; }
        [Required(ErrorMessage = "Pilih supplier.")]
        public string TransBy { get; set; }
        [Required(ErrorMessage = "Pilih cara pembayaran.")]
        public string TransPaymentMethod { get; set; }
        public string TransStatus { get; set; }
        public string TransDesc { get; set; }
        [Required(ErrorMessage = "Pilih jenis pekerjaan.")]
        public MJobType JobTypeId { get; set; }

        public IList<TTransDet> ListOfTransDet { get; internal set; }

        public SelectList WarehouseList { get; internal set; }
        public SelectList WarehouseToList { get; internal set; }
        public SelectList SupplierList { get; internal set; }
        public SelectList PaymentMethodList { get; internal set; }
        public SelectList UnitTypeList { get; internal set; }
        public SelectList JobTypeList { get; internal set; }
        public bool ViewWarehouse { get; internal set; }
        public bool ViewWarehouseTo { get; internal set; }
        public bool ViewSupplier { get; internal set; }
        public bool ViewDate { get; internal set; }
        public bool ViewFactur { get; internal set; }
        public bool ViewPrice { get; internal set; }
        public bool ViewPaymentMethod { get; internal set; }
        public bool ViewUnitType { get; internal set; }
        public bool ViewJobType { get; internal set; }
        public string Title { get; internal set; }
        public bool IsAddStock { get; internal set; }
        public bool IsGenerateFactur { get; internal set; }
        public bool IsCalculateStock { get; internal set; }

    }
}
