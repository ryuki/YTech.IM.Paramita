<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<YTech.IM.Paramita.Web.Controllers.ViewModel.ReportParamViewModel>" %>
<% using (Html.BeginForm())
   { %>
<%= Html.AntiForgeryToken() %>
<%= Html.Hidden("TransStatus", ViewData.Model.TransStatus )%>
<table>
    <tr>
        <td>
            <label for="ExportFormat">
                Format Laporan :</label>
        </td>
        <td>
            <%= Html.DropDownList("ExportFormat")%>
        </td>
    </tr>
    <% if (ViewData.Model.ShowDateFrom)
       {	%>
    <tr>
        <td>
            <label for="DateFrom">
                Tanggal :</label>
        </td>
        <td>
            <%= Html.TextBox("DateFrom", (Model.DateFrom.HasValue) ? Model.DateFrom.Value.ToString("dd-MMM-yyyy") : "")%>
        </td>
    </tr>
    <% } %>
    <% if (ViewData.Model.ShowDateTo)
       {	%>
    <tr>
        <td>
            <label for="DateTo">
                Sampai Tanggal :</label>
        </td>
        <td>
            <%= Html.TextBox("DateTo", (Model.DateTo.HasValue) ? Model.DateTo.Value.ToString("dd-MMM-yyyy") : "")%>
        </td>
    </tr>
    <% } %>
    <% if (ViewData.Model.ShowWarehouse)
       {	%>
    <tr>
        <td>
            <label for="WarehouseId">
                Gudang :</label>
        </td>
        <td>
            <%= Html.DropDownList("WarehouseId", Model.WarehouseList)%>
        </td>
    </tr>
    <% } %>
     <% if (ViewData.Model.ShowItem)
       {	%>
    <tr>
        <td>
            <label for="ItemId">
                Item :</label>
        </td>
        <td>
            <%= Html.DropDownList("ItemId", Model.ItemList)%>
        </td>
    </tr>
    <% } %>
    <% if (ViewData.Model.ShowCostCenter)
       {	%>
    <tr>
        <td>
            <label for="CostCenterId">
                Cost Center :</label>
        </td>
        <td>
            <%= Html.DropDownList("CostCenterId", Model.CostCenterList)%>
        </td>
    </tr>
    <% } %>
    <% if (ViewData.Model.ShowRecPeriod)
       {	%>
    <tr>
        <td>
            <label for="RecPeriodId">
                Periode :</label>
        </td>
        <td>
            <%= Html.DropDownList("RecPeriodId", Model.RecPeriodList)%>
        </td>
    </tr>
    <% } %>
    <tr>
        <td colspan="2" align="center">
            <button id="Save" type="submit" name="Save">
                Download Laporan</button>
        </td>
    </tr>
</table>
<% } %>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $("#Save").button();
        $("#DateFrom").datepicker({ dateFormat: "dd-M-yy" });
        $("#DateTo").datepicker({ dateFormat: "dd-M-yy" });
    });
</script>
