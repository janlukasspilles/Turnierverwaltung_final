<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Personen.aspx.cs" Inherits="Turnierverwaltung_final.View.Personen" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Personenverwaltung</h1>
    </div>
    <br />
    <br />

    <div class="btn-group">
        <asp:DropDownList CssClass="btn btn-primary dropdown-toggle" runat="server" ID="ddl_selection" OnSelectedIndexChanged="ddl_selection_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem runat="server" ID="li_Empty"></asp:ListItem>
            <asp:ListItem runat="server" ID="li_Alle">Alle</asp:ListItem>
            <asp:ListItem runat="server" ID="li_Trainer">Trainer</asp:ListItem>
            <asp:ListItem runat="server" ID="li_Physio">Physio</asp:ListItem>
            <asp:ListItem runat="server" ID="li_Fussballspieler">Fussballspieler</asp:ListItem>
            <asp:ListItem runat="server" ID="li_Handballspieler">Handballspieler</asp:ListItem>
            <asp:ListItem runat="server" ID="li_Tennisspieler">Tennisspieler</asp:ListItem>
        </asp:DropDownList> 
    </div>
 
    <br />
    <br />
    <asp:Panel runat="server" ID="pnl_tbl"></asp:Panel>
</asp:Content>
