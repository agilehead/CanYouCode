<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Search.Experts>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Find Experts: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content search">
        <%Html.RenderPartial("~/Views/canyoucode.com/Shared/SearchBar.ascx", Model); %>
        <ol class="results companies">
            <%foreach (var company in Model.Companies)
              { %>
            <li>
                <div class="developer">
                    <div class="logo">
                        <img src="<%:company.Logo_80 %>" alt="<%:company.Name %>" />
                    </div>
                    <div class="companyDetails">
                        <h3>
                            <a href="/<%:company.Account.Username %>">
                                <%:company.Name %></a></h3>
                        <span class="location">
                            <%:company.City %>,
                            <%:company.Country %></span>
                        <p class="description">
                            <%:company.DisplayDescription %>
                        </p>
                        <p class="skills">
                            <%foreach (var tag in company.Tags.Take(4))
                              { %>
                            <%=UIHelper.GetHTMLDisplayTag(tag, TAG_TYPE.COMPANIES)%>
                            <%} %>
                            <%if (company.Tags.Count > 4)
                              { %>
                            <span class="skill">
                                <%:company.Tags.Count - 4 %>
                                more.. </span>
                            <%} %>
                        </p>
                    </div>
                </div>
                <div class="clear">
                </div>
            </li>
            <%} %>
        </ol>
        <%if (Model.Companies.Count() <= 0)
          { %>
        <div class="results companies">
            <span>No Results.</span>
        </div>
        <%} %>
        <div class="clear">
        </div>
    </div>
    <div class="clear">
    </div>
</asp:Content>
