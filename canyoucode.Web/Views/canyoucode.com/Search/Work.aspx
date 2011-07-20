<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<canyoucode.Web.ViewModels.Search.Work>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Find Work: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content search">
        <%Html.RenderPartial("~/Views/canyoucode.com/Shared/SearchBar.ascx", Model); %>
        <ol class="projects results">
            <%foreach (var project in Model.Projects)
              { %>
            <li class="project simpleView">
                <h2>
                    <%: project.Title%>
                    <%if (project.ClosingDate <= DateTime.Now)
                      { %>[Closed]<%} %>
                </h2>
                <p class="addedOn">
                    <span class="subText">posted on:</span>
                    <%= project.DateAdded.GetFancyDate()%></p>
                <p class="postedBy">
                    <span class="subText">posted by </span>
                    <%if (User.Identity.IsAuthenticated)
                      { %>
                    <a class="employer" href="/<%= project.Employer.Account.Username %>">
                        <%= project.Employer.Name%></a>
                    <%}
                      else
                      { %>
                    <span class="employer">[hidden]</span>
                    <%} %>
                    <span class="location">
                        <%= project.Employer.City%>,
                        <%= project.Employer.Country%>
                    </span>
                </p>
                <div class="description">
                    <p>
                        <%= project.GetShortDescription() %></p>
                    <p class="skills">
                        <%foreach (var tag in project.Tags)
                          { %>
                        <%=UIHelper.GetHTMLDisplayTag(tag, TAG_TYPE.PROJECTS)%>
                        <%} %>
                    </p>
                    <p class="detailsBar">
                        <%  Bid companybid = Model.LoggedInAccount != null ? project.GetCompanyActiveBid(Model.LoggedInAccount.Id) : null;
                            if (companybid != null)
                            { %>
                        <a href="/Projects/<%= project.Id %>">
                            <img src="/images/buttons/see-details.png" alt="see details" /></a>
                        <img onclick="displayConfirmDialog('Withdraw Bid', '/<%= Model.LoggedInAccount.Username %>/WithdrawBid?bidId=<%=companybid.Id%>&returnUrl=/<%= Model.LoggedInAccount.Username %>/Projects', 'Withdraw Bid for project <%=companybid.Project.Title%>');"
                            src="/images/buttons/withdraw-bid.png" class="likeButton" alt="withdraw bid" />
                        <%}
                            else
                            {%>
                        <a href="/Projects/<%= project.Id %>">
                            <%if (project.ClosingDate <= DateTime.Now)
                              { %>
                            <img src="/images/buttons/see-details.png" alt="see details and bid" />
                            <%}
                              else
                              { %>
                            <img src="/images/buttons/see-details-and-bid.png" alt="see details and bid" />
                            <%} %>
                        </a>
                        <%} %>
                    </p>
                </div>
                <div class="bidInfo<%if(companybid != null){ %> more<%} else {%> less<% } %>">
                    <p>
                        Ends<br />
                        <span class="closingDate">
                            <%:project.ClosingDate.GetFancyDate()%></span></p>
                    <p>
                        Budget<br />
                        <span class="budget">
                            <%= project.GetBudget()%></span>
                    </p>
                    <%if (companybid != null)
                      { %>
                    <p>
                        Your Bid
                        <br />
                        <span class="yourBid">
                            <%= companybid.GetQuote()%></span>
                    </p>
                    <%} %>
                </div>
                <div class="clear">
                </div>
            </li>
            <%} %>
        </ol>
        <%if (Model.Projects.Count() <= 0)
          { %>
        <div class="results projects">
            <span>No Results.</span>
        </div>
        <%} %>
        <div class="clear">
        </div>
    </div>
</asp:Content>
