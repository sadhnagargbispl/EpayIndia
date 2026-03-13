<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UniTree.aspx.cs" Inherits="UniTree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>NewTree</title>
    <link href="css/tree.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        #dhtmltooltip {
            BORDER-RIGHT: black 1px solid;
            PADDING-RIGHT: 2px;
            BORDER-TOP: black 1px solid;
            PADDING-LEFT: 2px;
            Z-INDEX: 100;
            LEFT: -300px;
            VISIBILITY: hidden;
            PADDING-BOTTOM: 2px;
            BORDER-LEFT: black 1px solid;
            WIDTH: 150px;
            PADDING-TOP: 2px;
            BORDER-BOTTOM: black 1px solid;
            POSITION: absolute;
            BACKGROUND-COLOR: Yellow
        }

        #dhtmlpointer {
            Z-INDEX: 101;
            LEFT: -300px;
            VISIBILITY: hidden;
            POSITION: absolute
        }
    </style>
    
    <link href="css/dtree.css" rel="stylesheet" type="text/css" />
    <script src="js/Dtree/dtree.js" type="text/javascript"></script>
    <%--<script src="dtree/dtree.js" type="text/javascript"></script>--%>
    <script src="js/Dtree/Universaldtree.js" type="text/javascript"></script>
    <%--		<script src="dtree/vertdtree.js" type="text/javascript"></script>--%>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <script type="text/javascript">

        var offsetfromcursorX = 22 //Customize x offset of tooltip
        var offsetfromcursorY = 20 //Customize y offset of tooltip

        var offsetdivfrompointerX = 22 //Customize x offset of tooltip div relative to pointer image
        var offsetdivfrompointerY = 24 //Customize y offset of tooltip div relative to pointer image. Tip: Set it to (height_of_pointer_image-1).

        document.write('<div id="dhtmltooltip"></div>') //write out tooltip div
        document.write('<img id="dhtmlpointer" src="images/arrows.png">') //write out pointer image

        var ie = document.all
        var ns6 = document.getElementById && !document.all
        var enabletip = false
        if (ie || ns6)
            var tipobj = document.all ? document.all["dhtmltooltip"] : document.getElementById ? document.getElementById("dhtmltooltip") : ""

        var pointerobj = document.all ? document.all["dhtmlpointer"] : document.getElementById ? document.getElementById("dhtmlpointer") : ""

        function ietruebody() {
            return (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body
        }

        function ddrivetip(thetext, thewidth, thecolor) {
            if (ns6 || ie) {
                if (typeof thewidth != "undefined") tipobj.style.width = thewidth + "px"
                if (typeof thecolor != "undefined" && thecolor != "")
                    tipobj.style.backgroundColor = thecolor
                else
                    tipobj.style.backgroundColor = "black"
                tipobj.innerHTML = thetext
                enabletip = true
                return false
            }
        }

        function positiontip(e) {
            if (enabletip) {
                var nondefaultpos = false
                var curX = (ns6) ? e.pageX : event.clientX + ietruebody().scrollLeft;
                var curY = (ns6) ? e.pageY : event.clientY + ietruebody().scrollTop;
                //Find out how close the mouse is to the corner of the window
                var winwidth = ie && !window.opera ? ietruebody().clientWidth : window.innerWidth - 20
                var winheight = ie && !window.opera ? ietruebody().clientHeight : window.innerHeight - 20

                var rightedge = ie && !window.opera ? winwidth - event.clientX - offsetfromcursorX : winwidth - e.clientX - offsetfromcursorX
                var bottomedge = ie && !window.opera ? winheight - event.clientY - offsetfromcursorY : winheight - e.clientY - offsetfromcursorY

                var leftedge = (offsetfromcursorX < 0) ? offsetfromcursorX * (-1) : -1000

                //if the horizontal distance isn't enough to accomodate the width of the context menu
                if (rightedge < tipobj.offsetWidth) {
                    //move the horizontal position of the menu to the left by it's width
                    tipobj.style.left = curX - tipobj.offsetWidth + "px"
                    nondefaultpos = true
                }
                else if (curX < leftedge)
                    tipobj.style.left = "5px"
                else {
                    //position the horizontal position of the menu where the mouse is positioned
                    tipobj.style.left = curX + offsetfromcursorX - offsetdivfrompointerX + "px"
                    pointerobj.style.left = curX + offsetfromcursorX + "px"
                }

                //same concept with the vertical position
                if (bottomedge < tipobj.offsetHeight) {
                    tipobj.style.top = curY - tipobj.offsetHeight - offsetfromcursorY + "px"
                    nondefaultpos = true
                }
                else {
                    tipobj.style.top = curY + offsetfromcursorY + offsetdivfrompointerY + "px"
                    pointerobj.style.top = curY + offsetfromcursorY + "px"
                }
                tipobj.style.visibility = "visible"
                if (!nondefaultpos)
                    pointerobj.style.visibility = "visible"
                else
                    pointerobj.style.visibility = "hidden"
            }
        }

        function hideddrivetip() {
            if (ns6 || ie) {
                enabletip = false
                tipobj.style.visibility = "hidden"
                pointerobj.style.visibility = "hidden"
                tipobj.style.left = "-1000px"
                tipobj.style.backgroundColor = ''
                tipobj.style.width = ''
            }
        }

        document.onmousemove = positiontip



    </script>
</head>
<body>
    <form id="form1" runat="server">
        <center>
            <div>
                <div style="vertical-align: top; position: absolute; top: 8px; left: 0px;">

                    <table id="Table1" cellpadding="0" cellspacing="1" border="0" width="300px" style="vertical-align: top; padding-left: 10px"
                        runat="server">
                        <tr>
                            <td colspan="8" align="center">
                                <asp:Label ID="lblError" Visible="false" runat="server" Font-Size="13px" Text=""
                                    ForeColor="Red" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:Button ID="cmdBack" runat="server" Text="Back" class="btn btn-primary" OnClick="cmdBack_Click"   />
                            </td>
                        </tr>

                        <tr style="font-weight: bold; font-size: 10px;">
                            <td id="td21" style="width: 15%" align="center" runat="server"></td>
                            <td id="td22" style="width: 15%" align="center" runat="server"></td>
                            <td id="td23" style="width: 15%" align="center" runat="server"></td>
                            <td id="td24" style="width: 15%" align="center" runat="server"></td>
                            <td id="td25" style="width: 15%" align="center" runat="server"></td>
                            <td id="td26" style="width: 15%" align="center" runat="server"></td>
                            <td id="td27" style="width: 15%" align="center" runat="server"></td>
                            <td id="td28" style="width: 15%" align="center" runat="server"></td>
                        </tr>
                    </table>
                </div>
                <center>
                </center>
            </div>
        </center>
    </form>
</body>
</html>
