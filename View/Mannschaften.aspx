<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mannschaften.aspx.cs" Inherits="Turnierverwaltung_final.View.Mannschaften" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Mannschaftsverwaltung</h1>
    </div>
    <br />
    <br />
    <asp:Table runat="server" ID="tbl_mannschaften" CssClass="table table-bordered"></asp:Table>
    <asp:Panel runat="server" ID="pnl_mitglieder"></asp:Panel>
</asp:Content>
