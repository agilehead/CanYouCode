<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Projects.ProjectView>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Project -
    <%:Model.Project.Title %>: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- This is the Bid Modal Window -->
    <script type="text/javascript">
        function displayBidControl(projectId, projectName) {
            $('#biddingProjectId').val(projectId);
            $('#bidControl').dialog({
                modal: true,
                autoOpen: false,
                width: 400,
                title: 'Place bid for project ' + projectName
            });
            $('#bidControl').dialog('open');
        };
    </script>
    <div id="bidControl" style="display: none">
        <div class="form standard packed modal">
            <form action="/<%= Model.LoggedInAccount.Username %>/PlaceBid" method="post" id="bidForm"
            name="bidForm" onsubmit="return validateForm('bidForm')">
            <input type="hidden" name="biddingProjectId" id="biddingProjectId" />
            <ul>
                <li>
                    <label for="Quote">
                        Quote</label>
                    <%:Html.DropDownList("Quote", UIHelper.GetBidQuoteSelectItemList(), new Validation("Quote"))%>
                    <%:I18NUtil.GetCurrencyChar(Model.Project.Currency) %></li>
                <li>
                    <label for="HoursOfEffort">
                        Hours of effort</label>
                    <%:Html.TextBox("HoursOfEffort", "", new { style = "width:100px;" }, new Validation("Hours of effort").Required())%>
                </li>
                <li>
                    <label for="TimeFrame">
                        Completion Timeframe</label>
                    <%:Html.DropDownList("TimeFrame", HtmlUtil.GetSelectItems(TIMEFRAME.GetAll(), x => x, x => x), new { style = "width:160px;" })%></li>
                <li>
                    <label for="Message">
                        Message</label>
                    <%:Html.TextArea("Message", "", new { rows = 4, cols = 30 }, new Validation("Message").MaximumLength(200))%>
                </li>
            </ul>
            <input type="submit" value="Place Bid" />
            </form>
        </div>
    </div>
    <!-- Bid Modal Box end -->
    <!-- Confirm Dialog Box -->
    <%Html.RenderPartial("~/Views/canyoucode.com/Shared/ConfirmDialog.ascx"); %>
    <!-- Confirm Dialog Box end -->
    <div class="content project">
        <%var project = Model.Project;
          var myBid = Model.Project.GetCompanyActiveBid(Model.LoggedInAccount.Id); %>
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
        <div class="bidInfo<% if (myBid != null) { %> more<% } else { %> less<% } %>">
            <p>
                Ends<br />
                <span class="closingDate">
                    <%:project.ClosingDate.GetFancyDate()%></span></p>
            <p>
                Budget<br />
                <span class="budget">
                    <%= project.GetBudget()%></span>
            </p>
            <% if (myBid != null)
               { %>
            <p>
                Your Bid
                <br />
                <span class="yourBid">
                    <%= myBid.GetQuote()%></span></p>
            <% } %>
            <%if (project.ClosingDate > DateTime.Now)
              { %>
            <p class="bidButton">
                <%  if (myBid != null)
                    { %>
                <img onclick="displayConfirmDialog('Withdraw Bid', '/<%= Model.LoggedInAccount.Username %>/WithdrawBid?bidId=<%=myBid.Id%>&returnUrl=/Projects/<%= project.Id%>', 'Withdraw Bid for project <%=project.Title%>');"
                    src="/images/buttons/withdraw-bid.png" class="likeButton" alt="withdraw bid" />
                <% }
                    else
                    { %>
                <a href="javascript:displayBidControl('<%:project.Id %>', '<%:project.Title %>');">
                    <img src="/images/buttons/bid-L.png" alt="bid" /></a>
                <%} %>
            </p>
            <%} %>
        </div>
        <div class="infoSnippets">
            <p>
                <% if (project.Bids.Count > 0)
                   { %>
                <%= project.Bids.Count%>
                others are bidding on this project.
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
        <h2>
            There are
            <%= project.Bids.Count %>
            companies bidding for this project.</h2>
        <ul class="companies">
            <% foreach (var bid in project.Bids)
               { %>
            <li>
                <div class="companyDetails">
                    <h3>
                        <a href="/<%= bid.Company.Account.Username %>" class="companyName">
                            <%= bid.Company.Name %></a></h3>
                    <span class="location">
                        <%= bid.Company.City%>,
                        <%= bid.Company.Country%></span>
                </div>
            </li>
            <% } %>
        </ul>
        <div class="clear">
        </div>
        <% }
           else
           { %>
        <h2>
            No one has bid for this project yet.</h2>
        <% } %>
    </div>
</asp:Content>
