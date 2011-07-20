<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CanYouCodeViewModel>" %>
<script type="text/javascript">
    function closeAlert() {
        $(".alerts").slideUp();
    }
</script>
<%if (!string.IsNullOrEmpty(Model.AccountAlert))
  {%>
<div class="alerts">
    <%= Model.AccountAlert%>
    <a class="close" href="javascript:closeAlert();">
        <img src="/images/alerts-close.png" alt="close" /></a>
    <div class="clear"></div>
</div>
<%} %>
