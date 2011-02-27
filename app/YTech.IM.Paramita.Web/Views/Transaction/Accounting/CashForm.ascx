<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<YTech.IM.Paramita.Web.Controllers.ViewModel.CashFormViewModel>" %>
<% if (false)
   { %>
<script src="../../../Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<% } %>
<%-- <% using (Html.BeginForm())
   {%> --%>
<% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnBegin = "ajaxValidate",
                                           OnSuccess = "onSavedSuccess"
                                       }

          ))
   {%>
<div id="status">
</div>
<div class="ui-state-highlight ui-corner-all" style="padding: 5pt; margin-bottom: 5pt;
    display: none;" id="error">
    <p>
        <span class="ui-icon ui-icon-error" style="float: left; margin-right: 0.3em;"></span>
        <span id="error_msg"></span>.<br clear="all" />
    </p>
</div>
<div id="formArea">
    <%=Html.AntiForgeryToken()%>
    <%=Html.Hidden("Journal.Id", (ViewData.Model.Journal != null) ? ViewData.Model.Journal.Id : "")%>
    <%=Html.Hidden("Journal.JournalType", ViewData.Model.Journal.JournalType)%>
    <div>
        <span id="toolbar" class="ui-widget-header ui-corner-all"><a id="newJournal" href="<%= Url.Action(ViewData.Model.Journal.JournalType, "Accounting") %>">
            Baru</a>
            <button id="Save" type="submit">
                Simpan</button>
            <a id="print" href="<%= Url.Action(ViewData.Model.Journal.JournalType, "Accounting") %>">
                Cetak</a> </span>
    </div>
    <table>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <label for="Journal_JournalDate">
                                Tanggal :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("Journal.JournalDate", (Model.Journal.JournalDate.HasValue) ? Model.Journal.JournalDate.Value.ToString("dd-MMM-yyyy") : "")%>
                            <%= Html.ValidationMessage("Journal.JournalDate")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="Journal_JournalVoucherNo">
                                No Voucher :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("Journal.JournalVoucherNo", Model.Journal.JournalVoucherNo)%>
                            <%= Html.ValidationMessage("Journal.JournalVoucherNo")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="Journal_CostCenterId">
                                Cost Center :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("Journal.CostCenterId", Model.CostCenterList)%>
                            <%= Html.ValidationMessage("Journal.CostCenterId")%>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <label for="AccountId">
                                Akun Kas :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("AccountId", Model.AccountList)%>
                            <%= Html.ValidationMessage("AccountId")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="Journal_JournalDesc">
                                Keterangan :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("Journal.JournalDesc", Model.Journal.JournalDesc)%>
                            <%= Html.ValidationMessage("Journal.JournalDesc")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<%
    }
%>
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
<script language="javascript" type="text/javascript">

function onSavedSuccess() {
 $("#Save").attr('disabled', 'disabled');
}


function ajaxValidate() {
var imgerror = '<%= Url.Content("~/Content/Images/cross.gif") %>';
    return $('form').validate({
    rules: {
        "Journal.JournalDate": { required: true
//       , date: function(element) {
//        var d = Date.parse($("#Journal_JournalDate").val());
////        alert(d.toString);
//        if (d)
//        return true;
//        else 
//        return false;
//      }
 },
        "Journal.CostCenterId": { required: true  },
        "AccountId": { required: true  }
    },
    messages: {
        "Journal.JournalDate": "<img id='JournalDateerror' src='"+imgerror+"' hovertext='Tanggal tidak boleh kosong' />"
//        {
//            required: "Tanggal tidak boleh kosong",
//            date: "Format tanggal salah"
//            }
            ,
        "Journal.CostCenterId": "<img id='CostCenterIderror' src='"+imgerror+"' hovertext='Pilih cost center' />",
        "AccountId": "<img id='AccountIderror' src='"+imgerror+"' hovertext='Pilih akun kas' />"
        },        invalidHandler: function(form, validator) {          var errors = validator.numberOfInvalids();
						  if (errors) {
                          var message = "Validasi data kurang";
//										var message = errors == 1
//					? 'You missed 1 field. It has been highlighted below'
//					: 'You missed ' + errors + ' fields.  They have been highlighted below';
				$("div#error span#error_msg").html(message);
//				$("div#error").show();
                  $("div#error").dialog("open");
			} else {
                  $("div#error").dialog("close");
//				$("div#error").hide();
			}
            		},
		errorPlacement: function(error, element) { 
			error.insertAfter(element);
			//generateTooltips();
		}
    }).form();
}


    $(function () {
        $("#newJournal").button();
        $("#Save").button();
        $("#print").hide();
        <% if (TempData[EnumCommonViewData.SaveState.ToString()] != null)
{
    if (TempData[EnumCommonViewData.SaveState.ToString()].Equals(EnumSaveState.Failed))
    {%>
   $("#print").attr('disabled', 'disabled');
    <%
    }
} else { %>
  $("#print").attr('disabled', 'disabled');
<% } %>

        $("#Journal_JournalDate").datepicker({ dateFormat: "dd-M-yy" });
    });

    $(document).ready(function () {
      $("form").mouseover(function () {
                generateTooltips();
            });
        $("#dialog").dialog({
            autoOpen: false
        });

        $("div#error").dialog({
            autoOpen: false
        });

        var editDialog = {
            url: '<%= Url.Action("Update", "Accounting") %>'
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
            url: '<%= Url.Action("Insert", "Accounting") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    $('#Id').attr('disabled', '');
                    $('#JournalDetAmmount').attr('value', '0');

                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
        };
        var deleteDialog = {
            url: '<%= Url.Action("Delete", "Accounting") %>'
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
            url: '<%= Url.Action("List", "Accounting") %>',
            datatype: 'json',
            mtype: 'GET',
            colNames: ['Id', 'Akun', 'Akun', 'No Bukti', 'Status', 'Jumlah', 'Debet', 'Kredit', 'Keterangan'],
            colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: true, editable: false },
                    { name: 'AccountId', index: 'AccountId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { edithidden: true }, hidden: true },
                    { name: 'AccountName', index: 'AccountName', width: 200, align: 'left', editable: false, sortable: false, edittype: 'select', editrules: { edithidden: true} },
                    { name: 'JournalDetEvidenceNo', index: 'JournalDetEvidenceNo', width: 200, sortable: false, align: 'left', editable: true, editrules: { edithidden: true} },
                    { name: 'JournalDetStatus', index: 'JournalDetStatus', width: 200, align: 'left', editable: true, edittype: 'select', editoptions: { value: "D:Debet;K:Kredit" }, editrules: { edithidden: true }, hidden: true, editable: false },
                    { name: 'JournalDetAmmount', index: 'JournalDetAmmount', width: 200, align: 'left', editable: true, editrules: { edithidden: true }, hidden: false,
                        editoptions: {
                            dataInit: function (elem) {
                                $(elem).autoNumeric();
                                $(elem).attr("style", "text-align:right;");
                            }
                        }
                    },
                   { name: 'JournalDetAmmountDebet', index: 'JournalDetAmmountDebet', width: 200, sortable: false, editable: false, align: 'right', editable: false, editrules: { required: false, number: true, edithidden: true }, hidden: true },
                   { name: 'JournalDetAmmountKredit', index: 'JournalDetAmmountKredit', width: 200, sortable: false, editable: false, align: 'right', editable: false, editrules: { required: false, number: true, edithidden: true }, hidden: true },
                   { name: 'JournalDetDesc', index: 'BrandDesc', width: 200, sortable: false, align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "20" }, editrules: { required: false}}],

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
                $('#list').setColProp('AccountId', { editoptions: { value: accounts} });
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
    });

                var accounts = $.ajax({ url: '<%= ResolveUrl("~/Master/Account/GetList") %>', async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the accounts.'); } }).responseText;

                
//function to generate tooltips
		function generateTooltips() {
		  //make sure tool tip is enabled for any new error label//          alert('s');
			$("img[id*='error']").tooltip({
				showURL: false,
				opacity: 0.99,
				fade: 150,
				positionRight: true,
					bodyHandler: function() {
						return $("#"+this.id).attr("hovertext");
					}
			});
			//make sure tool tip is enabled for any new valid label
			$("img[src*='tick.gif']").tooltip({
				showURL: false,
					bodyHandler: function() {
						return "OK";
					}
			});
		}
</script>
