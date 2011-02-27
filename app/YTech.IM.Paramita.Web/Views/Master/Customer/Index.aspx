<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#dialog").dialog({
                autoOpen: false
            });

            var editDialog = {
                url: '<%= Url.Action("Update", "Customer") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true

                , onclickSubmit: function (params) {
                    var ajaxData = {};

                    var list = $("#list");
                    var selectedRow = list.getGridParam("selrow");
                    rowData = list.getRowData(selectedRow);
                    ajaxData = { Id: rowData.Id };
                    return ajaxData;
                }
                , afterShowForm: function (eparams) {
                    $('#Id').attr('disabled', 'disabled');
                }
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };
            var insertDialog = {
                url: '<%= Url.Action("Insert", "Customer") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    $('#Id').attr('disabled', '');
                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
            };
            var deleteDialog = {
                url: '<%= Url.Action("Delete", "Customer") %>'
                , modal: true
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };

            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Pembeli Baru";
            $.jgrid.edit.editCaption = "Edit Pembeli";
            $.jgrid.del.caption = "Hapus Pembeli";
            $.jgrid.del.msg = "Anda yakin menghapus Pembeli yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Customer") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Pembeli','Nama','', 'Nama' ,'Alamat', '', '', 'Telp', 'Kota', 'Keterangan'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'PersonFirstName', index: 'PersonFirstName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true, edithidden: true }, hidden: true, formoptions: { elmsuffix: ' *'} },
                    { name: 'PersonLastName', index: 'PersonLastName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false, edithidden: true }, hidden: true },
                    { name: 'PersonName', index: 'PersonName', width: 200, align: 'left', editable: false, edittype: 'text', editrules: { required: false} }, 
                   { name: 'AddressLine1', index: 'AddressLine1', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                   { name: 'AddressLine2', index: 'AddressLine2', width: 200, hidden: true, align: 'left', editable: true, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'AddressLine3', index: 'AddressLine3', width: 200, hidden: true, align: 'left', editable: true, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'AddressPhone', index: 'AddressPhone', width: 200, hidden: true, align: 'left', editable: true, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'AddressCity', index: 'AddressCity', width: 200, hidden: true, align: 'left', editable: true, edittype: 'text', editrules: { required: false, edithidden: true} },
                     { name: 'CustomerDesc', index: 'CustomerDesc', width: 200, hidden: true, sortable: false, align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "20" }, editrules: { required: false, edithidden: true} }
                   ],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Pembeli',
                autowidth: true,
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    $("#list").editGridRow(rowid, editDialog);
                }
            }).navGrid('#listPager',
                {
                    edit: true, add: true, del: true, search: false, refresh: true
                },
                editDialog,
                insertDialog,
                deleteDialog
            );
        });       
    </script>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
</asp:Content>
