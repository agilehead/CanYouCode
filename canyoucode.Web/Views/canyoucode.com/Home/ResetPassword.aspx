<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Minimal.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.ResetPassword>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Reset Password: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content login">
        <%if (Model.IsValidToken) { %>
        <div class="form standard unruled narrow standout">
            
            <form action="/ResetPassword" id="ResetPasswordForm" method="post" onsubmit="return validateForm('ResetPasswordForm');">
            <%:Html.Hidden("Username", Model.Username) %>
            <%:Html.Hidden("Token", Model.Token) %>
            <h1>
                Reset Password</h1>
            <div class="section">
                <ul>
                    <li>
                        <label for="Password">
                            New Password</label>
                        <%:Html.Password("Password", "", new { size = 20, style = "width:140px" }, new Validation("Password").Required())%>
                    </li>
                    <li>
                        <label for="ConfirmPassword">
                            Confirm Password</label>
                        <%:Html.Password("ConfirmPassword", "", new { size = 20, style = "width:140px" }, new Validation("Confirm Password").Required().ConfirmPassword())%>
                    </li>
                </ul>
            </div>
            <input type="submit" value="Reset" />
            </form>

        </div>
        <%} else { %>
        <div>
            <span class="Alert">This is not a valid Reset Password Link. Please use the link in the email or re-send the request.</span>
        </div>
        <%} %>
    </div>
</asp:Content>
