<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/javascript">

        var addthis_share = 
        { 
            url: 'http://canyoucode.com/<%= Model.Company.Account.Username %>',
            templates: {
                           twitter: 'check out my portfolio {{url}}',
                       }
        }
</script>
<script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#username=agilehead"></script>
<% if (Context.User.IsInRole(ACCOUNT_TYPE.COMPANY))
   { %>
<div class="addthis_toolbox addthis_32x32_style addthis_default_style">
    <span class="publish"> share portfolio on </span>
    <a class="addthis_button_twitter"><img src="/images/buttons/addthis/twitter_24.png" alt="share on twitter"/></a>
    <a class="addthis_button_facebook"><img src="/images/buttons/addthis/facebook_24.png" alt="share on facebook"/></a>
    <a class="addthis_button_linkedin"><img src="/images/buttons/addthis/linkedin_24.png" alt="share on linkedin"/></a>
    <a class="addthis_button_stumbleupon"><img src="/images/buttons/addthis/stumbleupon_24.png" alt="share on stumbleupon"/></a>
    <a class="addthis_button_compact"><img src="/images/buttons/addthis/other_24.png" alt="share on other social media.."/></a>
</div>
<%} %>
