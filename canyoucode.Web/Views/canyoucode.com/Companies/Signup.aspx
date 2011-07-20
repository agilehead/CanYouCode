<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Companies.Signup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Signup for Companies: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function validateSubmit(type) {
            var validated = validateForm(type + 'SignupForm');
            if (validated) {
                if ($('#iAccept' + type + ':checked').val() == null) {
                    alert('You need to accept the terms and conditions.');
                    return false;
                }
                else {
                    return true;
                }
            } else {
                return false;
            }
        }

        function validateUsername(type) {
            if ($("#" + type + "SignupForm .username").val() == '') {
                $('.usernameAvailability').html('');
                return;
            }
            var username = $("#" + type + "SignupForm .username").val();
            $.ajax({
                url: "/Users/Exists",
                data: {
                    username: username
                },
                success: function (data) {
                    if (!data.Result)
                        $('.usernameAvailability').css('color', 'Green').html('available');
                    else
                        $('.usernameAvailability').css('color', 'Red').html('not available');
                },
                async: true,
                type: 'POST'
            });
        }

        $(document).ready(function () {
            $("#CompanySignupForm .username").keyup(function () {
                $("#CompanySignupForm .portfolioURL").html($("#CompanySignupForm .username").val());
            });

            $("#IndividualSignupForm .username").keyup(function () {
                $("#IndividualSignupForm .portfolioURL").html($("#IndividualSignupForm .username").val());
            });

            $("#CompanySignupForm .username").blur(function () {
                validateUsername('Company');
            });

            $("#IndividualSignupForm .username").blur(function () {
                validateUsername('Individual');
            });

            $('#CompanySignupForm').submit(function () {
                return validateSubmit('Company');
            });

            $('#IndividualSignupForm').submit(function () {
                return validateSubmit('Individual');
            });

            if ($('input:radio[name=CompanyType]:checked').val() == 'Company') {
                showCompanyRegister();
            } else {
                showIndividualRegiester();
            }
        });

        function showCompanyRegister() {
            $('#CompanySignupForm').show();
            $('#IndividualSignupForm').hide();
        }

        function showIndividualRegiester() {
            $('#CompanySignupForm').hide();
            $('#IndividualSignupForm').show();
        }
    </script>
    <div class="content suggest">
        <div class="form standard broad standout float">
            <ul>
                <li>
                    <%:Html.RadioButton("CompanyType", "Company", true, new { onclick="showCompanyRegister();" })%>
                    Company
                    <%:Html.RadioButton("CompanyType", "Individual", false, new { onclick="showIndividualRegiester();" })%>
                    Individual </li>
            </ul>
            <h1>
                Sign Up</h1>
            <p class="subText red">
                All fields are mandatory.</p>
            <div class="clear">
            </div>
            <form action="/Companies/Signup" id="CompanySignupForm" method="post">
            <%:Html.Hidden("Type", COMPANY_TYPE.COMPANY) %>
            <div class="section">
                <ul>
                    <li>
                        <label for="CompanyName">
                            Company name</label>
                        <%:Html.TextBox("CompanyName", "", new { size = 32 }, new Validation("Company Name").Required())%>
                    </li>
                    <li>
                        <label for="Website">
                            Website</label>
                        <span class="dim">http://</span><%:Html.TextBox("Website", "", new { size = 32 }, new Validation("Website").Required())%>
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
                        <label for="MinimumRate">
                            Minimum Rate</label>
                        <%:Html.DropDownList("MinimumRate", Model.MinimumRates, new Validation("Minimum Rate").Required())%>
                        <%:Html.DropDownList("Currency", Model.Currency, new Validation("Currency").Required())%>
                        per hour </li>
                    <li>
                        <label for="UserName">
                            Choose a username</label>
                        <%:Html.TextBox("Username", "", new { @class = "username" }, new Validation("Username").Required().MinimumLength(6))%>
                        <span class="usernameAvailability"></span>
                        <br />
                        <p class="inputHint">
                            <span class="dim">Your company's url is </span><span>http://www.canyoucode.com/</span><span
                                id="PortfolioURL" class="portfolioURL"></span>
                        </p>
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
                            Email Address</label>
                        <%:Html.TextBox("Email", "", new { size = 32 }, new Validation("Email").Required())%>
                        <p class="inputHint">
                            Company email address. We send notifications to this email.</p>
                    </li>
                    <li>
                        <label for="Phone">
                            Phone Number</label>
                        <%:Html.TextBox("Phone", "", new { size = 20 }, new Validation("Phone").Required())%>
                    </li>
                </ul>
            </div>
            <h2>
                About You (the Admin)</h2>
            <div class="section">
                <ul>
                    <li>
                        <label for="FullName">
                            Your name</label>
                        <%:Html.TextBox("Fullname", "", new { size = 32 }, new Validation("Fullname").Required())%>
                    </li>
                    <li>
                        <label for="Designation">
                            Designation</label>
                        <%:Html.TextBox("Designation", "", new { size = 32 }, new Validation("Designation").Required())%>
                    </li>
                    <li>
                        <label for="LinkedIn">
                            LinkedIn Url</label>
                        <%:Html.TextBox("LinkedIn", "", new { size = 56 }, new Validation("LinkedIn").Required())%>
                    </li>
                </ul>
            </div>
            <ul>
                <li>
                    <input type="checkbox" id="iAcceptCompany" />
                    I Accept the <a href="/Terms">Terms and Conditions</a></li>
            </ul>
            <input type="submit" value="Sign up" />
            </form>
            <form action="/Companies/Signup" id="IndividualSignupForm" method="post" style="display: none;">
            <%:Html.Hidden("Type", COMPANY_TYPE.INDIVIDUAL) %>
            <div class="section">
                <ul>
                    <li>
                        <label for="FullName">
                            Your name</label>
                        <%:Html.TextBox("Fullname", "", new { size = 32 }, new Validation("Fullname").Required())%>
                    </li>
                    <li>
                        <label for="LinkedIn">
                            LinkedIn Url</label>
                        <%:Html.TextBox("LinkedIn", "", new { size = 56 }, new Validation("LinkedIn").Required())%>
                    </li>
                    <li>
                        <label for="Website">
                            Website</label>
                        <span class="dim">http://</span><%:Html.TextBox("Website", "", new { size = 32 }, new Validation("Website").Required())%>
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
                        <label for="MinimumRate">
                            Minimum Rate</label>
                        <%:Html.DropDownList("MinimumRate", Model.MinimumRates, new Validation("Minimum Rate").Required())%>
                        <%:Html.DropDownList("Currency", Model.Currency, new Validation("Currency").Required())%>
                        per hour </li>
                    <li>
                        <label for="UserName">
                            Choose a username</label>
                        <%:Html.TextBox("Username", "", new { @class= "username"}, new Validation("Username").Required().MinimumLength(6)) %>
                        <span class="usernameAvailability"></span>
                        <br />
                        <p class="inputHint">
                            <span class="dim">Your company's url is </span><span>http://www.canyoucode.com/</span><span
                                class="portfolioURL"></span>
                        </p>
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
                            Email Address</label>
                        <%:Html.TextBox("Email", "", new { size = 32 }, new Validation("Email").Required())%>
                        <p class="inputHint">
                            Company email address. We send notifications to this email.</p>
                    </li>
                    <li>
                        <label for="Phone">
                            Phone Number</label>
                        <%:Html.TextBox("Phone", "", new { size = 20 }, new Validation("Phone").Required())%>
                    </li>
                </ul>
            </div>
            <ul>
                <li>
                    <input type="checkbox" id="iAcceptIndividual" />
                    I Accept the <a href="/Terms">Terms and Conditions</a></li>
            </ul>
            <input type="submit" value="Sign up" />
            </form>
        </div>
        <div class="bannerPane" style="width: 248px; background: url(/images/in-great-company.png) no-repeat;
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
