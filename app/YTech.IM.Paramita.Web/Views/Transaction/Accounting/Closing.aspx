<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<ClosingViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script language="javascript" type="text/javascript">

        $(function () {
            $("#Save").button();
            $("#DateFrom").datepicker({ dateFormat: "dd-M-yy" });
            $("#DateTo").datepicker({ dateFormat: "dd-M-yy" });
        });

        $(document).ready(function () {


        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    Tutup Buku
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

<%= Html.Partial("~/Views/Shared/Status.ascx",Model) %>


    <% using (Html.BeginForm())
       {%>
    <%=Html.AntiForgeryToken()%>
    
    <table>
        <tr>
            <td>
                <label for="DateFrom">
                    Dari Tanggal :</label>
            </td>
            <td>
                <%= Html.TextBox("DateFrom", (Model.DateFrom.HasValue) ? Model.DateFrom.Value.ToString("dd-MMM-yyyy") : "")%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="DateTo">
                    Sampai Tanggal :</label>
            </td>
            <td>
                <%= Html.TextBox("DateTo", (Model.DateTo.HasValue) ? Model.DateTo.Value.ToString("dd-MMM-yyyy") : "")%>
            </td>
        </tr>
    <tr>
        <td colspan="2" align="center">
            <span id="toolbar" class="ui-widget-header ui-corner-all">

        <button id="Save" type="submit">
            Ok</button>
    </span>
        </td>
    </tr>
    </table>
    <%
        }%>
</asp:Content>
