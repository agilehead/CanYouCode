<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Projects.ProjectView>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Project -
    <%:Model.Project.Title %>: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content project">
        <%var project = Model.Project;%>
        <h1>
            <%:project.Title %>
            <%if (project.ClosingDate <= DateTime.Now)
              { %>[Closed]<%} %>
        </h1>
        <p class="addedOn">
            <span class="subText">posted on:</span>
            <%= project.DateAdded.GetFancyDate()%></p>
        <p class="postedBy">
            <span class="subText">posted by </span><span class="employer">[hidden]</span><span
                class="employer location">
                <%= project.Employer.City%>,
                <%= project.Employer.Country%>
            </span>
        </p>
        <div class="description">
            <h3>
                Project Specification</h3>
            <div>
                <%= project.GetShortDescription()%>
            </div>
            <p class="skills">
                <%foreach (var tag in project.Tags)
                  { %>
                <%=UIHelper.GetHTMLDisplayTag(tag, TAG_TYPE.PROJECTS)%>
                <%} %>
            </p>
            <div class="clear">
            </div>
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
            <%if (project.ClosingDate > DateTime.Now)
              { %>
            <p class="bidButton">
                <a href="/Login?ReturnUrl=/Projects/<%:project.Id %>">
                    <img src="/images/buttons/login-bid.png" alt="bid" /></a>
            </p>
            <div class="clear">
            </div>
            <%} %>
        </div>
        <div class="infoSnippets">
            <p>
                <% if (project.Bids.Count > 0)
                   { %>
                <%= project.Bids.Count %>
                companies are bidding on this project.
                <% }
                   else
                   { %>
                There are no bidders for this project yet.
                <% } %>
            </p>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
