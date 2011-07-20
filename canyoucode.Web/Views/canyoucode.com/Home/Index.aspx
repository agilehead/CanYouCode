<%@ Page Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.HomePage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Find Freelancers. Find Work. CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content home">
        <div class="topPane">
            <div class="billboard">
                <div class="verified">
                    <a href="/Employers/Signup">
                        <img src="/images/home/verified.png" alt="Verified Consultants" /></a></div>
                <div class="fairpay">
                    <a href="/Companies/Signup">
                        <img src="/images/home/fairpay.png" alt="Fair Pay" /></a></div>
            </div>
        </div>
        <div class="main">
            <h2>
                Developers who joined recently...
            </h2>
            <div class="topList left">
                <ol class="companies">
                    <% foreach (var developer in Model.Pane1)
                       { %>
                    <li class="developer">
                        <div class="logo">
                            <img src="<%= developer.Logo_80 %>" alt="<%= developer.Name %>" />
                        </div>
                        <div class="companyDetails">
                            <h3>
                                <a href="/<%= developer.Account.Username %>">
                                    <%= developer.Name %></a></h3>
                            <span class="location">
                                <%= developer.City %>,
                                <%= developer.Country %></span>
                            <p class="description">
                                <%= developer.DisplayDescription %></p>
                            <p class="skills">
                                <% foreach (var tag in developer.Tags.Take(5))
                                   { %>
                                <%=UIHelper.GetHTMLDisplayTag(tag, TAG_TYPE.COMPANIES)%>
                                <% } %>
                            </p>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <% } %>
                </ol>
            </div>
            <div class="topList right">
                <%--<h2>
                    Top Developers
                </h2>--%>
                <ol class="companies">
                    <% foreach (var developer in Model.Pane2)
                       { %>
                    <li class="developer">
                        <div class="logo">
                            <img src="<%= developer.Logo_80 %>" alt="<%= developer.Name %>" />
                        </div>
                        <div class="companyDetails">
                            <h3>
                                <a href="/<%= developer.Account.Username %>">
                                    <%= developer.Name %></a></h3>
                            <span class="location">
                                <%= developer.City %>,
                                <%= developer.Country %></span>
                            <p class="description">
                                <%= developer.Description %></p>
                            <p class="skills">
                                <% foreach (var tag in developer.Tags.Take(5))
                                   { %>
                                <%=UIHelper.GetHTMLDisplayTag(tag, TAG_TYPE.COMPANIES)%>
                                <% } %>
                            </p>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <% } %>
                </ol>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
</asp:Content>
