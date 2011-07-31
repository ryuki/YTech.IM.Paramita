<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

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
    <div id='popup'>
        <iframe width='100%' height='380px' id="popup_frame" frameborder="0"></iframe>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            $("#dialog").dialog({
                autoOpen: false
            });
            $("#popup").dialog({
                autoOpen: false,
                height: 420,
                width: '80%',
                modal: true,
                close: function(event, ui) {                 
                    $("#list").trigger("reloadGrid");
                 }
            });


            var editDialog = {
                url: '<%= Url.Action("Update", "CostCenter") %>'
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
//                    $('#CostCenterStartDate').datepicker({ dateFormat: "dd-M-yy" });
//                    $('#CostCenterEndDate').datepicker({ dateFormat: "dd-M-yy" }); 
                }
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };
            var insertDialog = {
                url: '<%= Url.Action("Insert", "CostCenter") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    $('#Id').removeAttr('disabled');
//                    $('#CostCenterStartDate').datepicker({ dateFormat: "dd-M-yy" });
//                    $('#CostCenterEndDate').datepicker({ dateFormat: "dd-M-yy" }); 

                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
            };
            var deleteDialog = {
                url: '<%= Url.Action("Delete", "CostCenter") %>'
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
            $.jgrid.edit.addCaption = "Tambah Cost Center Baru";
            $.jgrid.edit.editCaption = "Edit Cost Center";
            $.jgrid.del.caption = "Hapus Cost Center";
            $.jgrid.del.msg = "Anda yakin menghapus Cost Center yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "CostCenter") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['', 'Kode Cost Center', 'Nama', 'Total Budget', 'Status', 'Tgl Mulai', 'Tgl Selesai', 'Keterangan'],
                colModel: [
                    { name: 'act', index: 'act', width: 75, sortable: false },
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'CostCenterName', index: 'CostCenterName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                       { name: 'CostCenterTotalBudget', index: 'CostCenterTotalBudget', width: 200, sortable: false, align: 'right', editable: true, editrules: { required: false },
                           editoptions: {
                               dataInit: function (elem) {
                                   $(elem).autoNumeric();
                                   $(elem).attr("style", "text-align:right;");
                               }
                           }
                       },
                   { name: 'CostCenterStatus', index: 'CostCenterStatus', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                       { name: 'CostCenterStartDate', index: 'CostCenterStartDate', width: 200, sortable: false, align: 'left', editable: true, editrules: { required: false },
                           editoptions: {
                               dataInit: function (elem) {
                                   $(elem).datepicker({ dateFormat: "dd-M-yy" }); 
                               }
                           }
                       },
                       { name: 'CostCenterEndDate', index: 'CostCenterEndDate', width: 200, sortable: false, align: 'left', editable: true, editrules: { required: false },
                           editoptions: {
                               dataInit: function (elem) {
                                   $(elem).datepicker({ dateFormat: "dd-M-yy" });
                               }
                           }
                       },
                   { name: 'CostCenterDesc', index: 'CostCenterDesc', width: 200, sortable: false, align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "20" }, editrules: { required: false }}],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true, 
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Cost Center',
                autowidth: true,
                loadComplete: function() {
                    var ids = jQuery("#list").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var be = "<input type='button' value='T' tooltips='Tambah Type Unit' onClick=\"OpenPopup('" + cl + "');\" />";
                        $(this).setRowData(ids[i], { act: be });
                    }
                },
                subGrid: true,
                subGridUrl: '<%= Url.Action("ListForSubGrid", "UnitType") %>',
                subGridModel: [{ name: ['Nama', 'Total', 'Keterangan'],
                                 width: [80, 80, 80],
                                 align: ['right', 'left', 'left'],
                                 params: ['Id']
                }],
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

         function OpenPopup(id) {
             $("#popup_frame").attr("src", "<%= Url.Action("AddUnitType", "UnitType") %>?costCenterId="+id+"&rand="+(new Date()).getTime());
             $("#popup").dialog("open");
             return false;
         }       
    </script>
</asp:Content>
