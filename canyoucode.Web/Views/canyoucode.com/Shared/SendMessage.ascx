<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/javascript">
    function sendMessage() {
        $.ajax({
            url: "/SendMessage",
            data: $("#SendMessageForm").serialize(),
            success: onAjaxComplete,
            async: false,
            type: 'POST'
        });
        $('#sendMessageBox').dialog('close');
    }

    function displaySendMessage(type, message) {
        $('#Type').val(type);
        $('#MessageText').val(message);
        $('#sendMessageBox').dialog({
            modal: true,
            autoOpen: false,
            width: 400,
            title: type
        });
        $('#sendMessageBox').dialog('open');

        if (type == '<%=SEND_MESSAGE_TYPE.INVITE%>') {
            $('.recipient').show();
            $('#MessageArea').hide();
        } else {
            $('.recipient').hide();
            $('#MessageArea').show();
        }
    }
</script>
<div id="sendMessageBox" class="form standard" style="display: none">
    <form method="post" id="SendMessageForm" action="/SendMessage">
    <input type="hidden" name="Type" id="Type" />
    <ul>
        <li>
            <label for="yourName">
                Your name</label>
            <%: Html.TextBox("YourName", (HttpContext.Current.User.Identity.IsAuthenticated ? HttpContext.Current.User.Identity.Name: null),
                        new Validation("Your Name").Required())%>
        </li>
        <li class="recipient">
            <label for="yourName">
                Recipient Name</label>
            <%: Html.TextBox("RecipientName", new Validation("Recipient Name").Required())%><label
                for="yourName">
                Recipient Email</label>
            <%: Html.TextBox("RecipientEmail", new Validation("Recipient Email").Email().Required())%>
        </li>
        <li id="MessageArea">
            <p>
                What would you like to tell us?</p>
            <textarea name="MessageText" id="MessageText" rows="5" cols="36">
        </textarea>
        </li>
        <li>
            <input type="button" value="Send" onclick="sendMessage();" /></li>
    </ul>
    </form>
</div>
