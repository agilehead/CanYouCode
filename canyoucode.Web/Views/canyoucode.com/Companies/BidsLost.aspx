<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Companies.Projects>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Bids Lost: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content">
        <ul class='tab full'>
            <li><a href='/<%= Model.LoggedInAccount.Username %>/Projects'>Active</a></li><li><a
                href='/<%= Model.LoggedInAccount.Username %>/Projects/Won'>Bids Won</a></li><li class='selected'>
                    <a href='/<%= Model.LoggedInAccount.Username %>/Projects/Lost'>Bids Lost</a></li></ul>
        <div class="clear">
        </div>
        <% if (Model.Bids.Count() > 0)
           { %>
        <div class="projects">
            <ol class="bids">
                <%foreach (var bid in Model.Bids)
                  { %>
                <li class="project">
                    <h2 class="title">
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
                        </p>
                    </div>
                    <div class="bidInfo more">
                        <p>
                            Expired On<br />
                            <span class="closingDate">
                                <%:bid.Project.ClosingDate.GetFancyDate()%></span></p>
                        <%if (bid.Project.WinningBid != null)
                          { %>
                        <p>
                            Winning Bid
                            <br />
                            <span class="winningBid">
                                <%= bid.Project.WinningBid.GetQuote()%></span>
                        </p>
                        <%} %>
                        <p>
                            Your Bid
                            <br />
                            <span class="yourBid">
                                <%= bid.GetQuote()%></span>
                        </p>
                    </div>
                    <div class="infoSnippets">
                        <p>
                            The project budget was
                            <%= bid.Project.GetBudget()%>.
                        </p>
                        <p>
                            <% if (bid.Project.TotalBids > 1)
                               { %>
                            <%= bid.Project.TotalBids - 1%>
                            others were bidding on this project.
                            <% }
                               else
                               { %>
                            You were the only bidder on this project.
                            <% } %>
                        </p>
                    </div>
                    <div class="clear">
                    </div>
                </li>
                <%} %>
            </ol>
            <div class="clear">
            </div>
        </div>
        <% }
           else
           { %>
        <p>
            There is nothing in here.</p>
        <%} %>
    </div>
</asp:Content>
