<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<TUnit>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <% if (false) { %>
    <script src="../../../Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
   <% } %>
    <script type="text/javascript">
    
        $(document).ready(function () {
        
            $("#popup").dialog({
                autoOpen: false,
                height: 420,
                width: '80%',
                modal: true,
                close: function(event, ui) {                 
                    $("#list").trigger("reloadGrid");
                 }
            });

            $("#dialog").dialog({
                autoOpen: false
            });

            var editDialog = {
                url: '<%= Url.Action("Update", "Unit") %>'
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
                url: '<%= Url.Action("Insert", "Unit") %>'
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
                url: '<%= Url.Action("Delete", "Unit") %>'
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
            $.jgrid.edit.addCaption = "Tambah Unit Baru";
            $.jgrid.edit.editCaption = "Edit Unit";
            $.jgrid.del.caption = "Hapus Unit";
            $.jgrid.del.msg = "Anda yakin menghapus Unit yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Unit") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['', 'Kode Unit', 'Tipe', 'Luas Tanah', 'Luas Bangunan', 'Lokasi', 'Harga','Status', 'Keterangan'],
                colModel: [
                    { name: 'act', index: 'act', width: 75, sortable: false },
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'UnitType', index: 'UnitType', width: 100, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                   { name: 'UnitLandWide', index: 'UnitLandWide', width: 80, sortable: false, align: 'right', editable: true, edittype: 'text', editrules: { required: false },
                       editoptions: {
                           dataInit: function (elem) {
                               $(elem).autoNumeric({ mDec: 0 });
                               $(elem).attr("style","text-align:right;");
                           }
                       }
                   },
                   { name: 'UnitWide', index: 'UnitWide', width: 80, sortable: false, align: 'right', editable: true, edittype: 'text', editrules: { required: false },
                       editoptions: {
                           dataInit: function (elem) {
                               $(elem).autoNumeric({ mDec: 0 });
                               $(elem).attr("style", "text-align:right;");
                           }
                       }
                   },
                   { name: 'UnitLocation', index: 'UnitLocation', width: 200, sortable: false, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                   { name: 'UnitPrice', index: 'UnitPrice', width: 100, sortable: false, align: 'right', editable: true, edittype: 'text', editrules: { required: false },
                       editoptions: {
                           dataInit: function (elem) {
                               $(elem).autoNumeric();
                               $(elem).attr("style", "text-align:right;");
                           }
                       }
                   },
                                   { name: 'UnitStatus', index: 'UnitStatus', width: 200, sortable: false, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                   {name: 'UnitDesc', index: 'UnitDesc', width: 200, sortable: false, align: 'left', editable: true, edittype: 'text', editrules: { required: false}}],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Unit',
                autowidth: true,
                loadComplete: function () {
                 var ids = jQuery("#list").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i]; 
                     var rowId =  $(this).getRowData(ids[i]);
                     var status = rowId['act'];
//                         alert(rowId['act']);
                        var be= "<input type='button' value='Jual' tooltips='Jual Unit' ";
                        if (status == 'New') {
    be += " onClick=\"OpenPopup('"+cl+"');\" />";
}
else {
     be += " disabled=disabled />";
}
//       
//                                                alert(be); 
                        $(this).setRowData(ids[i], { act: be });
                        }
                },
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
        function OpenPopup(id)
        {
        var url = "<%= Url.Action("UnitSales", "Unit" ) %>?unitId="+id;
            $("#popup_frame").attr("src", url);
            $("#popup").dialog("open");
            return false;   
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
        <p>
        </p>
    </div>
    <div id='popup'>
        <iframe width='100%' height='340px' id="popup_frame"></iframe>
    </div>
</asp:Content>
