<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Projects.ProjectView>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Project -
    <%:Model.Project.Title %>: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function acceptBid(bidId, from, amount) {
            var accept = confirm("Accept this Bid from '" + from + "' for '" + amount + "' ?");
            if (accept == true) {
                window.location.href = '/<%= Model.LoggedInAccount.Username %>/AcceptBid?BidId=' + bidId;
            }
        }
    </script>
    <%Html.RenderPartial("~/Views/canyoucode.com/Shared/ConfirmDialog.ascx"); %>
    <div class="content project">
        <%var project = Model.Project;%>
        <h1>
            <%:project.Title %>
        </h1>
        <p class="addedOn">
            <span class="subText">posted on:</span>
            <%= project.DateAdded.GetFancyDate()%></p>
        <p class="postedBy">
            <span class="subText">posted by </span><a class="employer" href="/<%= project.Employer.Account.Username %>">
                <%= project.Employer.Name%></a><span class="employer location">
                    <%= project.Employer.City%>,
                    <%= project.Employer.Country%>
                </span>
        </p>
        <div class="description">
            <h3>
                Project Specification</h3>
            <div class="reset">
                <%= project.Description %>
            </div>
            <p class="skills">
                <%foreach (var tag in project.Tags)
                  { %>
                <%=UIHelper.GetHTMLDisplayTag(tag, TAG_TYPE.PROJECTS)%>
                <%} %>
            </p>
            <div class="clear">
            </div>
            <% if (project.Attachments.Count > 0)
               { %>
            <p class="attachments">
                <span class="subText">Attachments:</span>
                <% foreach (var attachment in project.Attachments)
                   { %>
                <a href="<%:attachment.Url%>">
                    <%:attachment.OriginalFileName%></a>
                <% } %>
            </p>
            <% } %>
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
        <div class="infoSnippets">
            <p>
                <% if (project.Bids.Count > 0)
                   { %>
                There are
                <%= project.Bids.Count%>
                bidders on this project.
                <% }
                   else
                   { %>
                There are no bidders for this project yet.
                <% } %>
            </p>
            <p>
                <%= project.Employer.Name%>
                has listed
                <%= project.Employer.TotalProjects %>
                projects to date.
            </p>
        </div>
        <div class="clear">
        </div>
        <% if (project.Bids.Count > 0)
           { %>
        <table class="companies">
            <tr>
                <th>
                </th>
                <th>
                    Company
                </th>
                <th>
                    Bid
                </th>
                <th>
                    Timeframe
                </th>
                <th>
                </th>
            </tr>
            <%foreach (var bid in project.Bids.OrderByDescending(b => b.DateCreated))
              {
                  var company = bid.Company; %>
            <tr>
                <td style="border-bottom: none">
                    <div class="logo">
                        <img src="<%:company.Logo %>" alt="<%= company.Name %>" />
                    </div>
                </td>
                <td>
                    <div class="companyDetails">
                        <h3>
                            <a href="/<%= bid.Company.Account.Username %>" class="companyName">
                                <%= bid.Company.Name %></a></h3>
                        <span class="location">
                            <%= bid.Company.City%>,
                            <%= bid.Company.Country%></span>
                        <% if (!string.IsNullOrWhiteSpace(bid.Message))
                           { %>
                        <p class="message">
                            <%= bid.Message%></p>
                        <% } %></div>
                </td>
                <td>
                    <%: bid.GetQuote()%>
                </td>
                <td>
                    <%= bid.Timeframe %>
                </td>
                <td>
                    <%if (project.Status != PROJECT_STATUS.CLOSED)
                      { %>
                    <a href="javascript:displayConfirmDialog('Accept Bid', '/<%= Model.LoggedInAccount.Username %>/AcceptBid?BidId=<%=bid.Id%>', 'Do you want to award this project to <strong><%=bid.Company.Name %></strong>?');">
                        <img src="/images/buttons/accept-bid.png" alt="accept bid" /></a>
                    <%}
                      else if (bid.Status == BID_STATUS.ACCEPTED)
                      {%>
                    <span>Accepted </span>
                    <%} %>
                </td>
            </tr>
            <% } %>
        </table>
        <% }
           else
           { %>
        <p>
            There are no bids for this project yet.
        </p>
        <% } %>
    </div>
</asp:Content>
