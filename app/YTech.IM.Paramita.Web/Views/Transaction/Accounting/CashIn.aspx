<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<YTech.IM.Paramita.Web.Controllers.ViewModel.CashFormViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
<%= ViewData.Model.Title %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("CashForm", ViewData); %>
</asp:Content>
