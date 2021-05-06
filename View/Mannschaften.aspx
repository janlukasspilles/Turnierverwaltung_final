<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mannschaften.aspx.cs" Inherits="Turnierverwaltung_final.View.Mannschaften" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Mannschaftsverwaltung</h1>
    </div>
    <br />
    <br />
    <asp:Table runat="server" ID="tbl_mannschaften" CssClass="table table-bordered"></asp:Table>
    <asp:Panel runat="server" ID="pnl_mitglieder"></asp:Panel>
    <%--<asp:Panel runat="server" ID="mitglieder_switch">
        <h2>Mitglieder der Mannschaft</h2>
        <div class="row">
            <div class="border row col-md-8 boxlayout form-inline">
                <div class="border col-md-3">
                    <asp:ListBox runat="server" ID="lbMitglieder" CssClass="form-control"></asp:ListBox>
                </div>
                <div class="border col-md-2">
                    <asp:Button runat="server" ID="btnAdd" Text="Add" CssClass="btn" />
                    <asp:Button runat="server" ID="btnRemove" Text="Remove" CssClass="btn" />
                </div>
                <div class="border col-md-3">
                    <asp:ListBox runat="server" ID="lbMoeglicheMitglieder" CssClass="form-control"></asp:ListBox>
                </div>
            </div>
        </div>
    </asp:Panel>--%>
</asp:Content>
