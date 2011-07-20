<%@ Page Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Companies.ViewItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Model.Company.Name %>'s Profile
</asp:Content>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <link href="/Styles/portfolio/zurb_orbit/zurb_orbit.css" rel="stylesheet" type="text/css" />
    <script src="/Styles/portfolio/zurb_orbit/jquery.orbit.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/Styles/portfolio/zurb_orbit/orbit.css" type="text/css" />
    <!--[if IE]>
         <style type="text/css">
             .timer { display: none !important; }
             div.caption { background:transparent; filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#99000000,endColorstr=#99000000);zoom: 1; }
        </style>
    <![endif]-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/C#" runat="server">
        public bool IsEditable()
        {
            return Model.LoggedInAccount != null && Model.LoggedInAccount.Type == ACCOUNT_TYPE.COMPANY
                && Model.LoggedInAccount.Username == Model.Company.Account.Username;
        }
    </script>
    <script type="text/javascript">
        function setPage(pageNumber) {
            $('.page').hide();
            $('#page' + pageNumber).slideDown();
            $('.pageLink a').removeClass('selected');
            $('#pageLink' + pageNumber + " a").addClass('selected');
        }

        $(document).ready(function () { setPage(1) });

        $(window).load(function () {
            $('#featured').orbit({
                animation: 'horizontal-slide', //fade, horizontal-slide, vertical-slide
                animationSpeed: 800, //how fast animations are
                advanceSpeed: 4000, //if timer advance is enabled, time between transitions 
                startClockOnMouseOut: true, //if timer should restart on MouseOut
                startClockOnMouseOutAfter: 3000, //how long after mouseout timer should start again
                directionalNav: true, //manual advancing directional navs
                captions: true, //if has a title, will be placed at bottom
                captionAnimationSpeed: 800, //how quickly to animate in caption on load and between captioned and uncaptioned photos
                timer: true, //if the circular timer is wanted
                bullets: true //true or false to activate the bullet navigation
            });
        });

    </script>
    <% Html.RenderPartial("~/Views/canyoucode.com/Companies/Contact.ascx"); %>
    <div class="content portfolio <% if (IsEditable()) { %>edit<% } %>">
        <% if (IsEditable())
           { %>
        <%Html.RenderPartial("~/Views/canyoucode.com/Shared/PublishPortfolio.ascx"); %>
        <%Html.RenderPartial("~/Views/canyoucode.com/Companies/SelectPortfolioStyle.ascx"); %>
        <ul class="tab full">
            <li class="selected">View</li>
            <li><a href="/<%= Model.LoggedInAccount.Username %>/Edit">Edit</a></li>
        </ul>
        <% } %>
        <div class="clear">
        </div>
        <div class="narrowLeft" style="display: none">
            <div class="companyHeader">
                <div class="logo">
                    <img src="<%= Model.Company.Logo_80 %>" alt="<%= Model.Company.Name %>" /></div>
                <div class="details">
                    <h1>
                        <%= Model.Company.Name %>
                    </h1>
                    <p class="location">
                        <%= Model.Company.City %>,
                        <%= Model.Company.Country %>
                    </p>
                </div>
            </div>
            <div class="companyDetails">
                <p class="description">
                    <%= Model.Company.Description %>
                </p>
                <p class="field">
                    <span class="fieldLabel">Website: </span><a href="<%= Model.Company.Website %>">
                        <%= Model.Company.Website %></a>
                </p>
                <%if (HttpContext.Current.User.Identity.IsAuthenticated)
                  { %>
                <%if (Model.Company.MinimumRate.HasValue)
                  { %>
                <p class="field">
                    <span class="fieldLabel">Minimum: </span><span class="fieldValue hourlyRate">
                        <%= Model.Company.GetMinimumRate()%></span>
                </p>
                <%} %>
                <p class="field">
                    <span class="fieldLabel">Email:</span> <span class="fieldValue">
                        <%= Model.Company.Account.Email%></span>
                </p>
                <p class="field">
                    <span class="fieldLabel">Phone:</span> <span class="fieldValue">
                        <%= Model.Company.Account.Phone%></span>
                </p>
                <%} %>
                <p class="contact">
                    <a href="javascript:displayContactForm('<%:Model.Company.Account.Id %>');">
                        <img src="/images/buttons/contact.png" alt="Contact" /></a>
                </p>
                <p class="skills">
                    <% foreach (var tag in Model.Company.Tags)
                       { %>
                    <%=UIHelper.GetHTMLDisplayTag(tag, TAG_TYPE.COMPANIES)%>
                    <% } %>
                </p>
                <div class="clear">
                </div>
                <% if (Model.Company.Type == COMPANY_TYPE.COMPANY)
                   {
                       if (!(Model.Company.Consultants.Count == 1 && Model.Company.Consultants.First().Name == "John Doe"))
                       { %>
                <div class="people">
                    <ul>
                        <% foreach (var person in Model.Company.Consultants)
                           {
                        %>
                        <li>
                            <img src="<%= person.Picture_80%>" alt="<%= person.Name%>" />
                            <h4>
                                <%= person.Name%></h4>
                            <p>
                                <%= person.Designation%><br />
                                <a class="mainCredential" href="<%= person.LinkedinProfile%>">LinkedIn Profile</a><br />
                                <% foreach (var credential in person.Credentials)
                                   { %>
                                <a class="credential" href="<%= credential.Link%>">
                                    <%= credential.Type%></a>
                                <% } %>
                            </p>
                            <div class="clear">
                            </div>
                        </li>
                        <%
                            } %>
                    </ul>
                </div>
                <% }
                   }
                   else
                   {
                       var person = Model.Company.Consultants.First();
                %>
                <div class="individualLinks">
                    <h3>
                        <%= person.Name %>'s Links:</h3>
                    <ul>
                        <li><a href="<%= person.LinkedinProfile%>">LinkedIn Profile</a></li>
                        <% foreach (var credential in person.Credentials)
                           { %>
                        <li><a href="<%= credential.Link%>">
                            <%= credential.Type%></a></li>
                        <% } %>
                    </ul>
                </div>
                <% } %>
            </div>
        </div>
        <div class="wideRight" style="width: 800px">
            <!-- If there are pages.... -->
            <% if (Model.Company.Portfolio.Count != 0)
               { %>
            <div id="featured">
                <% foreach (var entry in Model.Company.Portfolio)
                   { %>
                <img src="<%= entry.Image_564 %>" alt="<%= entry.Title %>" rel="featured_<%= entry.Id%>" />
                <% } %>
            </div>
            <% foreach (var entry in Model.Company.Portfolio)
               { %>
            <span class="orbit-caption" id="featured_<%= entry.Id%>">
                <%= entry.Title %></span>
            <% } %>
            <% }
               else
               { %>
            <div>
                <img src="/images/no-portfolio.png" alt="No Portfolio" />
            </div>
            <% } %>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
