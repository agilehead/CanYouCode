<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Minimal.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Login>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Login: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content login">
        <div class="form standard unruled narrow standout">
            <% using (Html.BeginForm())
               { %>
            <h1>
                Login</h1>
            <div class="section">
                <ul>
                    <li>
                        <label for="Designation">
                            Username</label>
                        <%:Html.TextBox("Username", "", new { size = 20, style = "width:140px" }, new Validation("Username").Required())%>
                    </li>
                    <li>
                        <label for="LinkedIn">
                            Password</label>
                        <%:Html.Password("Password", "", new { size = 20, style = "width:140px" }, new Validation("Password").Required())%>
                        <a href="/forgotpassword">forgot?</a>
                    </li>
                </ul>
            </div>
            <input type="submit" value="Login" />
            <% } %>
        </div>
        <div class="rPane">
            <h2>
                No account?</h2>
            <p>
                No problem. Signup for free<br />
                as a <a href="/Companies/Signup">consultant</a>, or an <a href="/Employers/Signup">employer</a>.</p>
        </div>
    </div>
</asp:Content>
