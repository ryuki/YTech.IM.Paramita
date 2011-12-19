<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="accordion">
    <h3>
        <a href="#">Home</a></h3>
    <div class="child-menu-container">
        <%=Html.ActionLinkForAreas<HomeController>(c => c.Index(), "Home") %>
    </div>
    <% if (Request.IsAuthenticated)
       {
    %>
    <h3>
        <a href="#">Data Pokok</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<WarehouseController>(c => c.Index(),"Master Gudang") %>
        <%= Html.ActionLinkForAreas<JobTypeController>(c => c.Index(), "Master Jenis Pekerjaan")%>
        <%= Html.ActionLinkForAreas<MItemCatController>(c => c.Index(),"Master Kategori Produk") %>
        <%= Html.ActionLinkForAreas<BrandController>(c => c.Index(),"Master Merek") %>
        <%= Html.ActionLinkForAreas<ItemController>(c => c.Index(), "Master Produk")%>
        <%-- <hr />--%>
        <%= Html.ActionLinkForAreas<SupplierController>(c => c.Index(),"Master Supplier") %>
        <%--<hr />--%>
        <%= Html.ActionLinkForAreas<DepartmentController>(c => c.Index(),"Master Departemen") %>
        <%= Html.ActionLinkForAreas<EmployeeController>(c => c.Index(), "Master Karyawan")%>
        <%--<hr />--%>
        <%= Html.ActionLinkForAreas<CostCenterController>(c => c.Index(),"Master Cost Center") %>
        <%= Html.ActionLinkForAreas<AccountController>(c => c.Index(),"Master Akun") %>
    </div>
    <h3>
        <a href="#">Inventori</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Index(), "Order Pembelian")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Received(), "Penerimaan Stok")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Purchase(), "Pembelian")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.ReturPurchase(), "Retur Pembelian")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Using(), "Pemakaian Material")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Mutation(), "Mutasi Stok")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Adjusment(), "Penyesuaian Stok")%>
        <%--<hr />--%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Budgeting(), "Rencana Anggaran Belanja")%>
        <%--<hr />--%>
        <%= Html.ActionLinkForAreas<RealController>(c => c.Index(), "Realisasi Proyek")%>
    </div>
    <h3>
        <a href="#">Penjualan Unit</a></h3>
    <div class="child-menu-container">
        <%=Html.ActionLinkForAreas<UnitController>(c => c.Index(), "Daftar Unit") %>
        <%=Html.ActionLinkForAreas<CustomerController>(c => c.Index(), "Daftar Pembeli") %>
    </div>
    <h3>
        <a href="#">Hutang</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<PaymentController>(c => c.Index(EnumPaymentType.Hutang), "Pembayaran Hutang")%>
    </div>
    <h3>
        <a href="#">Pembukuan</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<AccountingController>(c => c.GeneralLedger(), "Jurnal Umum")%>
        <%= Html.ActionLinkForAreas<AccountingController>(c => c.CashIn(), "Kas Masuk")%>
        <%= Html.ActionLinkForAreas<AccountingController>(c => c.CashOut(), "Kas Keluar")%>
        <%--   
            <hr />
        </div>
        
            <%=Html.ActionLinkForAreas<HomeController>(c => c.Index(), "Pelunasan Hutang") %>
        </div>
       Mutasi Kas</div>
                            Kasbon</div>
                            <hr /></div>
                            Pembayaran Hutang</div>
                            Pembayaran Gaji</div>--%>
    </div>
    <h3>
        <a href="#">Laporan</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptBrand), CommonHelper.GetStringValue(EnumReports.RptBrand))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptCostCenter), CommonHelper.GetStringValue(EnumReports.RptCostCenter))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptItem), CommonHelper.GetStringValue(EnumReports.RptItem))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptStockCard), CommonHelper.GetStringValue(EnumReports.RptStockCard))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptStockItem), CommonHelper.GetStringValue(EnumReports.RptStockItem))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptAnalyzeBudgetDetail), CommonHelper.GetStringValue(EnumReports.RptAnalyzeBudgetDetail))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.PurchaseOrder,null), string.Format(CommonHelper.GetStringValue(EnumReports.RptTransDetail), CommonHelper.GetStringValue(EnumTransactionStatus.PurchaseOrder)))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.Received, null), string.Format(CommonHelper.GetStringValue(EnumReports.RptTransDetail), CommonHelper.GetStringValue(EnumTransactionStatus.Received)))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.ReturPurchase, null), string.Format(CommonHelper.GetStringValue(EnumReports.RptTransDetail), CommonHelper.GetStringValue(EnumTransactionStatus.ReturPurchase)))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.Using, null), string.Format(CommonHelper.GetStringValue(EnumReports.RptTransDetail), CommonHelper.GetStringValue(EnumTransactionStatus.Using)))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.Mutation, null), string.Format(CommonHelper.GetStringValue(EnumReports.RptTransDetail), CommonHelper.GetStringValue(EnumTransactionStatus.Mutation)))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.Adjusment, null), string.Format(CommonHelper.GetStringValue(EnumReports.RptTransDetail), CommonHelper.GetStringValue(EnumTransactionStatus.Adjusment)))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.Budgeting, null), string.Format(CommonHelper.GetStringValue(EnumReports.RptTransDetail), CommonHelper.GetStringValue(EnumTransactionStatus.Budgeting)))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransRecap, EnumTransactionStatus.Received, EnumReportGroupBy.Supplier), string.Format(CommonHelper.GetStringValue(EnumReports.RptTransRecap), CommonHelper.GetStringValue(EnumTransactionStatus.Received), CommonHelper.GetStringValue(EnumReportGroupBy.Supplier)))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransRecap, EnumTransactionStatus.Budgeting, EnumReportGroupBy.UnitType), string.Format(CommonHelper.GetStringValue(EnumReports.RptTransRecap), CommonHelper.GetStringValue(EnumTransactionStatus.Budgeting), CommonHelper.GetStringValue(EnumReportGroupBy.UnitType)))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptJournal), CommonHelper.GetStringValue(EnumReports.RptJournal))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptBukuBesar), CommonHelper.GetStringValue(EnumReports.RptBukuBesar))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptJournalByCostCenter), CommonHelper.GetStringValue(EnumReports.RptJournalByCostCenter))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptBukuBesarByCostCenter), CommonHelper.GetStringValue(EnumReports.RptBukuBesarByCostCenter))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptNeraca), CommonHelper.GetStringValue(EnumReports.RptNeraca))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptLR), CommonHelper.GetStringValue(EnumReports.RptLR))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptNeracaSum), CommonHelper.GetStringValue(EnumReports.RptNeracaSum))%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptLRSum), CommonHelper.GetStringValue(EnumReports.RptLRSum))%>
    </div>
    <h3>
        <a href="#">Utiliti</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<UserAdministrationController>(c => c.Index(null), "Daftar Pengguna")%>
        <%-- Ganti Password</div>
        
            Backup Database</div>--%>
        <%= Html.ActionLinkForAreas<AccountingController>(c => c.Closing(), "Tutup Buku")%>
        <%= Html.ActionLinkForAreas<AccountingController>(c => c.Opening(), "Buka Buku")%>
    </div>
    <%
       }
    %>
</div>
