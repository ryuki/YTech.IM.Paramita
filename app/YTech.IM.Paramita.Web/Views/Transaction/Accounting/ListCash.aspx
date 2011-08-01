<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<YTech.IM.Paramita.Web.Controllers.ViewModel.CashFormViewModel>"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <label for="ddlSearchBy">
            Cari berdasar :</label>
        <select id="ddlSearchBy" name="ddlSearchBy">
            <option value="JournalVoucherNo">No Voucher</option>
        </select>
        <input id="txtSearch" type="text" />
        <input id="btnSearch" type="button" value="Cari" />
    </div>
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#dialog").dialog({
                autoOpen: false
            });
            
            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Kas Baru";
            $.jgrid.edit.editCaption = "Edit Kas";
            $.jgrid.del.caption = "Hapus Kas";
            $.jgrid.del.msg = "Anda yakin menghapus Kas yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("ListSearchCash", "Accounting") %>',
                postData: {
                    searchBy: function () { return $('#ddlSearchBy option:selected').val(); },
                    searchText: function () { return $('#txtSearch').val(); },
                    journalType: function () { return '<%= Request.QueryString["journalType"] %>'; }
                },
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Journal', 'No Voucher', 'Tanggal', 'No Bukti', 'Penerima Kas', 'Keterangan'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: true }, hidedlg: true, hidden: true, editable: false },
                    { name: 'JournalVoucherNo', index: 'JournalVoucherNo', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true } },
                    { name: 'JournalDate', index: 'JournalDate', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true } },
                    { name: 'JournalEvidenceNo', index: 'JournalEvidenceNo', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true } },
                    { name: 'JournalPic', index: 'JournalPic', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true } },
                   { name: 'ItemDesc', index: 'ItemDesc', width: 200, sortable: false, align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "20" }, editrules: { required: false}}],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true, 
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 270,
                caption: 'Daftar Kas',
                autowidth: true,
                loadComplete: function () {

                },
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    var list = $("#list");
                    var rowData = list.getRowData(rowid);
                    <% if (!string.IsNullOrEmpty(Request.QueryString["src"])) {	%>
                      window.parent.SetJournalDetail('<%= Request.QueryString["src"] %>',rowData["Id"]);
  <%} else {%>
   window.parent.SetJournalDetail(rowData["Id"]);
  <%}%>
                }
            }).navGrid('#listPager',
                {
                    edit: false, add: false, del: false, search: false, refresh: true
                }
            );

              $('#btnSearch').click(function () {
//              alert($('#ddlSearchBy option:selected').val());
//              alert($('#txtSearch').val());
                $("#list").jqGrid().trigger("reloadGrid");

            });
        });
    </script>
</asp:Content>
