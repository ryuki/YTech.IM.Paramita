<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %> 
<style>
        /* Update progress control */
        #LayerProgress
        {
            position: absolute;
            left: -10px;
            top: 0px;
            z-index: 5;
            background-color: Gray;
            filter: alpha(opacity=30);
            opacity: 0.3;
        }
        #LayerLabelProgress
        {
            position: absolute;
            left: -10px;
            top: 0px;
            z-index: 100;
        }
    </style>
    <div id="progress">
        <div id="LayerProgress">
            &nbsp;</div>
        <div id="LayerLabelProgress">
            <table width="100%" style="height: 100%">
                <tr>
                    <td align="center" valign="middle">
                        <img src='<%=
ResolveUrl("~/Content/Images/loading.gif") %>' alt="" style="vertical-align: middle;" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#progress').hide();
            initializeProgressLayer();
            window.onscroll = initializeProgressLayer;
            window.onresize = initializeProgressLayer;
        });

        function initializeProgressLayer() {
            var objLayer = $('#LayerProgress');
            var objLabel = $('#LayerLabelProgress');

            objLayer.css({
                'width': document.body.clientWidth + 50,
                'height': document.body.clientHeight + 50,
                'top': document.body.scrollTop
            });
            objLabel.css({
                'width': document.body.clientWidth + 50,
                'height': document.body.clientHeight + 50,
                'top': (document.body.clientHeight / 2) + document.body.scrollTop
            });
        }
    </script>