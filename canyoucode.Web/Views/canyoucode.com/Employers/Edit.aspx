<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Employers.Edit>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Profile: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content employer">
        <ul class="tab full">
            <li><a href="/<%= Model.LoggedInAccount.Username %>">View</a></li>
            <li class="selected">Edit</li>
        </ul>
        <div class="form standard wide standout">
            <form action="/<%= Model.Employer.Account.Username %>/Edit" method="post" enctype="multipart/form-data"
            id="ProfileForm" onsubmit="return validateForm('ProfileForm');">
            <ul>
                <li>
                    <label for="Name">
                        Name</label>
                    <%:Html.TextBox("Name", Model.Employer.Name, new { size = 32 }, new Validation("Name").Required())%>
                </li>
                <li>
                    <p>
                        <label for="Country">
                            City</label>
                        <%:Html.TextBox("City", Model.Employer.City, new { size = 24 }, new Validation("City").Required())%></p>
                    <p>
                        <label for="Country">
                            Country</label>
                        <%:Html.DropDownList("Country", Model.Countries, new Validation("Country").Required()) %>
                    </p>
                </li>
                <li>
                    <label for="Email">
                        Email address</label>
                    <%:Html.TextBox("Email", Model.Employer.Account.Email, new { size = 32 }, new Validation("Email").Email().Required())%>
                    <p class="inputHint">
                        We send notifications to this email.</p>
                </li>
                <li>
                    <label for="Phone Number">
                        Phone Number</label>
                    <%:Html.TextBox("Phone", Model.Employer.Account.Phone, new { size = 20 }, new Validation("PhoneNumber").Required())%>
                </li>
                <li>
                    <label for="Password">
                        Password</label>
                    <a href="javascript:showChangePassword();">Change</a> </li>
            </ul>
            <input type="submit" value="Save" />
            </form>
        </div>
    </div>
    <div class="changePassword">
        <% Html.RenderPartial("~/Views/canyoucode.com/Shared/ChangePassword.ascx", Model); %>
    </div>
</asp:Content>
