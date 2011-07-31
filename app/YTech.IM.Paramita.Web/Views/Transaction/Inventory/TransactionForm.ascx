<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TransactionFormViewModel>" %>
<%--<% using (Html.BeginForm())
   { %>--%>
<% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
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
<%= Html.AntiForgeryToken() %>
<%= Html.Hidden("Trans.Id", (ViewData.Model.Trans != null) ? ViewData.Model.Trans.Id : "")%>
<%= Html.Hidden("Trans.TransStatus", (ViewData.Model.Trans != null) ? ViewData.Model.Trans.TransStatus : "")%>
<div>
    <span id="toolbar" class="ui-widget-header ui-corner-all"><a id="newTrans" href="<%= Url.Action(ViewData.Model.Trans.TransStatus.Equals(EnumTransactionStatus.PurchaseOrder.ToString()) ? "Index" : Model.Trans.TransStatus.ToString(), "Inventory") %>">
        Baru</a>
        <button id="btnSave" type="submit" name="btnSave">
            Simpan</button>
        <%--<button id="btnPrint" name="btnPrint" type="submit">
            Cetak Faktur</button>--%>
        <button id="btnDelete" name="btnDelete" type="submit">
            Hapus</button>
           <%-- <%= ViewData.Model.Title %>--%>
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
        <td>
            <table>
                <% if (ViewData.Model.ViewUnitType)
                   {%>
                <tr>
                    <td>
                        <label for="Trans_UnitTypeId">
                            Tipe Unit :</label>
                    </td>
                    <td>
                        <%= Html.DropDownList("Trans.UnitTypeId", Model.UnitTypeList) %>
                        <%= Html.ValidationMessage("Trans.UnitTypeId") %>
                    </td>
                </tr>
                <%}%>
            </table>
        </td>
        <td>
            <table>
                <% if (ViewData.Model.ViewJobType)
                   {%>
                <tr>
                    <td>
                        <label for="Trans_JobTypeId">
                            Jenis Pekerjaan :</label>
                    </td>
                    <td>
                        <%= Html.DropDownList("Trans.JobTypeId", Model.JobTypeList) %>
                        <%= Html.ValidationMessage("Trans.JobTypeId") %>
                    </td>
                </tr>
                <%}%>
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
<script language="javascript" type="text/javascript">


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
        $("#btnPrint").removeAttr('disabled');
        $('#dialog p:first').text(msg);
        $("#Trans_TransFactur").val(json.FacturNo);
        $("#dialog").dialog("open"); 
    }
}


function ajaxValidate() {
var imgerror = '<%= Url.Content("~/Content/Images/cross.gif") %>';
    return $('form').validate({
    rules: {
       <% if (ViewData.Model.ViewFactur)
                   {	%> "Trans.TransFactur": { required: true  } <% } %>
     <% if (ViewData.Model.ViewDate)
                   {	%>
        ,"Trans.TransDate": { required: true } <% } %>
         <% if (ViewData.Model.ViewSupplier)
                   {	%> ,"Trans.TransBy": { required: true  } <% } %>
         <% if (ViewData.Model.ViewWarehouse)
                   {	%> ,"Trans.WarehouseId": { required: true  }<% } %>
        <% if (ViewData.Model.ViewWarehouseTo)
                   {	%> ,"Trans.WarehouseIdTo": { required: true  }<% } %>
        <% if (ViewData.Model.ViewUnitType)
                   {	%> ,"Trans.UnitTypeId": { required: true  }<% } %>
        <% if (ViewData.Model.ViewJobType)
                   {	%> ,"Trans.JobTypeId": { required: true  }<% } %>
    },
    messages: {
        <% if (ViewData.Model.ViewFactur) {	%>  "Trans.TransFactur": "<img id='TransFacturerror' src='"+imgerror+"' hovertext='No Faktur harus diisi' />"<% } %>
       <% if (ViewData.Model.ViewDate) {	%> ,"Trans.TransDate": "<img id='TransDateerror' src='"+imgerror+"' hovertext='Tanggal tidak boleh kosong' />" <% } %>
        <% if (ViewData.Model.ViewSupplier) {	%>  ,"Trans.TransBy": "<img id='TransByerror' src='"+imgerror+"' hovertext='Pilih Supplier' />"<% } %>
        <% if (ViewData.Model.ViewWarehouse) {	%>  ,"Trans.WarehouseId": "<img id='WarehouseIderror' src='"+imgerror+"' hovertext='Pilih Gudang' />"<% } %>
        <% if (ViewData.Model.ViewWarehouseTo) {	%>  ,"Trans.WarehouseIdTo": "<img id='WarehouseIdToerror' src='"+imgerror+"' hovertext='Pilih Gudang Tujuan' />"<% } %>
        <% if (ViewData.Model.ViewUnitType) {	%>  ,"Trans.UnitTypeId": "<img id='UnitTypeIdToerror' src='"+imgerror+"' hovertext='Pilih Tipe Unit' />"<% } %>
        <% if (ViewData.Model.ViewJobType) {	%>  ,"Trans.JobTypeId": "<img id='JobTypeIdToerror' src='"+imgerror+"' hovertext='Pilih Jenis Pekerjaan' />"<% } %>
        },
        invalidHandler: function(form, validator) {
          var errors = validator.numberOfInvalids();
						  if (errors) {
                          var message = "Validasi data kurang";
				$("div#error span#error_msg").html(message);
                  $("div#error").dialog("open");
			} else {
                  $("div#error").dialog("close");
			}
            		},
		errorPlacement: function(error, element) { 
			error.insertAfter(element);
		}
    }).form();
}


    function CalculateTotal() {
        var price = $('#TransDetPrice').val().replace(",","");
        var qty = $('#TransDetQty').val().replace(",","");
        var disc = $('#TransDetDisc').val().replace(",","");
        var subtotal = (price * qty)
        var total = subtotal - (disc * subtotal / 100);

        $('#TransDetTotal').attr('value', total);
    }

    $(function () {
        $("#newTrans").button();
        //$("#Save").button();
  $("#btnDelete").attr('disabled', 'disabled');

         <% if (TempData[EnumCommonViewData.SaveState.ToString()] != null)
{
    if (TempData[EnumCommonViewData.SaveState.ToString()].Equals(EnumSaveState.Failed))
    {%>
   $("#btnPrint").attr('disabled', 'disabled');
    <%
    }
} else { %>
  $("#btnPrint").attr('disabled', 'disabled');
<% } %>

        $("#Trans_TransDate").datepicker({ dateFormat: "dd-M-yy" });
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
         $("#popup").dialog({
                autoOpen: false,
                height: 420,
                width: '80%',
                modal: true,
                close: function(event, ui) {                 
                    //$("#list").trigger("reloadGrid");
                 }
            });

        $("#btnList").click(function () {
        var urlList = '<%= ResolveUrl("~/Transaction/Inventory/ListTransaction") %>';
          $("#popup_frame").attr("src", urlList+"?src=cc&transStatus="+$("#Trans_TransStatus").val());
            $("#popup").dialog("open");
                               });

      var editDialog = {
            url: '<%= Url.Action("Update", "Inventory") %>'
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
                     $('#imgItemId').click(function(){
                        OpenPopupItemSearch();
                        });
                }
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
        };
        var insertDialog = {
            url: '<%= Url.Action("Insert", "Inventory") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    $('#Id').removeAttr('disabled');
                    $('#TransDetQty').attr('value', '1');
                     <% if (ViewData.Model.ViewPrice)
               {%> 
                    $('#TransDetPrice').attr('value', '0');
                    $('#TransDetDisc').attr('value', '0');
                    $('#TransDetTotal').attr('value', '0');

                       <% if (ViewData.Model.ViewPrice)
               {%> 
                    $('#ItemId').change(function () {
                        var price = $.ajax({ url: '<%= ResolveUrl("~/Master/Item/Get") %>/' + $('#ItemId :selected').val(), async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the items.'); } }).responseText;
                        $('#TransDetPrice').attr('value', price);
                        CalculateTotal(); 
                    });
                    $('#TransDetPrice').change(function () {
                        CalculateTotal();
                    });
                    $('#TransDetQty').change(function () {
                        CalculateTotal();
                    });
                    $('#TransDetDisc').change(function () {
                        CalculateTotal();
                    });
                   <%
               }%>  
                   <%
               }%>  
                  
                     $('#imgItemId').click(function(){
                        OpenPopupItemSearch();
                        });  
                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
        };
        var deleteDialog = {
            url: '<%= Url.Action("Delete", "Inventory") %>'
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
        var imgLov = '<%= Url.Content("~/Content/Images/window16.gif") %>';
        $("#list").jqGrid({
            url: '<%= Url.Action("List", "Inventory", new { usePrice = ViewData.Model.ViewPrice} ) %>',
            datatype: 'json',
            mtype: 'GET',
            colNames: ['Id', 'Produk', 'Produk', 'Kuantitas',
            <% if (ViewData.Model.ViewPrice)
               {%> 'Harga', 'Diskon', 'Total',
                   <%
               }%>                   
                    'Keterangan'],
            colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: true, editable: false },
                      { name: 'ItemId', index: 'ItemId', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { edithidden: true }, hidden: true,
                       formoptions: {
                        "elmsuffix": "&nbsp;<img src='" + imgLov + "' style='cursor:hand;' id='imgItemId' />"
                    } }, 
                    { name: 'ItemName', index: 'ItemName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { edithidden: true} },
//                    { name: 'ItemId', index: 'ItemId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { edithidden: true }, hidden: true },
//                    { name: 'ItemName', index: 'ItemName', width: 200, align: 'left', editable: false, edittype: 'select', editrules: { edithidden: true} },
                     { name: 'TransDetQty', index: 'TransDetQty', width: 200, sortable: false, align: 'right', editable: true, editrules: { required: false  },
                       editoptions: {
                           dataInit: function (elem) {
                               $(elem).autoNumeric();
                               $(elem).attr("style","text-align:right;");
                           }
                       }
                        },
                   <% if (ViewData.Model.ViewPrice) {%> 
                   { name: 'TransDetPrice', index: 'TransDetPrice', width: 200, sortable: false, align: 'right', editable: true, editrules: { required: false  },
                       editoptions: {
                           dataInit: function (elem) {
                               $(elem).autoNumeric();
                               $(elem).attr("style","text-align:right;");
                           }
                       }
                        }, 
                   { name: 'TransDetDisc', index: 'TransDetDisc', width: 200, sortable: false, align: 'right', editable: true, editrules: { required: false },
                       editoptions: {
                           dataInit: function (elem) {
                               $(elem).autoNumeric();
                               $(elem).attr("style","text-align:right;");
                           }
                       }
                        },
                   { name: 'TransDetTotal', index: 'TransDetTotal', width: 200, sortable: false, align: 'right', editable: true, editrules: { required: false },
                       editoptions: {
                           dataInit: function (elem) {
                               $(elem).autoNumeric();
                               $(elem).attr("style","text-align:right;");
                           }
                       }
                        },
                   <%}%> 
                { name: 'TransDetDesc', index: 'TransDetDesc', width: 200, sortable: false, align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "20" }, editrules: { required: false}}],

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
                      
//function to generate tooltips
		function generateTooltips() {
		  //make sure tool tip is enabled for any new error label
//          alert('s');
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

         function OpenPopupItemSearch()
        {
            var popup_frame = $("#popup_frame");
            var new_url = '<%= ResolveUrl("~/Master/Item/Search") %>';
            if (popup_frame.attr("src") != new_url) {
                popup_frame.attr("src", new_url);
            }
        
            $("#popup").dialog("open");
            return false;   
        }

         function SetItemDetail(itemId, itemName, price)
        {
//        alert(itemId);
//        alert(itemName);
//        alert(price);
  $("#popup").dialog("close");
          $('#ItemId').val(itemId);
          $('#ItemName').val(itemName);
           <% if (ViewData.Model.ViewPrice)
               {%>   
               $('#TransDetPrice').val(price.toString());
            CalculateTotal();
                   <%
               }%>  
               
                <% if (ViewData.Model.Trans.TransStatus == EnumTransactionStatus.PurchaseOrder.ToString())
{%> 
                    var warehouseId  = $('#Trans_WarehouseId option:selected').val();
                    var warehouseName  = $('#Trans_WarehouseId option:selected').text();
        var imgLov = '<%= Url.Content("~/Content/Images/window16.gif") %>';
//                    alert(itemId);
//                    alert(warehouseId);
                    var totalQtyBudget = $.ajax({ url: '<%= ResolveUrl("~/Master/Item/GetTotalBudget") %>?itemId=' + itemId + '&warehouseId='+ warehouseId, async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the totalQtyBudget.'); } }).responseText;
                    var totalQtyUsed = $.ajax({ url: '<%= ResolveUrl("~/Master/Item/GetTotalUsed") %>?itemId=' + itemId + '&warehouseId='+ warehouseId, async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the totalQtyUsed.'); } }).responseText;
                    var displayText = "Detail "+ itemName + " di gudang " + warehouseName ;
                     displayText += "<br />Total Anggaran : " + totalQtyBudget;
                    displayText += "<br />Total Pemakaian : " + totalQtyUsed;

                    $('#dialog p:first').html(displayText);
                    $("#dialog").dialog("open");
//                    setTimeout("$('#dialog').dialog('close');",5000);
               <%
}%>      
        }

         function SetTransDetail(src,transId)
        {
            $("#popup").dialog("close");
            var trans = $.parseJSON($.ajax({ url: '<%= Url.Action("GetJsonTrans","Inventory") %>?transId=' + transId, async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the trans.'); } }).responseText);
            if (trans) {
                if (trans.TransDate)
                {
                    var transDate = new Date(parseInt(trans.TransDate.substr(6)));
                    //alert('debug 3');
                    $("#Trans_TransDate").val(transDate.format('dd-mmm-yyyy'));
                }

            $("#Trans_Id").val(trans.TransId); 
            $("#Trans_TransFactur").val(trans.TransFactur); 
            $("#Trans_WarehouseId").val(trans.WarehouseId); 
            $("#Trans_TransPaymentMethod").val(trans.TransPaymentMethod); 
            $("#Trans_TransBy").val(trans.TransBy); 
            $("#Trans_WarehouseIdTo").val(trans.WarehouseIdTo); 
            $("#Trans_UnitTypeId").val(trans.UnitTypeId); 
            $("#Trans_JobTypeId").val(trans.JobTypeId); 

            setTimeout("$('#list').trigger('reloadGrid')",1000); 
             $("#btnPrint").removeAttr('disabled');
             $("#btnDelete").removeAttr('disabled');
        }

        } 
</script>
