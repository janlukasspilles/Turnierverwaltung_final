<%@ Page Title="Ranking" Language="C#" MasterPageFile="~/View/Site.Master" AutoEventWireup="true" CodeBehind="Ranking.aspx.cs" Inherits="Turnierverwaltung.View.Pages.Ranking" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Ranking</h1>
    </div>
    <br />
    <br />
    <asp:DropDownList runat="server" ID="ddlTurnier"></asp:DropDownList>
    <asp:Gridview runat="server" ID="gvRanking" />
</asp:Content>
