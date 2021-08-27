<%@ Page Title="Anmeldung" Language="C#" MasterPageFile="~/View/Site.Master" AutoEventWireup="true" CodeBehind="Anmeldung.aspx.cs" Inherits="Turnierverwaltung.View.Pages.Anmeldung" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server" ID="pnlLogin">
        <asp:TextBox runat="server" ID="txtBenutzername"></asp:TextBox>
        <asp:TextBox runat="server" ID="txtPasswort"></asp:TextBox>
        <asp:Button runat="server" ID="btnLogin"/>
    </asp:Panel>
</asp:Content>
