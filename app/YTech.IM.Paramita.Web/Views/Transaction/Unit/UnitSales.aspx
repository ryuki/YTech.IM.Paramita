<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master"
    AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<UnitSalesFormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript">

      $(document).ready(function () {
          $('#Save').button();
          $("#TransUnit_TransUnitDate").datepicker({ dateFormat: "dd-M-yy" });
          $('#TransUnit_TransUnitPrice').autoNumeric();
      });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupContent" runat="server">
    <%= Html.Partial("~/Views/Shared/Status.ascx",Model) %>
    <% using (Html.BeginForm())
       {%>
    <%=Html.AntiForgeryToken()%>
<%= Html.Hidden("TransUnit.Id", (ViewData.Model.TransUnit.Id != null) ? ViewData.Model.TransUnit.Id : "")%>
    <div>
        <span id="toolbar" class="ui-widget-header ui-corner-all">
            <button id="Save" type="submit">
                Simpan</button>
        </span>
    </div>
    <table>
        <tr>
            <td >
             <label for="TransUnit_TransUnitDate">
                            Tanggal Jual :</label> 
                      </td>
            <td>
               <%= Html.TextBox("TransUnit.TransUnitDate", (Model.TransUnit.TransUnitDate.HasValue) ? Model.TransUnit.TransUnitDate.Value.ToString(CommonHelper.DateFormat) : "")%> 
                        <%= Html.ValidationMessage("TransUnit.TransUnitDate")%>
                    </td>
        </tr>
         <tr>
                    <td>
                        <label for="TransUnit_TransUnitFactur">
                            No Faktur :</label>
                    </td>
                    <td>
                        <%= Html.TextBox("TransUnit.TransUnitFactur", Model.TransUnit.TransUnitFactur)%>
                        <%= Html.ValidationMessage("TransUnit.TransFactur")%>
                    </td>
                </tr>
        <tr>
            <td >
             <label for="TransUnit_CustomerId">
                            Pembeli :</label> 
                      </td>
            <td>
                        <%= Html.DropDownList("TransUnit.CustomerId", Model.CustomerList)%>
                        <%= Html.ValidationMessage("TransUnit.CustomerId")%>
                    </td>
        </tr>
         <tr>
                    <td>
                        <label for="TransUnit_TransUnitPrice">
                            Harga :</label>
                    </td>
                    <td>
                       <%= Html.TextBox("TransUnit.TransUnitPrice", (Model.TransUnit.TransUnitPrice.HasValue) ? Model.TransUnit.TransUnitPrice.Value.ToString(CommonHelper.NumberFormat) : "")%>  
                        <%= Html.ValidationMessage("TransUnit.TransUnitPrice")%>
                    </td>
                </tr>
         <tr>
                    <td>
                        <label for="TransUnit_TransUnitDesc">
                            Keterangan :</label>
                    </td>
                    <td>
                        <%= Html.TextArea("TransUnit.TransUnitDesc", Model.TransUnit.TransUnitDesc)%>
                        <%= Html.ValidationMessage("TransUnit.TransUnitDesc")%>
                    </td>
                </tr>
    </table>
    <%
        }%>
</asp:Content>
