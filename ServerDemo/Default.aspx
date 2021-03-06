﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ServerDemo._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>服务接口测试DEMO</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center; font-size: 20px; font-weight: bold;">接口测试用例</div>
        <br /><hr /><br />
        <div>
            获得MD5：
            <asp:TextBox ID="txt_MD5" runat="server" Width="377px"></asp:TextBox>&nbsp;
            <asp:Button ID="btn_MD5" runat="server" Text="获得MD5" OnClick="btn_MD5_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_SendTest" runat="server" Text="测试" OnClick="btn_SendTest_Click" />
        </div>
        <br /><hr /><br />
        <div>
            <table>
                <tr>
                    <td><label >RequestStr(请求串)</label></td>
                    <td><label >ResponseStr(响应串)</label></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt_Request" runat="server" Height="400px" 
                        TextMode="MultiLine" Width="569px" Style="margin-top: 0px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Response" runat="server" Height="400px" 
                        TextMode="MultiLine" Width="569px" Style="margin-top: 0px"></asp:TextBox>
                    </td>
                </tr>
            
            </table>
        </div>
    </form>
</body>
</html>
