<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/javascript">
    function sendMessage() {
        $.ajax({
            url: "/Users/Contact",
            data: $("#ContactForm").serialize(),
            success: onAjaxComplete,
            async: false,
            type: 'POST'
        });
        $('#Contact').dialog('close');
    }

    function displayContactForm(recipientId) {
        $('#RecipientId').val(recipientId);
        $('#Contact').dialog({
            modal: true,
            autoOpen: false,
            width: 400,
            title: 'Send a Message'
        });
        $('#Contact').dialog('open');
    }
</script>
<div id="Contact" class="form standard" style="display: none">
    
    <form method="post" id="ContactForm">
    <input type="hidden" name="RecipientId" id="RecipientId" />
    <ul>
        <li>
            <label for="yourName">
                Your name</label>
              <%: Html.TextBox("SenderName", HttpContext.Current.User.Identity.Name,
                        new Validation("Your Name").Required())%>
        </li>
        <%if (!HttpContext.Current.User.Identity.IsAuthenticated)
          { %>
        <li>
            <label for="yourName">
                Your Email</label>
              <%: Html.TextBox("SenderEmail", null, new Validation("Your Email").Required())%>
        </li>
        <%} %>
        <li>
            <textarea name="Message" id="Message" rows="5" cols="36">
            </textarea>
        </li>
        <li>
            <input type="button" value="Send" onclick="sendMessage();" /></li>
    </ul>
    </form>
    
</div>


