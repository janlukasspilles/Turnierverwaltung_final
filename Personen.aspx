<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Personen.aspx.cs" Inherits="Turnierverwaltung_final.View.Personen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:DropDownList runat="server" ID="ddl_selection" OnSelectedIndexChanged="ddl_selection_SelectedIndexChanged" AutoPostBack="true">
        <asp:ListItem runat="server" ID="li_Empty"></asp:ListItem>
        <asp:ListItem runat="server" ID="li_Personen">Personen</asp:ListItem>
        <asp:ListItem runat="server" ID="li_Spieler">Spieler</asp:ListItem>
        <asp:ListItem runat="server" ID="li_Trainer">Trainer</asp:ListItem>
        <asp:ListItem runat="server" ID="li_Physio">Physio</asp:ListItem>
    </asp:DropDownList>
    <asp:Panel runat="server" ID="pnl_tbl"></asp:Panel>    
</asp:Content>
