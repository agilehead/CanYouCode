<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--<div class="fixedSideMenu">
        <a href="javascript:displaySendMessage('<%:SEND_MESSAGE_TYPE.INVITE %>', 'Check out http://canyoucode.com')">
        <img src="/images/send-invite.png" alt="Invite Friends" /></a><br />
    <a href="javascript:displaySendMessage('<%:SEND_MESSAGE_TYPE.FEEDBACK %>', '')">
        <img src="/images/feedback.png" alt="Feedback" /></a>
    
</div>--%>

<!-- AddThis Fixed-Positioned Toolbox -->
<%if (Model.ShowFixedSidePane)
  { %>
<script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js"></script>
<script type="text/javascript">
    var addthis_share = 
    { 
        url: 'http://canyoucode.com',
        templates: { twitter: 'check out {{url}}' }
    }
</script>
<div class="addthis_toolbox atfixed">   
    <div class="custom_images">
        <a class="addthis_button_twitter"><img src="/images/buttons/addthis/twitter_16.png" alt="Post on twitter"/></a>
        <a class="addthis_button_facebook"><img src="/images/buttons/addthis/facebook_16.png" alt="Post on facebook"/></a>
        <a class="addthis_button_linkedin"><img src="/images/buttons/addthis/linkedin_16.png" alt="Post on linkedin"/></a>
        <a class="addthis_button_stumbleupon"><img src="/images/buttons/addthis/stumbleupon_16.png" alt="Post on stumbleupon"/></a>
        <a class="addthis_button_compact"><img src="/images/buttons/addthis/other_16.png" alt="Post on other social media.."/></a>
    </div>
</div>
<%} %>
