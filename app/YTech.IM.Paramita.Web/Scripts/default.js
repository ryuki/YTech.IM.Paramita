/// <reference path="jquery-1.4.1-vsdoc.js" />

// Numeric only control handler
jQuery.fn.ForceNumericOnly =
function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            alert(e.keyCode);
            var key = e.charCode || e.keyCode || 0;
            // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
            return (
                key == 8 ||
                key == 9 ||
                key == 46 ||
                (key >= 37 && key <= 40) ||
                (key >= 48 && key <= 57) ||
                (key >= 96 && key <= 105));
        })
    })

    $.fn.autoNumeric.defaults = {/* plugin defaults */
        aNum: '0123456789', /*  allowed  numeric values */
        aNeg: '-', /* allowed negative sign / character */
        aSep: ',', /* allowed thousand separator character */
        aDec: '.', /* allowed decimal separator character */
        aSign: '', /* allowed currency symbol */
        pSign: 'p', /* placement of currency sign prefix or suffix */
        mNum: 15, /* max number of numerical characters to the left of the decimal */
        mDec: 2, /* max number of decimal places */
        dGroup: 3, /* digital grouping for the thousand separator used in Format */
        mRound: 'S', /* method used for rounding */
        aPad: true/* true= always Pad decimals with zeros, false=does not pad with zeros. If the value is 1000, mDec=2 and aPad=true, the output will be 1000.00, if aPad=false the output will be 1000 (no decimals added) Special Thanks to Jonas Johansson */
    };
};


function RemoveAttribute(ctlId, attr) {
    // alert($.browser.msie);
    // alert($.browser.version);
    if ($.browser.msie && $.browser.version == '6.0') {
        // alert('this ie6 and ie');
        //ie6 bugs
        var obj = document.getElementById(ctlId);
        if (obj)
            obj.setAttribute(attr, "");
    }
    else {
        $("#" + ctlId).removeAttr(attr);
    }
}


function exportExcel(grid) {
    //alert('export to excel');
    var mya = new Array();
    mya = grid.getDataIDs();  // Get All IDs
    var data = grid.getRowData(mya[0]);     // Get First row to get the labels
    var columnNames = grid.jqGrid('getGridParam', 'colNames');
    //            for (i = 0; i < columnNames.length; i++) {
    //                alert(columnNames[i]);
    //            }
    var colNames = new Array();
    var ii = 0;
    var j = 1;
    var html = "";
    for (var i in data) {
        //alert(columnNames[j]);
        html = html + columnNames[j] + ",";
        colNames[ii++] = i;
        j++;
    }    // capture col names
    html = html + "\n";   // Output header with end of line
    for (i = 0; i < mya.length; i++) {
        data = grid.getRowData(mya[i]); // get each row
        for (j = 0; j < colNames.length; j++) {
            html = html + data[colNames[j]] + ","; // output each column as tab delimited
        }
        html = html + "\n";  // output each row with end of line

    }
    html = html + "\n";  // end of line at the end
    //alert(html);
    var f = "<form id='formExport' method='post' action='../Master/Brand/Export' target = '_blank'>";
    f += "<input type='hidden' name='csvBuffer' id='csvBuffer' />"
    f += "</form>";
    $('body').append(f);
    $('#csvBuffer').val(html);
    $('#formExport').submit();
//    document.forms[0].csvBuffer.value = html;
//    document.forms[0].method = 'POST';
//    document.forms[0].action = '../Master/Brand/Export';  // send it to server which will open this contents in excel file
//    document.forms[0].target = '_blank';
//    document.forms[0].submit();
}    