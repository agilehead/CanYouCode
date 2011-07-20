<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Admin.CreateMarketingAccounts>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create Marketing Accounts: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Username").keyup(function () {
                $("#PortfolioURL").html($("#Username").val());
            });

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
        });
    </script>
    <div class="content">
        <div class="form standard wide standout">
            <form action="/Admin/CreateMarketingAccounts" method="post" enctype="multipart/form-data"
            id="CreateForm" onsubmit="return validateForm('CreateForm');">
            <h1>
                Create</h1>
            <div class="section">
                <ul>
                    <li>
                        <label for="Type">Type</label>
                        <%:Html.RadioButton("Type", "Company", true)%>
                        Company
                        <%:Html.RadioButton("Type", "Individual", false)%>
                        Individual 
                    </li>
                    <li>
                        <label for="CompanyName">
                            Company name</label>
                        <%:Html.TextBox("CompanyName", "", new { size = 32 }, new Validation("CompanyName").Required())%>
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
                        <label for="UserName">
                            Choose a username</label>
                        <%:Html.TextBox("Username", new Validation("Username").Required().MinimumLength(6)) %>
                        <span id="usernameAvailability"></span>
                        <br />
                        <p class="inputHint">
                            <span class="dim">Your company's url is </span><span>http://www.canyoucode.com/</span><span
                                id="PortfolioURL"></span>
                        </p>
                    </li>
                    <li>
                        <label for="Email">
                            Email address</label>
                        <%:Html.TextBox("Email", "", new { size = 32 }, new Validation("Email").Required())%>
                        <p class="inputHint">
                            We send notifications to this email.</p>
                    </li>
                    <li>
                        <label for="Phone">
                            Phone Number</label>
                        <%:Html.TextBox("Phone", "", new { size = 20 }, new Validation("Phone").Required())%>
                    </li>
                    <li>
                        <label for="Logo">
                            Logo</label>
                        <input type="file" id="Logo" name="Logo" />
                    </li>
                    <li>
                        <label for="Name">
                            Description</label>
                        <%:Html.TextArea("Description", "", new { rows=4, cols= 25 }, new Validation("Description").Required().MaximumLength(5000))%>
                    </li>
                    <li>
                        <label for="Name">
                            Referring URL</label>
                        <%:Html.TextBox("ReferringURL", "", new { size = 20 })%>
                        <p class="inputHint">
                            example the s_o_r_t_f_i_l_i_o url for this consultant.</p>
                    </li>
                </ul>
                <%foreach (var id in Enumerable.Range(1, 5))
                  { %>
                <div class="section">
                    <h3>
                        Portfolio Entry
                        <%:id %>
                    </h3>
                    <ul>
                        <li>
                            <label for="Title">
                                Portfolio Title</label>
                            <%:Html.TextBox("Title_" + id, new Validation("Title"))%>
                        </li>
                        <li>
                            <label for="Description">
                                Portfolio Description</label>
                            <%:Html.TextArea("Description_" + id, "You can put a description of this work here.", new { rows = 4, cols = 25 }, new Validation("Description").MaximumLength(5000))%>
                        </li>
                        <li>
                            <label for="Image">
                                Portfolio Image</label>
                            <input type="file" class="validated" name="Image_<%:id %>" id="Image_<%:id %>" validation="filetypes(jpg|png|jpeg)" />
                        </li>
                    </ul>
                </div>
                <%} %>
            </div>
            <input type="submit" value="Create" />
            </form>
        </div>
    </div>
</asp:Content>
