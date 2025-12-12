<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BW-StarkDetail.ascx.cs" Inherits="RockWeb.Blocks.Utility.BW_StarkDetail" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">
        
            <div class="panel-heading">
                <h1 class="panel-title">
                    <i class="ti ti-star"></i> 
                    Group Detail
                </h1>
            </div>
            <div class="panel-body">

                <div class="alert alert-info">
                    <asp:Literal ID="ltName" runat="server"></asp:Literal><br />
                    <asp:Literal ID="ltDescription" runat="server"></asp:Literal><br />
                    <asp:Literal ID="ltDateCreated" runat="server"></asp:Literal><br />
                    <asp:Literal ID="ltDateModified" runat="server"></asp:Literal><br />
                    <asp:Literal ID="ltCapacity" runat="server"></asp:Literal><br />
                </div>
            </div>
        
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>