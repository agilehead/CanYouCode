<%@ Page Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.CanYouCodeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Page Not Found
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content about textual">
        <div style="width: 500px; height:500px; padding-right: 410px; background: url(/images/error.jpg) no-repeat 560px 0">
            <h1>
                Page Not Found</h1>
            <p>
                You step in the stream,
                <br />
                But the water has moved on.
                <br />
                This page is not here.
            </p>
            <p>
                You can go to the <a href="/">home page</a>, or <a href="javascript:displaySendMessage('<%:SEND_MESSAGE_TYPE.FEEDBACK %>', '')">
                    shout at us</a>.
            </p>
        </div>
    </div>
</asp:Content>
