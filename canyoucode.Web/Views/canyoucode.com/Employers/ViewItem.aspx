<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Employers.ViewItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:Model.Employer.Name %>'s Profile
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content employer view">
        <% if (Model.LoggedInAccount != null && Model.LoggedInAccount.Type == ACCOUNT_TYPE.EMPLOYER)
           { %>
        <ul class="tab full">
            <li class="selected">View</li>
            <li><a href="/<%= Model.LoggedInAccount.Username %>/Edit">Edit</a></li>
        </ul>
        <% } %>
        <h1>
            <%:Model.Employer.Name %></h1>
        <p>
            <span class="location">
                <%:Model.Employer.City %>,
                <%:Model.Employer.Country %></span>
        </p>
        <div class="contact">
            <p>
                <span class="fieldLabel">Email:</span><span class="fieldValue">
                    <%= Model.Employer.Account.Email %></span>
            </p>
            <p>
                <span class="fieldLabel">Phone:</span> <span class="fieldValue">
                    <%= Model.Employer.Account.Phone %></span>
            </p>
        </div>
        <% var projects = Model.Employer.GetOpenProjects(); %>
        <% if (projects.Count() > 0)
           { %>
        <h2>
            Listed Projects</h2>
        <table class="projects">
            <colgroup>
                <col width="100px" />
                <col width="300px" />
                <col width="100px" />
                <col width="200px" />
                <col width="100px" />
            </colgroup>
            <tr>
                <th>
                    Date
                </th>
                <th>
                    Project
                </th>
                <th>
                    Budget
                </th>
                <th>
                    Awarded to
                </th>
                <th>
                    Bid
                </th>
            </tr>
            <%foreach (var project in projects)
              {
                  var awardedBid = project.GetAwarededBid(); %>
            <tr>
                <td style="font-size: 11px">
                    <%:project.DateAdded.GetFancyDate()%>
                </td>
                <td>
                    <a href="/Projects/<%= project.Id %>">
                        <%:project.Title%></a>
                </td>
                <td>
                    <%= project.GetBudget()%>
                </td>
                <td>
                    <% 
                        if (awardedBid != null)
                        {  %>
                    <%= awardedBid.Company.Name%>
                    <% }
                        else
                        { %>
                    Not awarded
                    <% } %>
                </td>
                <td>
                    <% 
                        if (awardedBid != null)
                        {  %>
                    <%= awardedBid.GetQuote()%>
                    <% } %>
                </td>
            </tr>
            <% } %>
        </table>
        <% }
           else
           { %>
        <p>
            <%= Model.Employer.Name %>
            has not listed any projects yet.</p>
        <% } %>
    </div>
</asp:Content>
