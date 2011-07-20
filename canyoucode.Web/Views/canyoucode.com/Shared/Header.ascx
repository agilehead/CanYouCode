<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CanYouCodeViewModel>" %>
<script type="text/C#" runat="server">
    public string IsSelected(params TopMenuSelection[] menuItems)
    {
        return menuItems.Any(i => Model.TopMenuSelectedItem == i) ?
                    " class=\"selected\"" : "";
    }    
</script>
<a href="/" class="logo">
    <img src="/images/logo.png" alt="Can you code?" /></a>
<ul>
    <% if (Context.User.IsInRole(ACCOUNT_TYPE.COMPANY))
       { %>
    <li<%= IsSelected(TopMenuSelection.COMPANIES_PROJECTS) %>><a href="/<%= Context.User.Identity.Name %>/Projects">
        <img src="/images/menu-projects.png" alt="My Projects" /></a></li>
    <li<%= IsSelected(TopMenuSelection.SEARCH_WORK) %>><a href="/Search/Work">
        <img src="/images/find-work.png" alt="Find work" /></a></li>
    <li<%= IsSelected(TopMenuSelection.COMPANIES_PROFILE_VIEW, TopMenuSelection.COMPANIES_PROFILE_EDIT) %>><a href="/<%= Context.User.Identity.Name %>">
        <img src="/images/menu-profile.png" alt="Profile" /></a></li>
    <% }
       else if (Context.User.IsInRole(ACCOUNT_TYPE.EMPLOYER))
       { %>
    <li<%= IsSelected(TopMenuSelection.EMPLOYERS_PROJECTS) %>><a href="/<%:Context.User.Identity.Name %>/Projects">
        <img src="/images/menu-projects.png" alt="My Projects" /></a></li>
    <li<%= IsSelected(TopMenuSelection.SEARCH_COMPANIES) %>><a href="/Search/Experts">
        <img src="/images/find-experts.png" alt="Find Experts" /></a></li>
    <li<%= IsSelected(TopMenuSelection.EMPLOYERS_PROFILE_VIEW, TopMenuSelection.EMPLOYERS_PROFILE_EDIT) %>><a href="/<%:Context.User.Identity.Name %>">
        <img src="/images/menu-profile.png" alt="My Profile" /></a></li>
   <% }
       else if (Context.User.IsInRole(ACCOUNT_TYPE.ADMIN))
       { %>
       <!-- There are currently no menu items for Admin -->
    <% }
       else
       { %>
    <li<%= IsSelected(TopMenuSelection.SEARCH_COMPANIES) %>><a href="/Search/Experts">
        <img src="/images/menu-hire.png" alt="Hire people" /></a></li>
    <li<%= IsSelected(TopMenuSelection.SEARCH_WORK) %>><a href="/Search/Work">
        <img src="/images/menu-work.png" alt="Find work" /></a></li>
    <li style="margin-left: 180px; background: none;"><a href="/login">
        <img src="/images/menu-login.png" alt="Login" /></a></li>
    <% } %>
</ul>
<% if (Context.User.Identity.IsAuthenticated)
   { %>
<div class="logoutLink">
    <a href="/logout">logout</a>
    <%= Context.User.Identity.Name %>
</div>
<% } %>