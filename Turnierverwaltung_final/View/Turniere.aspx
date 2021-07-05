<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Turniere.aspx.cs" Inherits="Turnierverwaltung.View.Turniere" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Turnierverwaltung</h1>
    </div>
    <br />
    <br />
    <asp:Table runat="server" ID="tbl_turniere" CssClass="table table-bordered"></asp:Table>
    <asp:Panel runat="server" ID="pnl_spiele"></asp:Panel>
</asp:Content>
