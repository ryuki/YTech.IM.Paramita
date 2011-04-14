<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
.ui-datepicker-calendar {
    display: none;
    }
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <label for="AccountCatId">
            Cost Center :</label>
        <%= Html.DropDownList("CostCenterId", (SelectList)ViewData["CostCenterList"])%>
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

            var editDialog = {
                url: '<%= Url.Action("Update", "Real") %>'
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
                url: '<%= Url.Action("Insert", "Real") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                 , beforeSubmit: function (postdata, formid) {
                     postdata.CostCenterId = $('#CostCenterId option:selected').val();
                     return [true, ''];
                 }
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
                url: '<%= Url.Action("Delete", "Real") %>'
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
            $.jgrid.edit.addCaption = "Tambah Realisasi Proyek Baru";
            $.jgrid.edit.editCaption = "Edit Realisasi Proyek";
            $.jgrid.del.caption = "Hapus Realisasi Proyek";
            $.jgrid.del.msg = "Anda yakin menghapus Realisasi Proyek yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Real") %>',
                postData: {
                    CostCenterId: function () { return $('#CostCenterId option:selected').val(); }
                },
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Realisasi Proyek', 'Bulan', 'Nilai Realisasi', 'Keterangan'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { edithidden: true }, hidedlg: true, hidden: true, editable: false },
                    { name: 'RealDate', index: 'RealDate', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *' },
                        editoptions: {
                            dataInit: function (elem) {
                                $(elem).datepicker({
                                    changeMonth: true,
                                    changeYear: true,
                                    showButtonPanel: true,
                                    dateFormat: 'M-yy',
                                    onClose: function (dateText, inst) {
                                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                                        $(this).datepicker('setDate', new Date(year, month, 1));
                                    },
                                    beforeShow: function (input, inst) {
                                        if ((datestr = $(this).val()).length > 0) {
                                            year = datestr.substring(datestr.length - 4, datestr.length);
                                            month = jQuery.inArray(datestr.substring(0, datestr.length - 5), $(this).datepicker('option', 'monthNamesShort'));
                                            $(this).datepicker('option', 'defaultDate', new Date(year, month, 1));
                                            $(this).datepicker('setDate', new Date(year, month, 1));
                                        }
                                    }
                                });
                            }
                        }
                    },
                    { name: 'RealPercentValue', index: 'RealPercentValue', width: 200, align: 'right', editable: true, edittype: 'text', editrules: { edithidden: true },
                        editoptions: {
                            dataInit: function (elem) {
                                $(elem).autoNumeric();
                                $(elem).attr("style", "text-align:right;");
                            }
                        }
                    },
                   { name: 'RealDesc', index: 'RealDesc', width: 200, sortable: false, align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "20" }, editrules: { required: false}}],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Realisasi Proyek',
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

            $('#CostCenterId').change(function () {
                $("#list").trigger("reloadGrid");
            });
        });       
    </script>
</asp:Content>
