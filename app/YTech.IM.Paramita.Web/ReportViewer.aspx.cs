using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Web.Controllers.Helper;

namespace YTech.IM.Paramita.Web
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                string rpt = Request.QueryString["rpt"];

                rv.ProcessingMode = ProcessingMode.Local;
                rv.LocalReport.ReportPath = Server.MapPath(string.Format("~/Views/Transaction/Report/{0}.rdlc", rpt));

                rv.LocalReport.DataSources.Clear();
                ReportDataSource[] repCol = GetReportData();
                if (repCol != null)
                {
                    foreach (ReportDataSource d in repCol)
                    {
                        rv.LocalReport.DataSources.Add(d);
                    }
                }

                rv.LocalReport.Refresh();
            }
        }

        private ReportDataSource[] GetReportData()
        {
            //if (Session["ReportData"] != null)
            //{
            //    ViewState["ReportData"] = Session["ReportData"];
            //}
            return Session["ReportData"] as ReportDataSource[];
        }
    }
}