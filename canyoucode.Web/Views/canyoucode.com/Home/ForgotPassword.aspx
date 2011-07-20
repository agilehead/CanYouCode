<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Minimal.Master" Inherits="DefaultViewPage<CanYouCodeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Forgot Password: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content login">
        <div class="form standard unruled narrow standout">
            <% using (Html.BeginForm())
               { %>
            <h1>
                Forgot Password</h1>
            <div class="section">
                <ul>
                    <li>
                        <label for="Username">
                            Username or Email</label>
                        <%:Html.TextBox("Username", "", new { size = 20, style = "width:140px" }, new Validation("Username").Required())%>
                    </li>
                </ul>
            </div>
            <input type="submit" value="Send" />
            <% } %>
        </div>
    </div>
</asp:Content>
