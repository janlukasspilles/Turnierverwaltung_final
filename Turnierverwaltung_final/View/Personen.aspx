<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Personen.aspx.cs" Inherits="Turnierverwaltung.View.Personen" %>

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
    <div class="wrapper text-center">
        <div class="btn-group btn-group-toggle">
            <asp:Button runat="server" ID="btnTrainer" OnClick="OnTypeSelected" class="btn btn-dark" Text="Trainer" CommandArgument="Trainer"></asp:Button>
            <asp:Button runat="server" ID="btnPhysio" OnClick="OnTypeSelected" class="btn btn-dark" Text="Physio" CommandArgument="Physio"></asp:Button>
            <asp:Button runat="server" ID="btnFussballspieler" OnClick="OnTypeSelected" class="btn btn-dark" Text="Fussballspieler" CommandArgument="Fussballspieler"></asp:Button>
            <asp:Button runat="server" ID="btnHandballspieler" OnClick="OnTypeSelected" class="btn btn-dark" Text="Handballspieler" CommandArgument="Handballspieler"></asp:Button>
            <asp:Button runat="server" ID="btnTennisspieler" OnClick="OnTypeSelected" class="btn btn-dark" Text="Tennisspieler" CommandArgument="Tennisspieler"></asp:Button>
        </div>
    </div>
    <br />
    <br />
    <asp:Panel runat="server" ID="pnl_tbl"></asp:Panel>
</asp:Content>
