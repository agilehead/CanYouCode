<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Setup Account: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form standard unruled">
        <form action="/Companies/SetupAccount" method="post" enctype="multipart/form-data"
        id="SetupAccountForm" onsubmit="return validateForm('SetupAccountForm');">
        <h1>
            Choose a password</h1>
        <ul class="section">
            <li>
                <label for="LinkedIn">
                    Password</label>
                <%:Html.Password("Password", "", new { size = 20, style = "width:140px" }, new Validation("Password").Required())%>
            </li>
            <li>
                <label for="LinkedIn">
                    Confirm Password</label>
                <%:Html.Password("ConfirmPassword", "", new { size = 20, style = "width:140px" }, new Validation("Confirm Password").ConfirmPassword().Required())%>
            </li>
        </ul>
        <input type="submit" value="Finish" />
        </form>
    </div>
</asp:Content>
