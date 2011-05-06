<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master"
    AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<UnitSalesFormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $('#Save').button();
            $("#TransUnitDate").datepicker({ dateFormat: "dd-M-yy" });
            $('#TransUnitPrice').autoNumeric();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%= Html.Partial("~/Views/Shared/Status.ascx",Model) %>
    <% using (Html.BeginForm())
       {%>
    <%=Html.AntiForgeryToken()%>
    <%= Html.Hidden("Id", (ViewData.Model.TransUnit != null) ? ViewData.Model.TransUnit.Id : "")%>
    <div>
        <span id="toolbar" class="ui-widget-header ui-corner-all">
            <button id="Save" type="submit">
                Simpan</button>
        </span>
    </div>
    <table>
        <%-- <tr>
            <td >
             <label for="TransUnit_CostCenterId">
                            Cara Pembayaran :</label> 
                      </td>
            <td>
                        <%= Html.DropDownList("TransUnit.CostCenterId", Model.CostCenterList)%>
                        <%= Html.ValidationMessage("TransUnit.CostCenterId")%>
                    </td>
        </tr>--%>
        <tr>
            <td>
                <label for="TransUnitDate">
                    Tanggal Jual :</label>
            </td>
            <td>
                <%= Html.TextBox("TransUnitDate", (Model.TransUnit.TransUnitDate.HasValue) ? Model.TransUnit.TransUnitDate.Value.ToString(CommonHelper.DateFormat) : "")%>
                <%= Html.ValidationMessage("TransUnitDate")%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="TransUnitFactur">
                    No Faktur :</label>
            </td>
            <td>
                <%= Html.TextBox("TransUnitFactur", Model.TransUnit.TransUnitFactur)%>
                <%= Html.ValidationMessage("TransFactur")%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="CustomerId">
                    Pembeli :</label>
            </td>
            <td>
                <%= Html.DropDownList("CustomerId", Model.CustomerList)%>
                <%= Html.ValidationMessage("CustomerId")%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="TransUnitPrice">
                    Harga :</label>
            </td>
            <td>
                <%= Html.TextBox("TransUnitPrice", (Model.TransUnit.TransUnitPrice.HasValue) ? Model.TransUnit.TransUnitPrice.Value.ToString(CommonHelper.NumberFormat) : "")%>
                <%= Html.ValidationMessage("TransUnitPrice")%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="TransUnitPaymentMethod">
                    Cara Pembayaran :</label>
            </td>
            <td>
                <%= Html.DropDownList("TransUnitPaymentMethod", Model.PaymentMethodList)%>
                <%= Html.ValidationMessage("TransUnitPaymentMethod")%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="TransUnitDesc">
                    Keterangan :</label>
            </td>
            <td>
                <%= Html.TextArea("TransUnitDesc", Model.TransUnit.TransUnitDesc)%>
                <%= Html.ValidationMessage("TransUnitDesc")%>
            </td>
        </tr>
    </table>
    <%
        }%>
</asp:Content>
