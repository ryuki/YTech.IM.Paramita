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
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.Transaction.Unit;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Web.Controllers.ViewModel
{
    public class UnitSalesFormViewModel
    {
        public static UnitSalesFormViewModel CreateUnitSalesFormViewModel(IMCustomerRepository mCustomerRepository)
        {
            UnitSalesFormViewModel viewModel = new UnitSalesFormViewModel();
            
            TTransUnit t = new TTransUnit();
            t.SetAssignedIdTo(Guid.NewGuid().ToString());
            t.TransUnitDate = DateTime.Today;
            

            viewModel.TransUnit = t;
            IList<MCustomer> listCust = mCustomerRepository.GetAll();
            var vals = from customer in listCust
                         select new { ID = customer.Id, Name = customer.PersonId.PersonName };

            //MCustomer cust = new MCustomer();
            //cust.CustomerDesc = "-Pilih Pembeli-";
            //vals.Union( .Insert(0, cust));
            viewModel.CustomerList = new SelectList(vals, "ID", "Name");
            
            return viewModel;
        }

        public TTransUnit TransUnit { get; internal set; } 

        public SelectList CustomerList { get; internal set; } 
        public string Title { get; internal set; }
    }
}
