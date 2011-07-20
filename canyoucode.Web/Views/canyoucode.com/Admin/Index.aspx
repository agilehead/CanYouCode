<%@ Page Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.CanYouCodeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Administrative Console: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content admin">
        <h1>Admin Console</h1>
        <ul class="options">
            <li>
                <img src="/images/admin/accounts.png" alt="Create Accounts for Marketing" />
                <p>
                    <a href="/Admin/CreateMarketingAccounts">Create accounts for Marketing</a></p>
            </li>
            <li>
                <img src="/images/admin/accounts.png" alt="Send Alerts" />
                <p>
                    <a href="/Admin/SendAlerts">Send Alerts</a></p>
            </li>
        </ul>
        <div class="clear">
        </div>
    </div>
</asp:Content>
