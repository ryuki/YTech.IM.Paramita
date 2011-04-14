<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<MAccount>>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
 <div>  
    <label for="AccountCatId">Kategori Akun :</label>   
    <%= Html.DropDownList("AccountCatId", (SelectList)ViewData["AccountCatList"])%>
     </div>
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

                //                rowNum: 20,
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
                    var list = $("#list");
                    var rowData = list.getRowData(rowid);
                     <% if (!string.IsNullOrEmpty(Request.QueryString["src"])) {	%>
                      window.parent.SetAccountDetail('<%= Request.QueryString["src"] %>',rowData["Id"], rowData["AccountName"]);
  <%} else {%>
   window.parent.SetAccountDetail(rowData["Id"], rowData["AccountName"]);
  <%}%>
                   
                    return false;
                }
            }).navGrid('#listPager',
                {
                    edit: false, add: false, del: false, search: false, refresh: true
                }
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
