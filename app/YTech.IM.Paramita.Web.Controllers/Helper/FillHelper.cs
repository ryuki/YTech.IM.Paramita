using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Web.Controllers.Helper
{
    public class FillHelper
    {
        public static SelectList GetCostCenters(IMCostCenterRepository mCostCenterRepository, string defaultText)
        {
            IList<MCostCenter> list = mCostCenterRepository.GetAll();
            if (!string.IsNullOrEmpty(defaultText))
            {
                MCostCenter costCenter = new MCostCenter();
                costCenter.CostCenterName = defaultText;
                list.Insert(0, costCenter);
            }
            return new SelectList(list, "Id", "CostCenterName");
        }

        public static SelectList GetAccounts(IMAccountRepository mAccountRepository, string defaultText)
        {
            IList<MAccount> listAcc = mAccountRepository.GetAll();
            if (!string.IsNullOrEmpty(defaultText))
            {
                MAccount account = new MAccount();
                account.AccountName = defaultText;
                listAcc.Insert(0, account);
            }
            return new SelectList(listAcc, "Id", "AccountName");
        }

        internal static SelectList GetMonths(string defaultText)
        {
            var values = from EnumMonth e in Enum.GetValues(typeof(EnumMonth))
                         select new { ID = e, Name = CommonHelper.GetStringValue(e) };

            return new SelectList(values, "Id", "Name");
        }

        internal static SelectList GetYears(string defaultText)
        {
            ArrayList arr = new ArrayList();
            for (int i = DateTime.Today.Year - 5; i < DateTime.Today.Year + 5; i++)
            {
                var year = new { year = i };
                arr.Add(year);
            }
            return new SelectList(arr, "year", "year");
        }
    }
}
