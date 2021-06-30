<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Personen.aspx.cs" Inherits="Turnierverwaltung_final.View.Personen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.datepicker-field').datepicker();
        });
    </script>
    <div class="jumbotron">
        <h1>Personenverwaltung</h1>
    </div>
    <br />
    <br />
    <div class="btn-group btn-group-toggle">
        <asp:Button runat="server" ID="btnTrainer" OnClick="OnTypeSelected" type="button" class="btn btn-primary" Text="Trainer" CommandArgument="Trainer"></asp:Button>
        <asp:Button runat="server" ID="btnPhysio" OnClick="OnTypeSelected" class="btn btn-primary" Text="Physio" CommandArgument="Physio"></asp:Button>
        <asp:Button runat="server" ID="btnFussballspieler" OnClick="OnTypeSelected" class="btn btn-primary" Text="Fussballspieler" CommandArgument="Fussballspieler"></asp:Button>
        <asp:Button runat="server" ID="btnHandballspieler" OnClick="OnTypeSelected" class="btn btn-primary" Text="Handballspieler" CommandArgument="Handballspieler"></asp:Button>
        <asp:Button runat="server" ID="btnTennisspieler" OnClick="OnTypeSelected" class="btn btn-primary" Text="Tennisspieler" CommandArgument="Tennisspieler"></asp:Button>
    </div>
    <br />
    <br />
    <asp:Panel runat="server" ID="pnl_tbl"></asp:Panel>
</asp:Content>
