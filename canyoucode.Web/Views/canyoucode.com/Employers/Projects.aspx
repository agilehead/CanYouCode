<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Employers.Projects>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    My Projects: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content employerProjects">
        <a href="/<%:Context.User.Identity.Name %>/AddProject" style="position:absolute; margin-left: 760px;">
            <img src="/images/buttons/new-project.png" alt="new project" />
        </a>
        <ul class="tab full">
            <%if (Model.SelectedTab == PROJECT_STATUS.CLOSED)
              {%>
            <li><a href="/<%:Context.User.Identity.Name %>/Projects">Open</a></li>
            <li class="selected">Closed</li>
            <%}
              else
              { %>
            <li class="selected">Open</li>
            <li><a href="/<%:Context.User.Identity.Name %>/Projects?view=Closed">Closed</a></li>
            <%} %>
        </ul>
        <% if (Model.ProjectList.Count() > 0)
           { %>
        <ol class="projects bids">
            <%foreach (var project in Model.ProjectList)
              { %>
            <li class="project iconic">
                <h2>
                    <%: project.Title%></h2>
                <p class="addedOn">
                    <span class="subText">posted on:</span>
                    <%= project.DateAdded.GetFancyDate()%></p>
                <div class="description">
                    <p>
                        <%= project.GetShortDescription()%></p>
                    <p class="skills">
                        <%foreach (var tag in project.Tags)
                          { %>
                        <%=UIHelper.GetHTMLDisplayTag(tag, TAG_TYPE.PROJECTS)%>
                        <%} %>
                    </p>
                    <p class="detailsBar">
                        <a href="/Projects/<%= project.Id %>">
                            <img src="/images/buttons/see-all-bids.png" alt="see all bids" /></a></p>
                </div>
                <div class="bidInfo less">
                    <p>
                        Ends<br />
                        <span class="closingDate">
                            <%:project.ClosingDate.GetFancyDate()%></span></p>
                    <p>
                        Budget<br />
                        <span class="budget">
                            <%= project.GetBudget()%></span>
                    </p>
                </div>
                <% if (project.Bids.Count > 0)
                   { %>
                <div class="infoSnippets">
                    <h3>
                        Latests bids</h3>
                    <% var latestBids = project.Bids.Reverse().Take(6);
                       foreach (var bid in latestBids)
                       { %>
                    <p>
                        <%= bid.GetQuote()%><span class="subText"> by </span>
                        <%= bid.Company.Name%></p>
                    <% } %>
                    <a class="lineItem clear" href="/Projects/<%= project.Id %>">See all bids</a>
                </div>
                <% } %>
                <div class="clear">
                </div>
            </li>
            <%} %>
        </ol>
        <% }
           else
           { %>
        <p>
            There are no projects in here. Do you want to <a href="/<%:Context.User.Identity.Name %>/AddProject">
                create one</a>?
        </p>
        <%} %>
    </div>
</asp:Content>
