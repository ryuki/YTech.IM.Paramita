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
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Web.Controllers.Helper;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel
{
    public class UnitViewModel
    {
        public static UnitViewModel Create(IMCostCenterRepository mCostCenterRepository)
        {
            UnitViewModel viewModel = new UnitViewModel();

            viewModel.CostCenterList = FillHelper.GetCostCenters(mCostCenterRepository, null);

            return viewModel;
        }

        public SelectList CostCenterList { get; internal set; }
    }
}
