<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<MJobType>>" %>

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
                close: function (event, ui) {

                }
            });

            var editDialog = {
                url: '<%= Url.Action("Update", "JobType") %>'
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
                    $('#imgAccountId').click(function () {
                        OpenPopupAccountSearch();
                    });
                }
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };
            var insertDialog = {
                url: '<%= Url.Action("Insert", "JobType") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    RemoveAttribute("Id","disabled"); 
                    $('#imgAccountId').click(function () {
                        OpenPopupAccountSearch();
                    });
                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
            };
            var deleteDialog = {
                url: '<%= Url.Action("Delete", "JobType") %>'
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
            $.jgrid.edit.addCaption = "Tambah Jenis Pekerjaan Baru";
            $.jgrid.edit.editCaption = "Edit Jenis Pekerjaan";
            $.jgrid.del.caption = "Hapus Jenis Pekerjaan";
            $.jgrid.del.msg = "Anda yakin menghapus Jenis Pekerjaan yang dipilih?";
            var imgLov = '<%= Url.Content("~/Content/Images/window16.gif") %>';
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "JobType") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Jenis Pekerjaan', 'Nama Jenis Pekerjaan', 'Status', 'Akun Biaya', 'Nama Akun', 'Keterangan'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'JobTypeName', index: 'JobTypeName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'JobTypeStatus', index: 'JobTypeStatus', width: 50, sortable: false, align: 'left', editable: true, edittype: 'checkbox', editoptions: { value: "Aktif:Tidak Aktif" }, editrules: { required: false} },
                     { name: 'AccountId', index: 'AccountId', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false },
                         formoptions: {
                             "elmsuffix": "&nbsp;<img src='" + imgLov + "' style='cursor:hand;' id='imgAccountId' />"
                         }
                     },
                   { name: 'AccountName', index: 'AccountName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                    { name: 'JobTypeDesc', index: 'JobTypeDesc', width: 250, hidden: true, sortable: false, align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "20" }, editrules: { required: false, edithidden: true} }
                   ],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Jenis Pekerjaan',
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

        function OpenPopupAccountSearch() { 
            var popup_frame = $("#popup_frame");
            var new_url = '<%= Url.Content("~/Master/Account/Search") %>';
            if (popup_frame.attr("src") != new_url) {
                popup_frame.attr("src", new_url);
            }
            $("#popup").dialog("open");
            return false;
        }

        function SetAccountDetail(accountId, accountName) {
            $("#popup").dialog("close");
            $('#AccountId').val(accountId);
            $('#AccountName').val(accountName);
        }           
    </script>
</asp:Content>
