<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% if (Context.User.IsInRole(ACCOUNT_TYPE.COMPANY))
   { %>
<script type="text/javascript">
    $(document).ready(function () {
        $("#ChangeStyle").change(function () {
            var style = $("#ChangeStyle option:selected").text();

            window.location = '/<%:Context.User.Identity.Name %>/ChangeStyle?style=' + style;
        });
    });
</script>

<div class="portfolioStyleSelect">

   <span class="publish">Change Style:</span>  <%:Html.DropDownList("ChangeStyle", PORTFOLIO_STYLE.GetAll().Select(s => new SelectListItem() { Text = s, Value = s, Selected = s == Model.Company.Style }))%>
</div>
<%} %>
