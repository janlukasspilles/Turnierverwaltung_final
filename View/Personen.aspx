<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Personen.aspx.cs" Inherits="Turnierverwaltung_final.View.Personen" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Personenverwaltung</h1>
    </div>
    <br />
    <br />
    <div class="btn-group">
        <asp:DropDownList CssClass="btn btn-primary dropdown-toggle" runat="server" ID="ddl_selection" AutoPostBack="true" OnSelectedIndexChanged="OnSelectionChanged">
            <asp:ListItem></asp:ListItem>
            <asp:ListItem>Alle</asp:ListItem>
            <asp:ListItem>Trainer</asp:ListItem>
            <asp:ListItem>Physio</asp:ListItem>
            <asp:ListItem>Fussballspieler</asp:ListItem>
            <asp:ListItem>Handballspieler</asp:ListItem>
            <asp:ListItem>Tennisspieler</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br />
    <br />
    <asp:Table runat="server" ID="tbl_personen" CssClass="table table-bordered"></asp:Table>
</asp:Content>
