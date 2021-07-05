<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mannschaften.aspx.cs" Inherits="Turnierverwaltung.View.Mannschaften" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Mannschaftsverwaltung</h1>
    </div>
    <br />
    <br />
    <asp:Panel runat="server" ID="pnl_tbl"></asp:Panel>
    <asp:Panel runat="server" ID="pnl_mitglieder"></asp:Panel>
</asp:Content>
