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
        <a href="#">Pembukuan</a></h3>
    <div class="child-menu-container">
        
            <%= Html.ActionLinkForAreas<AccountingController>(c => c.GeneralLedger(), "General Ledger")%>
        
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
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptBrand), "Daftar Merek")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptCostCenter), "Daftar Cost Center")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptItem), "Daftar Produk")%>
        
            <%--<hr />--%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptStockCard), "Kartu Stok")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptStockItem), "Laporan Stok Per Gudang")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptAnalyzeBudgetDetail), "Laporan Analisa Budget")%>
        
            <%--<hr />--%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.PurchaseOrder), "Lap. Detail Order Pembelian")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.Received), "Lap. Detail Penerimaan Stok")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.ReturPurchase), "Lap. Detail Retur Pembelian")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.Using), "Lap. Detail Pemakaian")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.Mutation), "Lap. Detail Mutasi Stok")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.ReportTrans(EnumReports.RptTransDetail, EnumTransactionStatus.Adjusment), "Lap. Detail Penyesuaian")%>
        
            <%--<hr />--%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptJournal), "Lap. Jurnal")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptBukuBesar), "Lap. Buku Besar")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptNeraca), "Lap. Neraca")%>
        
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptLR), "Lap. Laba / Rugi")%>
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
