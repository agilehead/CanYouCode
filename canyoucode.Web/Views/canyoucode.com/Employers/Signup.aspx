<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Employers.Edit>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Signup for Employers: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Username").blur(function () {
                if ($("#Username").val() == '') {
                    $('#usernameAvailability').html('');
                    return;
                }
                $.ajax({
                    url: "/Users/Exists",
                    data: {
                        username: $("#Username").val()
                    },
                    success: function (data) {
                        if (!data.Result)
                            $('#usernameAvailability').css('color', 'Green').html('available');
                        else
                            $('#usernameAvailability').css('color', 'Red').html('not available');
                    },
                    async: true,
                    type: 'POST'
                });
            });

            $('#EmployerSignupForm').submit(function () {
                var validated = validateForm('EmployerSignupForm');
                if (validated) {
                    if ($('#iAccept:checked').val() == null) {
                        alert('You need to accept the terms and conditions.');
                        return false;
                    }
                    else {
                        return true;
                    }
                } else {
                    return false;
                }
            });
        });
    </script>
    <div class="content register">
        <div class="form standard broad standout float">
            <form onsubmit="return validateForm('EmployerSignupForm');" method="post" id="EmployerSignupForm"
            action="/Employers/Signup">
            <h1>
                Sign Up</h1>
            <p class="subText red">
                All fields are mandatory.</p>
            <div class="section">
                <ul>
                    <li>
                        <label for="Name">
                            Name</label>
                        <%:Html.TextBox("Name", "", new { size = 32 }, new Validation("Name").Required())%>
                    </li>
                    <li>
                        <p>
                            <label for="Country">
                                City</label>
                            <%:Html.TextBox("City", "", new { size = 24 }, new Validation("City").Required())%></p>
                        <p>
                            <label for="Country">
                                Country</label>
                            <%:Html.DropDownList("Country", Model.Countries, new Validation("Country").Required()) %>
                        </p>
                    </li>
                    <li>
                        <label for="UserName">
                            Choose a username</label>
                        <%:Html.TextBox("Username", new Validation("Username").Required().MinimumLength(6)) %>
                        <span id="usernameAvailability"></span>
                        <br />
                    </li>
                    <li>
                        <p>
                            <label for="Password">
                                Password</label>
                            <%:Html.Password("Password", new Validation("Password").Required().MinimumLength(8)) %>
                        </p>
                        <p>
                            <label for="ConfirmPassword">
                                Confirm Password</label>
                            <%:Html.Password("ConfirmPassword", new Validation("Confirm Password").Required().ConfirmPassword()) %>
                        </p>
                    </li>
                    <li>
                        <label for="Email">
                            Email address</label>
                        <%:Html.TextBox("Email", "", new { size = 32 }, new Validation("Email").Email().Required())%>
                        <p class="inputHint">
                            We send notifications to this email.</p>
                    </li>
                    <li>
                        <label for="Phone Number">
                            Phone Number</label>
                        <%:Html.TextBox("Phone", "", new { size = 20 }, new Validation("PhoneNumber").Required())%>
                    </li>
                </ul>
            </div>
            <ul>
                <li>
                    <input type="checkbox" id="iAccept" />
                    I Accept the <a href="/Terms">Terms and Conditions</a></li>
            </ul>
            <input type="submit" value="Sign up" />
            </form>
        </div>
        <div class="bannerPane" style="width: 248px; background: url(/images/the-best-talent.png) no-repeat;
            padding: 120px 0 0 32px">
            <p>
                <img src="/images/ext/pixelhaven.gif" alt="pixel haven" />
            </p>
            <p>
                <img src="/images/ext/art-working.png" alt="art working" />
            </p>
            <p>
                <img src="/images/ext/sandbenders.png" alt="sandbenders" />
            </p>
            <p>
                <img src="/images/ext/emotions-by-mike.png" alt="emotions by mike" />
            </p>
            <p>
                <img src="/images/ext/artifice-studios.png" alt="artifice studios" />
            </p>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
