<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Turnierverwaltung_final.View.View" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Turnierverwaltung</h1>
            <table>
                <%--<thead>
                    <tr>
                        <%foreach (var p in Properties)
                            {%>
                        <th><%=p %></th>
                        <%} %>
                    </tr>
                </thead>--%>
                <tbody>
                    <%foreach (var t in Teilnehmer)
                        {%>
                    <tr>
                        <%foreach (string s in t.GetInformation().Split(';'))
                            { %>
                        <td><%=s  %></td>
                        <%} %>
                    </tr>
                    <%} %>
                </tbody>
            </table>
            <asp:Button ID="btn_Refresh" runat="server" Text="Refresh" OnClick="btn_Refresh_Click" />
        </div>
        <p>
            <asp:Label ID="Label1" runat="server" Text="Vorname"></asp:Label>
            <asp:TextBox ID="txt_Vorname" runat="server"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label5" runat="server" Text="Nachname"></asp:Label>
            <asp:TextBox ID="txt_Nachname" runat="server"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label4" runat="server" Text="Geburtstag"></asp:Label>
            <asp:TextBox ID="txt_Geburtstag" runat="server"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label3" runat="server" Text="Mannschaft"></asp:Label>
            <asp:TextBox ID="txt_Mannschaft" runat="server"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="Label2" runat="server" Text="Anzahl Tore"></asp:Label>
            <asp:TextBox ID="txt_AnzahlTore" runat="server"></asp:TextBox>
        </p>
        <asp:Button ID="btn_Insert" runat="server" Text="Neuanlage" OnClick="btn_Insert_Click" />
    </form>
</body>
</html>
