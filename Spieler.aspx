<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Spieler.aspx.cs" Inherits="Turnierverwaltung_final.View.Spieler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <asp:DropDownList ID="ddl_selection" runat="server" OnSelectedIndexChanged="ddl_selection_SelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem Selected="True" Value="Alle"> Alle </asp:ListItem>
            <asp:ListItem Value="Fussballspieler"> Fussballspieler </asp:ListItem>
            <asp:ListItem Value="Handballspieler"> Handballspieler </asp:ListItem>
            <asp:ListItem Value="Tennisspieler"> Tennisspieler </asp:ListItem>
        </asp:DropDownList>
    </div>

    <asp:Panel ID="pnl_tbl" runat="server">
    </asp:Panel>
</asp:Content>
