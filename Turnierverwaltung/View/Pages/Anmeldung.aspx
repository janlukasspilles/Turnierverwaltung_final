<%@ Page Title="Anmeldung" Language="C#" MasterPageFile="~/View/Site.Master" AutoEventWireup="true" CodeBehind="Anmeldung.aspx.cs" Inherits="Turnierverwaltung.View.Pages.Anmeldung" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Login ID="LoginCtrl" runat="server" DestinationPageUrl="Startseite.aspx" OnAuthenticate="LoginCtrl_Authenticate"></asp:Login>
</asp:Content>
