<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Einstellungen.aspx.cs" Inherits="Turnierverwaltung_final.View.Einstellungen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Neue Sportart hinzufügen</h1>
    <h2>Sportart</h2>
    <asp:Panel runat="server">
        <section>
            <label>Bezeichnung</label>
            <asp:TextBox runat="server"></asp:TextBox>
        </section>
        <section>
            <label>Anzahl Punkte bei Sieg</label>
            <asp:TextBox runat="server"></asp:TextBox>
        </section>
        <section>
            <label>Anzahl Punkte bei Verlust</label>
            <asp:TextBox runat="server"></asp:TextBox>
        </section>
        <section>
            <label>Anzahl Punkte bei Unentschieden</label>
            <asp:TextBox runat="server"></asp:TextBox>
        </section>
        <section>
            <label>Anzahl Spieler pro Team</label>
            <asp:TextBox runat="server"></asp:TextBox>
        </section>
    </asp:Panel>
    <h2>Detailinformationen zu der Sportart</h2>
    <asp:Panel id="details" runat="server">
        <asp:Panel runat="server" ID="tbl_details">
            
        </asp:Panel>        
        <asp:Button runat="server" Text="Hinzufügen" ID="btn_add_property" OnClick="btn_add_property_Click"/>
    </asp:Panel>
</asp:Content>
