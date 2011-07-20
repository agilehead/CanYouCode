<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/C#" runat="server">
    public bool IsInPreviewMode()
    {
        return Request.Cookies["PreviewMode"] != null;
    }
</script>
<script type="text/javascript">
    $(document).ready(function () {
        setTimeout('showActivationTip()', 10000);
    });

    function showActivationTip() {
        function toolTipParams(content) {
            return {
                content: {
                    text: content, 
                    title: {
                        text: 'Activate Account',
                        button: 'X'
                    }
                },
                position: {
                    corner: {
                        target: TipCorners.BL,
                        tooltip: TipCorners.TR
                    }
                },
                style: {
                    border: {
                        width: 5,
                        radius: 10
                    },
                    padding: 10,
                    textAlign: 'center',
                    tip: true, // Give it a speech bubble tip with automatic corner detection
                    name: 'green' // Style it according to the preset 'cream' style
                },
                show: { ready: true }, 
                hide: false
            }
        }

        var tTip = toolTipParams('Your profile will not be listed until you activate your account.');
        $('.activateAccount').qtip(tTip);
    }

</script>
<% if (IsInPreviewMode())
   { %>
<div class="previewHeader">
    <p>
        To enable your account, you should <a style="color: #0F0" href="/Companies/ActivateAccount?Key=<%= Request.Cookies["PreviewMode"].Value %>">
            ACTIVATE IT</a>.
    </p>
    <a class="activateAccount" href="/Companies/ActivateAccount?Key=<%= Request.Cookies["PreviewMode"].Value %>">
        <img src="/images/buttons/activate-account.png" alt="Activate Account" /></a>
    <a style="margin-right: 40px" href="javascript:displayConfirmDialog('Delete Account', '/Companies/DeleteAccount?Key=<%= Request.Cookies["PreviewMode"].Value %>', 'Are you sure you want to remove your account?');">
        <img src="/images/buttons/delete-account.png" alt="Delete Account" /></a>
</div>
<div class="previewHeaderSpace">
</div>
<%Html.RenderPartial("~/Views/canyoucode.com/Shared/ConfirmDialog.ascx"); %>
<% } %>