<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jsontool.aspx.cs"  ValidateRequest="false" Inherits="PartyProject.jsontool" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <%--<link rel="shortcut icon" href="bai.ico" type="image/x-icon" />--%>
    <title>在线测试</title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 69px;
            height: 516px;
        }
        .style4
        {
            font-size: xx-large;
            font-weight: bold;
            text-align: center;
            height: 89px;
        }
        .style5
        {
            width: 69px;
            height: 43px;
        }
        .style6
        {
            height: 43px;
        }
        .style8
        {
            height: 45px;
        }
        .style9
        {
            width: 69px;
            height: 38px;
        }
        .style10
        {
            height: 38px;
        }
        .style11
        {
            height: 43px;
            width: 594px;
        }
        .style12
        {
            height: 38px;
            width: 594px;
        }
        .style13
        {
            height: 45px;
            width: 594px;
        }
        .style14
        {
            width: 594px;
            height: 516px;
        }
        .style15
        {
            height: 516px;
        }
        .style16
        {
            height: 45px;
            text-align: center;
            width: 69px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table class="style1">
        <tr>
            <td class="style4" colspan="3">
                接口测试页面</td>
        </tr>
        <tr>
            <td class="style5">
            </td>
            <td class="style11">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                测试环境：<asp:DropDownList 
                    ID="DropDownList1" runat="server" AutoPostBack="True" 
                    Height="23px" onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
                    Width="300px">
                </asp:DropDownList>
            </td>
            <td class="style6">
                <asp:Label ID="lbl_url" runat="server" Text=" 测试的url" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style9">
            </td>
            <td class="style12">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                测试类别：<asp:DropDownList ID="DropDownList2" runat="server" Height="23px" 
                    onselectedindexchanged="DropDownList2_SelectedIndexChanged" Width="300px" 
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td class="style10">
                测试服务号：<asp:Label ID="lbl_RequestId" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style9">
                &nbsp;</td>
            <td class="style12">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                测试服务：<asp:DropDownList ID="DropDownList3" 
                    runat="server" Height="23px" 
                    Width="300px" AutoPostBack="True" 
                    onselectedindexchanged="DropDownList3_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="style10">
                 <asp:Button ID="Button3" runat="server" onclick="Button1_Click" Text="测试" Height="32px" Width="77px" />
            </td>
        </tr>
        <tr>
            <td class="style9">
                &nbsp;</td>
            <td class="style12">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                获得MD5：<asp:TextBox ID="tb_MD5" runat="server"></asp:TextBox>
                <input type="button"  value="获得MD5" onclick="getMD5()" />
            </td>
            <td class="style10">
                <%--<asp:Button ID="btn_MD5" runat="server" onclick="btn_MD5_Click" Text="获得MD5" Height="32px" Width="77px" />--%>
            </td>
        </tr>
        <tr>
            <td class="style9">
                &nbsp;</td>
            <td class="style12">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                转换坐标（高德转百度）:<br/>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                Lat:<asp:TextBox ID="txt_lat" runat="server"></asp:TextBox><br/>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                Lng:<asp:TextBox ID="txt_lng" runat="server"></asp:TextBox><br/>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button"  value="转换坐标" onclick="getCoordinator()" />
            </td>
            <td class="style10">
                <%--<asp:Button ID="btn_MD5" runat="server" onclick="btn_MD5_Click" Text="获得MD5" Height="32px" Width="77px" />--%>
            </td>
        </tr>
        <tr>
            <td class="style16">
                &nbsp;</td>
            <td class="style13">
                RequestXml</td>
            <td class="style8">
                &nbsp;ResponseXml&nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
            </td>
            <td class="style14">
        <asp:TextBox ID="tbx_Request" runat="server" Height="495px" TextMode="MultiLine" 
            Width="569px" style="margin-top: 0px"></asp:TextBox>
    
            </td>
            <td class="style15">
                <asp:TextBox ID="tbx_Response" runat="server" Height="494px" 
                    TextMode="MultiLine" Width="549px"></asp:TextBox>
            </td>
        </tr>
        </table>
    </form>
    <script type="text/javascript">
        function getMD5()
        {
            if (document.getElementById("<%=tb_MD5.ClientID %>").value == "") {
                return;
            }
            AjaxGetFunc(
                "jsontool.aspx?GETMD5=" + document.getElementById("<%=tb_MD5.ClientID %>").value,
                function(responseText){
                    document.getElementById("<%=tb_MD5.ClientID %>").value = responseText;
                });
            
        }

        function getCoordinator() {
            if (document.getElementById("<%=txt_lat.ClientID %>").value == "" || document.getElementById("<%=txt_lng.ClientID %>").value == "") {
                return;
            }
            AjaxGetFunc(
                "jsontool.aspx?ChangeCoords=1&lat=" + document.getElementById("<%=txt_lat.ClientID %>").value + "&lng=" + document.getElementById("<%=txt_lng.ClientID %>").value,
                function (responseText) {
                    document.getElementById("<%=tbx_Response.ClientID %>").value = responseText;
                });
        }

        //url: GET调用的地址，例如：jsontool.aspx?word=xxx
        //success: Ajax调用成功后相应的方法
        //error: Ajax调用失败后相应的方法
        function AjaxGetFunc(url,success,error)
        {
            var xmlhttp;
            if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
                xmlhttp = new XMLHttpRequest();
            }
            else {// code for IE6, IE5
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                    success(xmlhttp.responseText);
                }
                else {
                    error();
                }
            }
            xmlhttp.open("GET", url, true);
            xmlhttp.send();
        }
    </script>
</body>
</html>
