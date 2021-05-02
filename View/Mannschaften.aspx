<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mannschaften.aspx.cs" Inherits="Turnierverwaltung_final.View.Mannschaften" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Mannschaftsverwaltung</h1>
    </div>
    <br />
    <br />
    <asp:Table runat="server" ID="tbl_mannschaften" CssClass="table table-bordered"></asp:Table>
    <asp:Panel runat="server" ID="mitglieder_switch">
        <h2>Mitglieder der Mannschaft</h2>
        <div class="row">
            <div class="border row col-md-6 boxlayout">
                <div class="border col-md-3">
                    <asp:ListBox runat="server" ID="lbMitglieder"></asp:ListBox>
                </div>
                <div class="border col-md-3">
                    <asp:Button runat="server" ID="btnAdd" Text="Add" />
                    <asp:Button runat="server" ID="btnRemove" Text="Remove" />
                </div>
                <div class="border col-md-3">
                    <asp:ListBox runat="server" ID="lbMoeglicheMitglieder"></asp:ListBox>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
