<%@ Page Title="Spieleverwaltung" Language="C#" MasterPageFile="~/View/Site.Master" AutoEventWireup="true" CodeBehind="Spieleverwaltung.aspx.cs" Inherits="Turnierverwaltung.View.Pages.Spieleverwaltung" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Spieleverwaltung</h1>
    </div>
    <br />
    <br />
    <asp:Panel runat="server" ID="pnl_head">
        <asp:DropDownList runat="server" ID="ddlTurnierauswahl" AutoPostBack="true" OnSelectedIndexChanged="ddlTurnierauswahl_SelectedIndexChanged"></asp:DropDownList>
        <%--<asp:Button runat="server" ID="btnDurchfuehren" Text="Turnier durchführen" />--%>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_tbl">

    </asp:Panel>

</asp:Content>
