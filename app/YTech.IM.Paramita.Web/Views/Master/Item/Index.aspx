<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<MItem>>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
 <div>
        <label for="ddlSearchBy">
            Cari berdasar :</label>
        <select id="ddlSearchBy">
            <option value="0">Kode Produk</option>
            <option value="1">Nama</option>
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
    <script type="text/javascript">

            var itemCats = $.ajax({ url: '<%= Url.Action("GetList","MItemCat") %>', async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the ItemCats.'); } }).responseText;
            var brands = $.ajax({ url: '<%= Url.Action("GetList","Brand") %>', async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Brands.'); } }).responseText;
        $(document).ready(function () {
            $("#dialog").dialog({
                autoOpen: false
            });


            var editDialog = {
                url: '<%= Url.Action("Update", "Item") %>'
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
                url: '<%= Url.Action("Insert", "Item") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    RemoveAttribute("Id","disabled"); 

                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
            };
            var deleteDialog = {
                url: '<%= Url.Action("Delete", "Item") %>'
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
            $.jgrid.edit.addCaption = "Tambah Produk Baru";
            $.jgrid.edit.editCaption = "Edit Produk";
            $.jgrid.del.caption = "Hapus Produk";
            $.jgrid.del.msg = "Anda yakin menghapus Produk yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Item") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Produk', 'Nama', 'Kategori Produk', 'Kategori Produk', 'Merek', 'Merek', 'Satuan', 'Satuan', 'Harga', 'Keterangan'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'ItemName', index: 'ItemName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'ItemCatId', index: 'ItemCatId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { edithidden: true },hidden:true  },
                    { name: 'ItemCatName', index: 'ItemCatName', width: 200, align: 'left', editable: false, edittype: 'select', editrules: { edithidden: true } },
                    { name: 'BrandId', index: 'BrandId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { edithidden: true }, hidden: true },
                    { name: 'BrandName', index: 'BrandName', width: 200, align: 'left', editable: false, edittype: 'select', editrules: { edithidden: true} },
                    { name: 'ItemUomId', index: 'ItemUomId', width: 200, align: 'left', editable: false, editrules: { edithidden: true }, hidden: true },
                    { name: 'ItemUomName', index: 'ItemUomName', width: 200, editable: true, editrules: { edithidden: true} },
                    { name: 'ItemUomPurchasePrice', index: 'ItemUomPurchasePrice', width: 200, editable: true, editrules: { edithidden: true },
                        editoptions: {
                            dataInit: function (elem) {
                                $(elem).autoNumeric();
                                $(elem).attr("style", "text-align:right;");
                            }
                        }
                    },
                   { name: 'ItemDesc', index: 'ItemDesc', width: 200, sortable: false, align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "20" }, editrules: { required: false}}],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true, 
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Produk',
                autowidth: true,
                loadComplete: function () {
                    $('#list').setColProp('ItemCatId', { editoptions: { value: itemCats} });
					//alert(itemCats);
                    $('#list').setColProp('BrandId', { editoptions: { value: brands} });
                },
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    $('#list').editGridRow(rowid, editDialog);
                }
            }).navGrid('#listPager',
                {
                    edit: true, add: true, del: true, search: false, refresh: true
                },
                editDialog,
                insertDialog,
                deleteDialog
            )
            .navButtonAdd('#listPager', {
                caption: "Export ke Excel",
                buttonicon: "ui-icon-save",
                onClickButton: function () {
                    exportExcel($(this));
                },
                position: "last"
            });
        });

            $('#btnSearch').click(function () {
                var newurl = '<%= Url.Action("ListSearch", "Item") %>';
                var searchby = $("#ddlSearchBy option:selected").val();
                if (searchby == "0") {
                    newurl += '?itemId=';
                }
                else if (searchby == "1") {
                    newurl += '?itemName=';
                }
                newurl += $("#txtSearch").val();
                //                alert(newurl);
                $("#list").jqGrid().setGridParam({ url: newurl }).trigger("reloadGrid");

            });


//            alert(brands.toString());
    </script>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
</asp:Content>
