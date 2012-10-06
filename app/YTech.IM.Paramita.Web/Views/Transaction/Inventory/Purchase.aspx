<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<TransactionFormViewModel>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%--<%= Html.Partial("~/Views/Shared/Status.ascx",Model) %>--%>
 
<% Html.EnableClientValidation(); %> 
   <%--<% using (Html.BeginForm())
   { %>--%>
<% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           //OnBegin = "ajaxValidate",
                                           OnSuccess = "onSavedSuccess",
                                           LoadingElementId = "progress"
                                       }

          ))
   {%>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Trans.Id", (ViewData.Model.Trans != null) ? ViewData.Model.Trans.Id : "")%>
    <%= Html.Hidden("Trans.TransStatus", (ViewData.Model.Trans != null) ? ViewData.Model.Trans.TransStatus : "")%>
<%= Html.Hidden("IsGenerateFactur", ViewData.Model.IsGenerateFactur)%>
<%= Html.Hidden("IsAddStock", ViewData.Model.IsAddStock.ToString())%>
<%= Html.Hidden("IsCalculateStock", ViewData.Model.IsCalculateStock.ToString())%>
    <%= Html.Hidden("TransId", (ViewData.Model.TransId))%>
    <%= Html.Hidden("TransStatus", (ViewData.Model.TransStatus))%>
    <div>
        <span id="toolbar" class="ui-widget-header ui-corner-all"><a id="newTrans" href="<%= Url.Action(ViewData.Model.Trans.TransStatus.Equals(EnumTransactionStatus.PurchaseOrder.ToString()) ? "Index" : Model.Trans.TransStatus.ToString(), "Inventory") %>">
            Baru</a>
            <button id="btnSave" name="btnSave" type="submit">
                Simpan</button>
        <button id="btnDelete" name="btnDelete" type="submit">
            Hapus</button>

        <button id="btnList" name="btnList" type="button">
            Edit
            <%= ViewData.Model.Title %></button>
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
                            <label for="TransDate">
                                Tanggal :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("TransDate", (Model.TransDate.HasValue) ? Model.TransDate.Value.ToString("dd-MMM-yyyy") : "")%>
                            <%= Html.ValidationMessage("TransDate")%>
                        </td>
                    </tr>
                    <% } %>
                    <% if (ViewData.Model.ViewFactur)
                       {	%>
                    <tr>
                        <td>
                            <label for="TransFactur">
                                No Faktur :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("TransFactur", Model.TransFactur)%>
                            <%= Html.ValidationMessage("TransFactur")%>
                        </td>
                    </tr>
                    <% } %>
                 <% if (ViewData.Model.ViewPaymentMethod)
                   {	%>
                <tr>
                    <td>
                        <label for="TransPaymentMethod">
                            Cara Pembayaran :</label>
                    </td>
                    <td>
                      <%= Html.DropDownList("TransPaymentMethod", Model.PaymentMethodList)%>
                        <%= Html.ValidationMessage("TransPaymentMethod")%>
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
                            <label for="TransBy">
                                Supplier :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("TransBy", Model.SupplierList)%>
                            <%= Html.ValidationMessage("TransBy")%>
                        </td>
                    </tr>
                    <% } %>
                    <% if (ViewData.Model.ViewWarehouse)
                       {	%>
                    <tr>
                        <td>
                            <label for="WarehouseId">
                                Gudang :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("WarehouseId", Model.WarehouseList)%>
                            <%= Html.ValidationMessage("WarehouseId")%>
                        </td>
                    </tr>
                    <% } %>
                    <% if (ViewData.Model.ViewWarehouseTo)
                       {	%>
                    <tr>
                        <td>
                            <label for="WarehouseIdTo">
                                Ke Gudang :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("WarehouseIdTo", Model.WarehouseToList)%>
                            <%= Html.ValidationMessage("WarehouseIdTo")%>
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
<div id='popup'>
    <iframe width='100%' height='380px' id="popup_frame" frameborder="0"></iframe>
</div>
    <% } %>


    <script type="text/javascript">
    
function onSavedSuccess(e) {
 //$("#Save").attr('disabled', 'disabled');
  var json = e.get_response().get_object();
//alert(json);
    var success = json.Success;
       //alert(json.Success);
        var msg = json.Message;
    if (success == false) {
        //alert(json.Message);
        if (msg) {

            if (msg == "redirect") {
                var urlreport = '<%= ResolveUrl("~/ReportViewer.aspx?rpt=RptPrintFactur") %>';
                   // alert(urlreport);
                window.open(urlreport);
            }
            else {
                $('#dialog p:first').text(msg);
                $("#dialog").dialog("open"); 
            }
            return false ;  
        }
    }
    else{
        $("#btnSave").attr('disabled', 'disabled');
        $("#btnDelete").attr('disabled', 'disabled');
        RemoveAttribute("btnPrint","disabled");  
        $('#dialog p:first').text(msg);
        $("#Trans_TransFactur").val(json.FacturNo);
        $("#dialog").dialog("open"); 
    }
}

        $(function () {
            $("#newTrans").button();
//            $("#Save").button();
            $("#TransDate").datepicker({ dateFormat: "dd-M-yy" });
        });

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
                    //$("#list").trigger("reloadGrid");
                }
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
                    RemoveAttribute("Id", "disabled");
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
                colNames: ['Id', 'No Faktur', 'No Faktur', 'Tanggal', 'Total', 'Keterangan'],
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
                var trans = $.ajax({ url: '<%= Url.Action("GetListTransNotRef", "Inventory") %>?transStatus=<%= EnumTransactionStatus.Received.ToString() %>&warehouseId=' + $('#WarehouseId option:selected').val() + '&transBy=' + $('#TransBy option:selected').val(), async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the items.'); } }).responseText;
                $('#list').setColProp('TransIdRef', { editoptions: { value: trans} });
                //                alert(trans);
            }
            $('#WarehouseId').change(function () {
                //                var acc = $('#Trans_WarehouseId option:selected').val();
                //                                alert(acc);
                $("#list").trigger("reloadGrid");
            });
            $('#TransBy').change(function () {
                $("#list").trigger("reloadGrid");
            });

            $("#btnList").click(function () {
                var urlList = '<%= ResolveUrl("~/Transaction/Inventory/ListTransaction") %>';
                $("#popup_frame").attr("src", urlList + "?src=cc&transStatus=" + $("#TransStatus").val());
                $("#popup").dialog("open");
            });

        });

        function SetTransDetail(src, transId) {
//            alert(src);
//            alert(transId);
            $("#popup").dialog("close");
            //alert("close");
            var trans = $.parseJSON($.ajax({ url: '<%= Url.Action("GetJsonTrans","Inventory") %>?transId=' + transId, async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the trans.'); } }).responseText);
            //alert(trans);
            if (trans) {
                if (trans.TransDate) {
                    var transDate = new Date(parseInt(trans.TransDate.substr(6)));
                    //alert('debug 3');
                    $("#TransDate").val(transDate.format('dd-mmm-yyyy'));
                }

                $("#TransId").val(trans.TransId);
                $("#TransFactur").val(trans.TransFactur);
                $("#WarehouseId").val(trans.WarehouseId);
                $("#TransPaymentMethod").val(trans.TransPaymentMethod);
                $("#TransBy").val(trans.TransBy);
//                $("#Trans_WarehouseIdTo").val(trans.WarehouseIdTo);
//                $("#Trans_UnitTypeId").val(trans.UnitTypeId);
//                $("#Trans_JobTypeId").val(trans.JobTypeId);

                setTimeout("$('#list').trigger('reloadGrid')", 1000);
                RemoveAttribute("btnPrint", "disabled");
                RemoveAttribute("btnDelete", "disabled");
                RemoveAttribute("btnSave", "disabled");
            }
        }
    </script>
</asp:Content>
