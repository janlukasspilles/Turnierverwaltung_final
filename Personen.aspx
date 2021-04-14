<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Personen.aspx.cs" Inherits="Turnierverwaltung_final.View.Personen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server" ID="pnl_tbl"></asp:Panel>
    <%--<asp:UpdatePanel ID="pnl_tbl" runat="server">
        <ContentTemplate runat="server">--%>
            <%--<ContentTemplate>
            <asp:GridView ID="grdvUserInfo" AllowPaging="True" PagerSettings-Mode="NextPreviousFirstLast" PageSize="20" runat="server" AutoGenerateColumns="False" Width="700px" SkinID="grdMySite" DataSourceID="ObjectDataSource1">
                <Columns>
                    <asp:BoundField DataField="UserID" Visible="false" HeaderText="UserID" SortExpression="UserID" />
                    <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
                    <asp:BoundField DataField="ProviderName" HeaderText="ProviderName" SortExpression="ProviderName" />
                    <asp:BoundField DataField="IpAddress" HeaderText="IpAddress" SortExpression="IpAddress" />
                    <asp:BoundField DataField="TimeIn" HeaderText="TimeIn" SortExpression="TimeIn" />
                    <asp:BoundField DataField="TimeOUt" HeaderText="TimeOUt" SortExpression="TimeOUt" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>--%>
        <%--</ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAccept" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
