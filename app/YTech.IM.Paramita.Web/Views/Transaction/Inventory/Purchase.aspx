<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<YTech.IM.Paramita.Web.Controllers.ViewModel.TransactionFormViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript">

        $(function () {
            $("#newTrans").button();
            $("#Save").button();
            $("#Trans_TransDate").datepicker({ dateFormat: "dd-M-yy" });
        });

        $(document).ready(function () {
            $("#dialog").dialog({
                autoOpen: false
            });

            var editDialog = {
                url: '<%= Url.Action("UpdateTransRef", "Inventory") %>'
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
                url: '<%= Url.Action("InsertTransRef", "Inventory") %>'
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
                , recreateForm: true
            };
            var deleteDialog = {
                url: '<%= Url.Action("DeleteTransRef", "Inventory") %>'
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
            $.jgrid.edit.addCaption = "Tambah Detail Baru";
            $.jgrid.edit.editCaption = "Edit Detail";
            $.jgrid.del.caption = "Hapus Detail";
            $.jgrid.del.msg = "Anda yakin menghapus Detail yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("GetListTransRef", "Inventory") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Id', 'TransIdRef', 'No Faktur', 'Tanggal', 'Total', 'Keterangan'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: true }, edittype: 'select', hidden: true, editable: false },
                    { name: 'TransIdRef', index: 'TransIdRef', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: true }, edittype: 'select', hidden: true, editable: true },
                    { name: 'TransFactur', index: 'TransFactur', width: 200, align: 'left', editable: false, editrules: { edithidden: true} },
                   { name: 'TransDate', index: 'TransDate', width: 200, sortable: false, align: 'left', editable: false, editrules: { edithidden: true} },
                   { name: 'TransSubTotal', index: 'TransSubTotal', width: 200, sortable: false, align: 'right', editable: false, editrules: { required: false, number: true, edithidden: true} },
                   { name: 'TransDesc', index: 'TransDesc', width: 200, sortable: false, align: 'left', editable: false, editrules: { required: false, edithidden: true}}],

                pager: $('#listPager'),
                rowNum: -1,
                //              rowList: [20, 30, 50, 100],
                rownumbers: true,
                //              sortname: 'Id',
                //              sortorder: "asc",
                //              viewrecords: true,
                height: 150,
                caption: 'Daftar Detail',
                autowidth: true,
                loadComplete: function () {
                    GetTransRef();
                    $('#listPager_center').hide();
                },
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    //$("#list").editGridRow(rowid, editDialog);
                }, footerrow: true, userDataOnFooter: true, altRows: true
            }).navGrid('#listPager',
                {
                    edit: false, add: true, del: true, search: false, refresh: true, view: false
                },
                editDialog,
                insertDialog,
                deleteDialog
            );
            function GetTransRef() {
                var trans = $.ajax({ url: '<%= Url.Action("GetListTrans", "Inventory") %>?transStatus=<%= EnumTransactionStatus.Received.ToString() %>&warehouseId=' + $('#Trans_WarehouseId option:selected').val() + '&transBy=' + $('#Trans_TransBy option:selected').val(), async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the items.'); } }).responseText;
                $('#list').setColProp('TransIdRef', { editoptions: { value: trans} });
                //                alert(trans);
            }
            $('#Trans_WarehouseId').change(function () {
                //                var acc = $('#Trans_WarehouseId option:selected').val();
                //                                alert(acc);
                $("#list").trigger("reloadGrid");
            });
            $('#Trans_TransBy').change(function () {
                $("#list").trigger("reloadGrid");
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%= Html.Partial("~/Views/Shared/Status.ascx",Model) %>
    <% using (Html.BeginForm())
       { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Trans.Id", (ViewData.Model.Trans != null) ? ViewData.Model.Trans.Id : "")%>
    <%= Html.Hidden("Trans.TransStatus", (ViewData.Model.Trans != null) ? ViewData.Model.Trans.TransStatus : "")%>
    <div>
        <span id="toolbar" class="ui-widget-header ui-corner-all"><a id="newTrans" href="<%= Url.Action(ViewData.Model.Trans.TransStatus.Equals(EnumTransactionStatus.PurchaseOrder.ToString()) ? "Index" : Model.Trans.TransStatus.ToString(), "Inventory") %>">
            Baru</a>
            <button id="Save" type="submit">
                Simpan</button>
        </span>
    </div>
    <table>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <% if (ViewData.Model.ViewDate)
                       {	%>
                    <tr>
                        <td>
                            <label for="Trans_TransDate">
                                Tanggal :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("Trans.TransDate", (Model.Trans.TransDate.HasValue) ? Model.Trans.TransDate.Value.ToString("dd-MMM-yyyy") : "")%>
                            <%= Html.ValidationMessage("Trans.TransDate")%>
                        </td>
                    </tr>
                    <% } %>
                    <% if (ViewData.Model.ViewFactur)
                       {	%>
                    <tr>
                        <td>
                            <label for="Trans_TransFactur">
                                No Faktur :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("Trans.TransFactur", Model.Trans.TransFactur)%>
                            <%= Html.ValidationMessage("Trans.TransFactur")%>
                        </td>
                    </tr>
                    <% } %>
                 <% if (ViewData.Model.ViewPaymentMethod)
                   {	%>
                <tr>
                    <td>
                        <label for="Trans_TransPaymentMethod">
                            Cara Pembayaran :</label>
                    </td>
                    <td>
                      <%= Html.DropDownList("Trans.TransPaymentMethod", Model.PaymentMethodList)%>
                        <%= Html.ValidationMessage("Trans.TransPaymentMethod")%>
                    </td>
                </tr>
                <% } %>
                </table>
            </td>
            <td>
                <table>
                    <% if (ViewData.Model.ViewSupplier)
                       {	%>
                    <tr>
                        <td>
                            <label for="Trans_TransBy">
                                Supplier :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("Trans.TransBy", Model.SupplierList)%>
                            <%= Html.ValidationMessage("Trans.TransBy")%>
                        </td>
                    </tr>
                    <% } %>
                    <% if (ViewData.Model.ViewWarehouse)
                       {	%>
                    <tr>
                        <td>
                            <label for="Trans_WarehouseId">
                                Gudang :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("Trans.WarehouseId", Model.WarehouseList)%>
                            <%= Html.ValidationMessage("Trans.WarehouseId")%>
                        </td>
                    </tr>
                    <% } %>
                    <% if (ViewData.Model.ViewWarehouseTo)
                       {	%>
                    <tr>
                        <td>
                            <label for="Trans_WarehouseIdTo">
                                Ke Gudang :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("Trans.WarehouseIdTo", Model.WarehouseToList)%>
                            <%= Html.ValidationMessage("Trans.WarehouseIdTo")%>
                        </td>
                    </tr>
                    <% } %>
                </table>
            </td>
        </tr>
    </table>
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
    <% } %>
</asp:Content>
