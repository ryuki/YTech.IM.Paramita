<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.Master" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
       <%-- OMG, What have you done???  You broke the application...YOU broke it!!!--%>
         <% Html.RenderPartial("DebugFormSubmission", ViewData); %>
    </div>
</asp:Content>