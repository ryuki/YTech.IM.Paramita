<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //SETTING UP OUR POPUP
        //0 means disabled; 1 means enabled;
        var popupStatus = 0;

        //loading popup with jQuery magic!
        function loadPopup() {
            //loads popup only if it is disabled
            if (popupStatus == 0) {
                $("#backgroundPopup").css({
                    "opacity": "0.7"
                });
                $("#backgroundPopup").fadeIn("slow");
                $("#popupContact").fadeIn("slow");
                popupStatus = 1;
            }
        }

        //disabling popup with jQuery magic!
        function disablePopup() {
            //disables popup only if it is enabled
            if (popupStatus == 1) {
                $("#backgroundPopup").fadeOut("slow");
                $("#popupContact").fadeOut("slow");
                popupStatus = 0;
            }
        }

        //centering popup
        function centerPopup() {
            //request data for centering
            var windowWidth = document.documentElement.clientWidth;
            var windowHeight = document.documentElement.clientHeight;
            var popupHeight = $("#popupContact").height();
            var popupWidth = $("#popupContact").width();
            //centering
            $("#popupContact").css({
                "position": "absolute",
                "top": windowHeight / 2 - popupHeight / 2,
                "left": windowWidth / 2 - popupWidth / 2
            });
            //only need force for IE6

            $("#backgroundPopup").css({
                "height": windowHeight
            });

        }

        $(document).ready(function () {
            //following code will be here
            //LOADING POPUP
            //Click the button event!
            $("#button").click(function () {
                //centering with css
                centerPopup();
                //load popup
                loadPopup();
            });
            //CLOSING POPUP
            //Click the x event!
            $("#popupContactClose").click(function () {
                disablePopup();
            });
            //Click out event!
            $("#backgroundPopup").click(function () {
                disablePopup();
            });
            //Press Escape event!
            $(document).keypress(function (e) {
                if (e.keyCode == 27 && popupStatus == 1) {
                    disablePopup();
                }
            });

        });

        var costCenterId = '<%= Request.QueryString["costCenterId"] %>';

        $(document).ready(function () {
            $("#dialog").dialog({
                autoOpen: false
            });

            $("#popup").dialog({
                autoOpen: false,
                height: 420,
                width: '80%',
                modal: true,
                close: function (event, ui) {
                    $("#list").trigger("reloadGrid");
                }
            });

            var editDialog = {
                url: '<%= Url.Action("Update", "UnitType") %>'
                , beforeSubmit: function (postdata, formid) {
                    postdata.CostCenterId = costCenterId;
                    return [true, ''];
                }
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
                url: '<%= Url.Action("Insert", "UnitType") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , beforeSubmit: function (postdata, formid) {
                    postdata.CostCenterId = costCenterId;
                        return [true, ''];
                }
                , afterShowForm: function (eparams) {
                    RemoveAttribute("Id","disabled"); 
                    //    $('#imgItemId').click(function () {
                    //        OpenPopupItemSearch();
                    //    });
                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
            };
            var deleteDialog = {
                url: '<%= Url.Action("Delete", "UnitType") %>'
                , modal: true
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                    $("#list").trigger("reloadGrid");
                }
            };

            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Tipe Unit Baru";
            $.jgrid.edit.editCaption = "Edit Tipe Unit";
            $.jgrid.del.caption = "Hapus Tipe Unit";
            $.jgrid.del.msg = "Anda yakin menghapus Tipe Unit yang dipilih?";

            var imgLov = '<%= Url.Content("~/Content/Images/window16.gif") %>';

            $("#list").jqGrid({
                url: '<%= Url.Action("List", "UnitType") %>',
                postData: {
                    CostCenterId: function () { return costCenterId; }
                },
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Type Unit', 'Nama', 'Total', 'Keterangan'],
                colModel: [
                    { name: 'UnitTypeId', index: 'UnitTypeId', width: 100, sortable: false, align: 'left', key: true, editrules: { required: false, edithidden: true }, editable: false, hidden: true },
                    { name: 'UnitTypeName', index: 'UnitTypeName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { edithidden: true} },
                    { name: 'UnitTypeTotal', index: 'UnitTypeTotal', width: 200, align: 'right', editable: true, edittype: 'text', editrules: { edithidden: true }, hidden: false,
                        editoptions: {
                            dataInit: function (elem) {
                                $(elem).autoNumeric({
                                    mDec: 0
                                });
                                $(elem).attr("style", "text-align:right;");
                            }
                        }
                    },
                    { name: 'UnitTypeDesc', index: 'UnitTypeDesc', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { edithidden: true }, hidden: false }
                ],
                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 250,
                caption: 'Daftar Tipe Unit',
                autowidth: true,
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
            );
        });
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
    <div id='popup'>
        <iframe width='100%' height='380px' id="popup_frame"></iframe>
    </div>
</asp:Content>