<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<MBrand>>" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {

            $("#dialog").dialog({
                autoOpen: false
            });


            var editDialog = {
                url: '<%= Url.Action("Update", "Brand") %>'
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
                url: '<%= Url.Action("Insert", "Brand") %>'
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
                url: '<%= Url.Action("Delete", "Brand") %>'
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
            $.jgrid.edit.addCaption = "Tambah Merek Baru";
            $.jgrid.edit.editCaption = "Edit Merek";
            $.jgrid.del.caption = "Hapus Merek";
            $.jgrid.del.msg = "Anda yakin menghapus Merek yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Brand") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Merek', 'Nama', 'Keterangan'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'BrandName', index: 'BrandName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                   { name: 'BrandDesc', index: 'BrandDesc', width: 200, sortable: false, align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "20" }, editrules: { required: false }, formoptions: { elmsuffix: ' *'}}],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true, 
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Merek',
                autowidth: true,
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    $("#list").editGridRow(rowid, editDialog);
                }
            }).navGrid('#listPager',
                {
                    edit: true, add: true, del: true, search: false, refresh: true,excel:true
                },
                editDialog,
                insertDialog,
                deleteDialog
            )
            .navButtonAdd('#listPager', {
                caption: "Export to Excel",
                buttonicon: "ui-icon-save",
                onClickButton: function () {
                    exportExcel($(this));
                },
                position: "last"
            });
        });

        function exportExcel(grid) {
            //alert('export to excel');
            var mya = new Array();
            mya = grid.getDataIDs();  // Get All IDs
            var data = grid.getRowData(mya[0]);     // Get First row to get the labels
            var columnNames = grid.jqGrid('getGridParam', 'colNames');
//            for (i = 0; i < columnNames.length; i++) {
//                alert(columnNames[i]);
//            }
            var colNames = new Array();
            var ii = 0;
            var j = 1;
            var html = "";
            for (var i in data) {
                //alert(columnNames[j]);
                html = html + columnNames[j] + ",";
                colNames[ii++] = i;
                j++;
            }    // capture col names
            html = html + "\n";   // Output header with end of line
            for (i = 0; i < mya.length; i++) {
                data = grid.getRowData(mya[i]); // get each row
                for (j = 0; j < colNames.length; j++) {
                    html = html + data[colNames[j]] + ","; // output each column as tab delimited
                }
                html = html + "\n";  // output each row with end of line

            }
            html = html + "\n";  // end of line at the end
            //alert(html);
            document.forms[0].csvBuffer.value = html;
            document.forms[0].method = 'POST';
            document.forms[0].action = '<%= Url.Action("Export", "Brand") %>';  // send it to server which will open this contents in excel file
            document.forms[0].target = '_blank';
            document.forms[0].submit();
        }      
    </script>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <div id="dialog" title="Status">
        <p></p>
    </div>

    <form method="post">
    <input type="hidden" name="csvBuffer" id="csvBuffer" value="" />
</form>
</asp:Content>
