<%@ Page Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Companies.ViewItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Model.Company.Name %>'s Profile
</asp:Content>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <link href="/Styles/portfolio/simple/simple.css" rel="stylesheet" type="text/css" />
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
        <div class="narrowLeft">
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
                    <%= Model.Company.GetDescription(Model.LoggedInAccount) %>
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
        <div class="wideRight">
            <!-- If there are pages.... -->
            <% if (Model.Company.Portfolio.Count != 0)
               { %>
            <div class="pageNumber">
                <div class="contentArea" <% if (Model.Company.Portfolio.Count <= 1) { %>style="display:None"
                    <% } %>>
                    <span>Pages: </span>
                    <ul>
                        <% var pageNumber = 1;
                           foreach (var entry in Model.Company.Portfolio)
                           { %>
                        <li class="pageLink" id="pageLink<%= pageNumber %>"><a href="javascript:setPage(<%= pageNumber %>);">
                            <%= pageNumber%></a></li>
                        <% pageNumber++;
                           } %>
                    </ul>
                    <div class="clear">
                    </div>
                </div>
            </div>
            <div class="pages">
                <% int i = 1;
                   foreach (var entry in Model.Company.Portfolio)
                   { %>
                <div class="page" id="page<%= i %>">
                    <% if (entry.Type == PORTFOLIO_ENTRY_TYPE.IMAGE)
                       { %>
                    <img src="<%= entry.Image_564 %>" alt="<%: entry.Title %>" />
                    <div class="projectDesc">
                        <h3>
                            <%: entry.Title%></h3>
                        <p>
                            <%= entry.GetDescription(Model.LoggedInAccount)%>
                        </p>
                    </div>
                    <div class="clear">
                    </div>
                    <% }
                       else
                       { %>
                    <div class="htmlContent reset">
                        <%= entry.Description %>
                    </div>
                    <% } %>
                </div>
                <% i++;
                   } %>
            </div>
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
