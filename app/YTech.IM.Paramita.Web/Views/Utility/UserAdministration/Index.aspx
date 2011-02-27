<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" Inherits="System.Web.Mvc.ViewPage<YTech.IM.Paramita.Web.Controllers.ViewModel.UserAdministration.IndexViewModel>" %>

<asp:Content ContentPlaceHolderID="title" runat="server">
	Administrasi User
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

	<%--<link href='<% =Url.Content("~/Content/MvcMembership.css") %>' rel="stylesheet" type="text/css" />

	<h2 class="mvcMembership">Administrasi User</h2>
	
	<h3 class="mvcMembership">Users</h3>
	<div class="mvcMembership-allUsers">
	<% if(Model.Users.Count > 0){ %>
		<ul class="mvcMembership mvcMembership-users">
			<% foreach(var user in Model.Users){ %>
			<li>
				<span class="mvcMembership-username"><% =Html.ActionLink(user.UserName, "Details", "UserAdministration", new { id = user.ProviderUserKey },null)%></span>
				<span class="mvcMembership-email"><a href="mailto:<%= user.Email %>"><%= user.Email %></a></span>
				<% if(user.IsOnline){ %>
					<span class="mvcMembership-isOnline">Online</span>
				<% }else{ %>
					<span class="mvcMembership-isOffline">Offline for
						<%
							var offlineSince = (DateTime.Now - user.LastActivityDate);
							if (offlineSince.TotalSeconds <= 60) Response.Write("1 minute.");
							else if(offlineSince.TotalMinutes < 60) Response.Write(Math.Floor(offlineSince.TotalMinutes) + " minutes.");
							else if (offlineSince.TotalMinutes < 120) Response.Write("1 hour.");
							else if (offlineSince.TotalHours < 24) Response.Write(Math.Floor(offlineSince.TotalHours) + " hours.");
							else if (offlineSince.TotalHours < 48) Response.Write("1 day.");
							else Response.Write(Math.Floor(offlineSince.TotalDays) + " days.");
						%>
					</span>
				<% } %>
				<% if(!string.IsNullOrEmpty(user.Comment)){ %>
					<span class="mvcMembership-comment"><%= user.Comment %></span>
				<% } %>
			</li>
			<% } %>
		</ul>
		<ul class="mvcMembership mvcMembership-paging">
			<% if (Model.Users.IsFirstPage){ %>
			<li>First</li>
			<li>Previous</li>
			<% }else{ %>
			<li><% =Html.ActionLink("First", "Index", "UserAdministration")%></li>
			<li><% =Html.ActionLink("Previous", "Index", "UserAdministration", new { index = Model.Users.PageIndex - 1 },null)%></li>
			<% } %>

			<li>Page <% =Model.Users.PageNumber%> of <% =Model.Users.PageCount%></li>

			<% if (Model.Users.IsLastPage){ %>
			<li>Next</li>
			<li>Last</li>
			<% }else{ %>
			<li><% =Html.ActionLink("Next", "Index", "UserAdministration", new { index = Model.Users.PageIndex + 1 }, null)%></li>
			<li><% =Html.ActionLink("Last", "Index", "UserAdministration", new { index = Model.Users.PageCount - 1 }, null)%></li>
			<% } %>
		</ul>
	<% }else{ %>
		<p>No users have registered.</p>
	<% } %>
	</div>

	<h3 class="mvcMembership">Roles</h3>
	<div class="mvcMembership-allRoles">
	<% if(Model.Roles.Count() > 0 ){ %>
		<ul class="mvcMembership">
			<% foreach(var role in Model.Roles){ %>
			<li>
				<% =Html.ActionLink(role, "Role", "UserAdministration", new { id = role },null)%>
				<% using(Html.BeginForm("DeleteRole", "UserAdministration", new{id=role})){ %>
				<input type="submit" value="Delete" />
				<% } %>
			</li>
			<% } %>
		</ul>
	<% }else{ %>
		<p>No roles have been created.</p>
	<% } %>
	<% using(Html.BeginForm("CreateRole", "UserAdministration")){ %>
		<fieldset>
			<label for="id">Role:</label>
			<% =Html.TextBox("id") %>
			<input type="submit" value="Create Role" />
		</fieldset>
	<% } %>
	</div>--%>

     <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <div id="dialog" title="Status">
        <p></p>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript">

      $(document).ready(function () {

          $("#dialog").dialog({
              autoOpen: false
          });


          var editDialog = {
              url: '<%= Url.Action("Update", "UserAdministration") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true

                , onclickSubmit: function (params) {
                    var ajaxData = {};

                    var list = $("#list");
                    var selectedRow = list.getGridParam("selrow");
                    rowData = list.getRowData(selectedRow);
                    ajaxData = { UserName: rowData.UserName };

                    return ajaxData;
                }
                , afterShowForm: function (eparams) {
                    $('#UserName').attr('disabled', 'disabled');
                    $('#tr_Password', eparams).hide();
                    $('#tr_PasswordConfirm', eparams).hide();
                }
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
          };
          var insertDialog = {
              url: '<%= Url.Action("Insert", "UserAdministration") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    $('#UserName').attr('disabled', '');
                    $('#tr_Password', eparams).show();
                    $('#tr_PasswordConfirm', eparams).show();
                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
          };
          var deleteDialog = {
              url: '<%= Url.Action("Delete", "UserAdministration") %>'
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
          $.jgrid.edit.addCaption = "Tambah User Baru";
          $.jgrid.edit.editCaption = "Edit User";
          $.jgrid.del.caption = "Hapus User";
          $.jgrid.del.msg = "Anda yakin menghapus User yang dipilih?";
          $("#list").jqGrid({
              url: '<%= Url.Action("List", "UserAdministration") %>',
              datatype: 'json',
              mtype: 'GET',
              colNames: ['Nama User', 'Password', 'Konfirmasi Password', 'Keterangan', 'Aktifitas Terakhir'],
              colModel: [
                    { name: 'UserName', index: 'UserName', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'Password', index: 'Password', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false }, hidden: true },
                    { name: 'PasswordConfirm', index: 'PasswordConfirm', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false }, hidden: true },
                    { name: 'Comment', index: 'Comment', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false } },
                   { name: 'LastActivityDate', index: 'LastActivityDate', width: 200, sortable: false, align: 'left', editable: false, editoptions: { edithidden: true }, editrules: { required: false}}],

              pager: $('#listPager'),
              rowNum: 20,
              rowList: [20, 30, 50, 100],
              rownumbers: true,
              sortname: 'UserName',
              sortorder: "asc",
              viewrecords: true,
              height: 300,
              caption: 'Daftar User',
              autowidth: true,
              ondblClickRow: function (rowid, iRow, iCol, e) {
                  $("#list").editGridRow(rowid, editDialog);
              }
          }).navGrid('#listPager',
                {
                    edit: false, add: true, del: true, search: false, refresh: true
                },
                editDialog,
                insertDialog,
                deleteDialog
            );
      });       
    </script>
</asp:Content>