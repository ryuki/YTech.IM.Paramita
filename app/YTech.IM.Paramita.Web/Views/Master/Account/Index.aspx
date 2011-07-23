<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<MAccount>>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    Kategori Akun :
    <%= Html.DropDownList("AccountCatId", (SelectList)ViewData["AccountCatList"])%>
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
                url: '<%= Url.Action("Update", "Account") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , beforeSubmit: function (postdata, formid) {
                    //                    alert($('#AccountCatId option:selected').val());
                    postdata.AccountCatId = $('#AccountCatId option:selected').val();
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
                    $("#list").trigger("reloadGrid");
                }
                , recreateForm: true
            };
            var insertDialog = {
                url: '<%= Url.Action("Insert", "Account") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    $('#Id').attr('disabled', '');
                }
                , beforeSubmit: function (postdata, formid) {
                    //                    alert($('#AccountCatId option:selected').val());
                    postdata.AccountCatId = $('#AccountCatId option:selected').val();
                    return [true, ''];
                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                    $("#list").trigger("reloadGrid");
                }
                , width: "400"
                , recreateForm : true
            };
            var deleteDialog = {
                url: '<%= Url.Action("Delete", "Account") %>'
                , modal: true
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                    $("#list").trigger("reloadGrid");
                }
                , recreateForm: true
            };

            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Akun Baru";
            $.jgrid.edit.editCaption = "Edit Akun";
            $.jgrid.del.caption = "Hapus Akun";
            $.jgrid.del.msg = "Anda yakin menghapus Akun yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Account") %>',
                postData: {
                    AccountCatId: function () { return $('#AccountCatId option:selected').val(); }
                },
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Akun', 'Kode Akun', 'Nama', 'Induk Akun', 'Keterangan'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, sortable: false, align: 'left', key: true, editrules: { required: true, edithidden: true }, editable: true, hidden: true },
                    { name: 'RecursiveId', index: 'RecursiveId', width: 100, sortable: false, align: 'left', key: true, editrules: { required: true, edithidden: true }, hidedlg: true, hidden: false, editable: false },
                    { name: 'AccountName', index: 'AccountName', width: 200, sortable: false, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'ParentId', index: 'ParentId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { edithidden: true }, hidden: true },
                   { name: 'AccountDesc', index: 'AccountDesc', width: 200, sortable: false, align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "20" }, editrules: { required: false} }

                   ],

                pager: $('#listPager'),
                //set rownum to 0 to show all rows
                rowNum: -1,
                //                rowList: [20, 30, 50, 100],
                //rownumbers: true, 
                //                sortname: 'Id',
                //                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Akun',
                autowidth: true,
                treeGrid: true,
                treeGridModel: 'adjacency',
                ExpandColumn: 'RecursiveId',
                ExpandColClick: true,
                loadComplete: function () {
                    GetParents();
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

            function GetParents() {
                var parents = $.ajax({ url: '<%= Url.Action("GetList", "Account") %>?accountCatId=' + $('#AccountCatId option:selected').val(), async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the parents.'); } }).responseText;
                $('#list').setColProp('ParentId', { editoptions: { value: parents} });
//                alert(parents);
            }

            $('#AccountCatId').change(function () {
                //                var acc = $('#AccountCatId option:selected').val();
                //                alert(acc);
                $("#list").trigger("reloadGrid");
            });
        });

    </script>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
</asp:Content>
