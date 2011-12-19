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
