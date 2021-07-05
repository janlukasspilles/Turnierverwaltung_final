<%@ Page Title="Mannschaftsverwaltung" Language="C#" MasterPageFile="~/View/Site.Master" AutoEventWireup="true" CodeBehind="Mannschaften.aspx.cs" Inherits="Turnierverwaltung.View.Pages.Mannschaften" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Mannschaftsverwaltung</h1>
    </div>
    <br />
    <br />
    <asp:Panel runat="server" ID="pnl_tbl"></asp:Panel>
    <asp:Panel runat="server" ID="pnl_mitglieder"></asp:Panel>
</asp:Content>
