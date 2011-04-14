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
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Web.Controllers.Helper;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel
{
   public class RealFormViewModel
    {
       public RealFormViewModel Create(IMCostCenterRepository mCostCenterRepository)
       {
           RealFormViewModel viewModel = new RealFormViewModel();

           TReal r = new TReal();

           viewModel.CostCenterList = FillHelper.GetCostCenters(mCostCenterRepository, "-Pilih Cost Center-");
           viewModel.CostCenterList = FillHelper.GetMonths("-Pilih Bulan-");
           viewModel.YearList = FillHelper.GetYears("-Pilih Tahun-");
           return viewModel;
       }

       public TReal Real { get; internal set; }
       public SelectList CostCenterList { get; internal set; }
       public SelectList MonthList { get; internal set; }
       public SelectList YearList { get; internal set; }
    }
}
