<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<UnitViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <% if (false)
       { %>
    <script src="../../../Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <% } %>
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    Cost Center :
    <%= Html.DropDownList("CostCenterId", Model.CostCenterList)%>
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

              $('#CostCenterId').change(function () {
                $("#list").trigger("reloadGrid");
            });

            var editDialog = {
                url: '<%= Url.Action("Update", "Unit") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , beforeSubmit: function (postdata, formid) { 
                    postdata.CostCenterId = $('#CostCenterId option:selected').val();
                    return [true, ''];
                }
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
                , beforeSubmit: function (postdata, formid) { 
                    postdata.CostCenterId = $('#CostCenterId option:selected').val();
                    return [true, ''];
                }
                , afterShowForm: function (eparams) {
                    $('#Id').removeAttr('disabled');
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
                postData: {
                    CostCenterId: function () { return $('#CostCenterId option:selected').val(); }
                },
                datatype: 'json',
                mtype: 'GET',
                colNames: ['','Kode Unit', 'No', 'Tipe', 'Tipe', 'Luas Tanah', 'Luas Bangunan', 'Lokasi', 'Harga','Status', 'Keterangan'],
                colModel: [
                    { name: 'act', index: 'act', width: 175, sortable: false },
                    { name: 'Id', index: 'Id', width: 100, align: 'left', editable: true, key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: true, editable: true },
                   { name: 'UnitNo', index: 'UnitNo', width: 200, sortable: false, align: 'left', editable: true, edittype: 'text', editrules: { required: true}, formoptions: { elmsuffix: ' *'} },
                    { name: 'UnitTypeId', index: 'UnitTypeId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { required: true,edithidden: true }, hidden: true, formoptions: { elmsuffix: ' *'} }, 
                    { name: 'UnitTypeName', index: 'UnitTypeName', width: 200, align: 'left', editable: false, edittype: 'select', editrules: { edithidden: true} },
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
                 $('#list').setColProp('UnitTypeId', { editoptions: { value: unitTypeIds} });
                 var ids = jQuery("#list").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i]; 
                     var rowId =  $(this).getRowData(ids[i]);
                     var status = rowId['act'];
//                         alert(rowId['act']);
                        var sellButton= "<input type='button' value='Jual' tooltips='Jual Unit' onClick=\"OpenPopup('"+cl+"');\" /> ";
//                        if (status == 'New') {
//                            sellButton += " onClick=\"OpenPopup('"+cl+"');\" />";
//                        }
//                        else {
//                             sellButton += " disabled=disabled />";
//                        }
//       
//                                                alert(be); 
                        var cancelButton = "&nbsp;<input type='button' value='Batal' tooltips='Pembatalan Penjualan Unit' "
                        if (status == 'New') {
                            cancelButton += " disabled=disabled />";
                        }
                        else {
                             cancelButton +=  " onClick=\"DeleteTransUnit('"+cl+"');\" />";
                        }
                        $(this).setRowData(ids[i], { act: sellButton+cancelButton });
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
        function DeleteTransUnit(id)
        {
            if (confirm("Anda yakin membatalkan penjualan unit?")) {
                var url = "<%= Url.Action("DeleteUnitSales", "Unit" ) %>?unitId="+id;
               var response = $.ajax({ url: url, async: false, cache: false, success: function (data, result) { if (!result) alert('Pembatalan Penjualan unit tidak berhasil.'); } }).responseText;
                  $('#dialog p:first').text(response);
                    $("#dialog").dialog("open");
                     $("#list").trigger("reloadGrid");
            }
        }   
            var unitTypeIds = $.ajax({ url: '<%= Url.Action("GetUnitTypeList","Unit") %>?costCenterId='+$('#CostCenterId option:selected').val(), async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the ItemCats.'); } }).responseText;
    </script>
</asp:Content>
