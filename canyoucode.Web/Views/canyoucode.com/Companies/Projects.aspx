<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Companies.Projects>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    My Projects: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content">
        <%Html.RenderPartial("~/Views/canyoucode.com/Shared/ConfirmDialog.ascx"); %>
        <ul class='tab full'>
            <li class='selected'><a href='/<%= Model.LoggedInAccount.Username %>/Projects'>Active</a></li><li>
                <a href='/<%= Model.LoggedInAccount.Username %>/Projects/Won'>Bids Won</a></li><li><a
                    href='/<%= Model.LoggedInAccount.Username %>/Projects/Lost'>Bids Lost</a></li></ul>
        <div class="clear">
        </div>
        <% if (Model.Invites.Count() > 0)
           { %>
        <ol class="invites">
            <%foreach (var invite in Model.Invites)
              { %>
            <li><span class="text">
                <%= invite.Project.Employer.Name %>
                has invited you to bid for <a href="/Projects/<%= invite.Project.Id %>">
                    <%: invite.Project.Title%></a>.</span> <a class="cancel" href="javascript:displayConfirmDialog('Decline Invite', '/<%= Model.LoggedInAccount.Username %>/DeclineInvite?projectId=<%=invite.ProjectId%>&returnUrl=/<%= Model.LoggedInAccount.Username %>/Projects', 'Decline Invite for project <%=invite.Project.Title%>');">
                        <img src="/images/cancel.png" alt="cancel" /></a><div class="clear">
                        </div>
            </li>
            <% } %></ol>
        <% } %>
        <% if (Model.Bids.Count() > 0)
           { %>
        <div class="projects">
            <ol class="bids">
                <%foreach (var bid in Model.Bids)
                  { %>
                <li class="project iconic">
                    <h2>
                        <%: bid.Project.Title%></h2>
                    <p class="addedOn">
                        <span class="subText">posted on:</span>
                        <%= bid.Project.DateAdded.GetFancyDate()%></p>
                    <p class="postedBy">
                        <span class="subText">posted by </span><a class="employer" href="/<%= bid.Project.Employer.Account.Username %>">
                            <%= bid.Project.Employer.Name%></a><span class="location">
                                <%= bid.Project.Employer.City%>,
                                <%= bid.Project.Employer.Country%>
                            </span>
                    </p>
                    <div class="description">
                        <p>
                            <%= bid.Project.GetShortDescription() %></p>
                        <p class="skills">
                            <%foreach (var tag in bid.Project.Tags)
                              { %>
                            <%=UIHelper.GetHTMLDisplayTag(tag, TAG_TYPE.PROJECTS)%>
                            <%} %>
                        </p>
                        <p class="detailsBar">
                            <a href="/Projects/<%= bid.Project.Id %>">
                                <img src="/images/buttons/see-details.png" alt="see details" /></a>
                            <img onclick="displayConfirmDialog('Withdraw Bid', '/<%= Model.LoggedInAccount.Username %>/WithdrawBid?bidId=<%=bid.Id%>&returnUrl=/<%= Model.LoggedInAccount.Username %>/Projects', 'Withdraw Bid for project <%=bid.Project.Title%>');"
                                src="/images/buttons/withdraw-bid.png" class="likeButton" alt="withdraw bid" /></p>
                    </div>
                    <div class="bidInfo more">
                        <p>
                            Ends<br />
                            <span class="closingDate">
                                <%:bid.Project.ClosingDate.GetFancyDate()%></span></p>
                        <p>
                            Budget<br />
                            <span class="budget">
                                <%= bid.Project.GetBudget()%></span>
                        </p>
                        <p>
                            Your Bid
                            <br />
                            <span class="yourBid">
                                <%= bid.GetQuote()%></span>
                        </p>
                    </div>
                    <div class="infoSnippets">
                        <p>
                            <% if (bid.Project.TotalBids > 1)
                               { %>
                            <%= bid.Project.TotalBids - 1%>
                            others are bidding on this project.
                            <% }
                               else
                               { %>
                            You are the only bidder on this project.
                            <% } %>
                        </p>
                        <p>
                            <%= bid.Project.Employer.Name%>
                            has listed
                            <%= bid.Project.Employer.TotalProjects%>
                            projects to date.
                        </p>
                    </div>
                    <div class="clear">
                    </div>
                </li>
                <%} %>
            </ol>
            <% }
           else
           {%>
            <span>You haven't bid for any projects. <a href="/Search/Work">Find projects</a> and
                place your bid.</span>
            <%} %>
            <div class="clear">
            </div>
        </div>
    </div>
</asp:Content>
