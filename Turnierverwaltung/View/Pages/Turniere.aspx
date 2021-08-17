<%@ Page Title="Turniere" Language="C#" MasterPageFile="~/View/Site.Master" AutoEventWireup="true" CodeBehind="Turniere.aspx.cs" Inherits="Turnierverwaltung.View.Pages.Turniere" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Turnierverwaltung</h1>
    </div>
    <br />
    <br />
    <asp:Panel runat="server" ID="pnl_turniere"></asp:Panel>
    <asp:Panel runat="server" ID="pnl_teilnehmer"></asp:Panel>
</asp:Content>
