<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Spieler.aspx.cs" Inherits="Turnierverwaltung_final.View.Spieler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <section id="main-content">
        <section id="wrapper">
            <div class="row">
                <div class="col-lg-12">
                    <section class="panel">
                        <header class="panel-heading">
                            <div class="col-md-4 col-md-offset-4">
                                <h1>Spieler</h1>
                            </div>
                        </header>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Spieler Name" runat="server" />
                                        <asp:TextBox runat="server" Enabled="true" CssClass="form-control input-sm" placeholder="Spieler Name" />

                                    </div>
                                </div>
                            </div>
                        </div>

                    </section>
                </div>
            </div>
        </section>
    </section>
    <%--<asp:Table ID="tblFussballer" runat="server">
    </asp:Table>--%>
    <asp:Panel ID="Panel1" runat="server">
    </asp:Panel>
</asp:Content>
