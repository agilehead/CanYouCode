<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DefaultViewModel>" %>
<script type="text/C#" runat="server">
    string AddErrors(IEnumerable<AgileFx.MVC.ViewModels.UIMessage> messages)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type=\"text/javascript\">");
        sb.Append("$(document).ready(function() {");
        sb.Append("var mBox = new AgileFx.MessageBox();");
        foreach (var msg in messages)
        {
            if (msg.Parameters != null)
                sb.Append("mBox.addError(" + JS.String(I18n.GetString(msg.Code, msg.Parameters)) + ");");
            else
                sb.Append("mBox.addError(" + JS.String(I18n.GetString(msg.Code)) + ");");
        }
        
        sb.Append("});    </" + "script>"); //don't confuse the IDE.
        return sb.ToString();
    }

    string AddMessage(string code)
    {
        if (code != null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\">");
            sb.Append("$(document).ready(function() {");
            sb.Append("new AgileFx.MessageBox().addMessage(" + JS.String(I18n.GetString(code)) + ");");
            sb.Append("});    </" + "script>"); //don't confuse the IDE.
            return sb.ToString();
        }
        else return "";
    } 
</script>

<%= AddErrors(Model.Errors) %>
<%= AddMessage(Model.MessageCode) %>

<ul id="errorList" style="display:none">
</ul>

<ul id="messageList" style="display:none">
</ul>