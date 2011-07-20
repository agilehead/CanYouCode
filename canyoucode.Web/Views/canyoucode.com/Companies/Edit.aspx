<%@ Page Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Companies.Edit>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Profile: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/Scripts/tiny_mce/jquery.tinymce.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#tabs').tabs();
            $('#tabs').show();
            
            <% if (!string.IsNullOrEmpty(Model.Company.Logo))
            { 
            %>
            $(".logoShow").show();
            $(".logoEdit").hide(); 
            <%
            } else {%>
            $(".logoShow").hide();
            $(".logoEdit").show(); 
            <%} %>

            $(".changeLogo").click(function(){
                $(".logoShow").hide();
                $(".logoEdit").show(); 
            });

            $(".credentials").hide();

            $('.htmlEditor textarea').tinymce({
			    script_url : '/Scripts/tiny_mce/tiny_mce.js',
                content_css : "/Styles/tinymce-custom.css",
			    mode : "textareas",
                theme : "simple",
                width: "700",
                height: "320"
		    });
        });

        function showCreds()
        {
            $('.credentials').show();
            $('.otherCreds').hide();
        }

    </script>
    <%if (Model.Company.Type == COMPANY_TYPE.COMPANY)
      { %>
    <%Html.RenderPartial("~/Views/canyoucode.com/Companies/EditConsultant.ascx"); %>
    <%} %>
    <%Html.RenderPartial("~/Views/canyoucode.com/Shared/ConfirmDialog.ascx"); %>
    <div class="content company edit">
        <% if (Context.User.IsInRole(ACCOUNT_TYPE.COMPANY))
           { %>
        <ul class="tab full">
            <li><a href="/<%= Model.Company.Account.Username %>">View</a></li>
            <li class="selected"><a href="/<%= Model.Company.Account.Username %>/Edit">Edit</a></li>
        </ul>
        <% } %>
        <div id="tabs" style="display: none;">
            <ul>
                <li><a href="#Summary">Summary</a></li>
                <li><a href="#People">
                    <%if (Model.Company.Type == COMPANY_TYPE.COMPANY)
                      { %>People<%}
                      else
                      {%>Personal<%} %></a></li>
                <%foreach (var porfolioEntry in Model.Company.Portfolio)
                  {%>
                <li><a href="#Portfolio<%:porfolioEntry.Id %>">
                    <%: porfolioEntry.Title.Length > 14 ? porfolioEntry.Title.Substring(0, 14) + ".." : porfolioEntry.Title%>
                </a></li>
                <%} %>
                <li><a href="#PortfolioNew">New Page</a></li>
            </ul>
            <div id="Summary">
                <div class="form standard">
                    <form action="/<%= Model.Company.Account.Username %>/Save" method="post" enctype="multipart/form-data"
                    id="SummaryForm" onsubmit="return validateForm('SummaryForm');">
                    <ul>
                        <%if (Model.Company.Type == COMPANY_TYPE.COMPANY)
                          { %>
                        <li>
                            <label for="Name">
                                Company Name</label>
                            <%:Html.TextBox("Name", Model.Company.Name, new Validation("Company Name").Required().MaximumLength(50))%>
                        </li>
                        <li>
                            <label for="Name">
                                Logo</label>
                            <img alt="Logo" src="<%:Model.Company.Logo_80 %>" class="logoShow" />
                            <span class="likeLink logoShow changeLogo">change</span>
                            <input type="file" class="logoEdit validated" name="Logo" id="Logo" validation="filetypes(jpg|png|jpeg)" />
                        </li>
                        <%}
                          else
                          { %>
                        <li>
                            <label for="Name">
                                Name</label>
                            <%:Html.TextBox("Name", Model.Company.Name, new Validation("Name").Required().MaximumLength(50))%>
                        </li>
                        <%} %>
                        <li>
                            <label for="Name">
                                City</label>
                            <%:Html.TextBox("City", Model.Company.City, new Validation("City").Required().MaximumLength(50))%>
                        </li>
                        <li>
                            <label for="Name">
                                Country</label>
                            <%:Html.DropDownList("Country", Model.Countries, " Select ", new Validation("Country").Required()) %>
                        </li>
                        <li>
                            <label for="Name">
                                Description</label>
                            <%:Html.TextArea("Description", Model.Company.Description, new { rows = 4, cols = 60 }, new Validation("Description").Required().MaximumLength(200))%>
                        </li>
                        <li>
                            <label for="Name">
                                Technology</label>
                            <%Html.RenderPartial("~/Views/canyoucode.com/Shared/Tags.ascx", Model.Company.Tags); %>
                        </li>
                        <li>
                            <label for="Name">
                                Minimum Rate</label>
                            <%:Html.DropDownList("MinimumRate", Model.MinimumRates, new Validation("MinimumRate").Required())%>
                            <%:Html.DropDownList("Currency", Model.Currency, new Validation("Currency").Required())%>
                            per hour </li>
                        <li>
                            <label for="Name">
                                Contact Email</label>
                            <%:Html.TextBox("Email", Model.Company.Account.Email, new Validation("Email").Required().Email())%>
                        </li>
                        <li>
                            <label for="Name">
                                Contact Phone</label>
                            <%:Html.TextBox("Phone", Model.Company.Account.Phone, new Validation("Phone").Required())%>
                        </li>
                        <li>
                            <label for="Password">
                                Password</label>
                            <a href="javascript:showChangePassword();">Change</a> </li>
                        <li>
                            <label for="Account">
                                Account Status</label>
                            <%if (Model.Company.Account.Status == ACCOUNT_STATUS.ACTIVE)
                              { %>
                            <a href="javascript:displayConfirmDialog('Deactivate your account', '/<%: Model.Company.Account.Username %>/ChangeStatus', 'Are you sure you want to deactivate your account? You will not be able to bid on projects and your profile will no longer be available publicly.');">
                                Deactivate</a>
                            <%}
                              else
                              { %>
                            <a href="/<%= Model.Company.Account.Username %>/ChangeStatus">Activate</a>
                            <%} %>
                        </li>
                    </ul>
                    <input type="submit" value="Save" />
                    </form>
                </div>
            </div>
            <div id="People">
                <%if (Model.Company.Type == COMPANY_TYPE.COMPANY)
                  { %>
                <div class="form standard packed dense unruled left">
                    <form action="/<%= Model.Company.Account.Username %>/AddConsultant" method="post"
                    name="AddConsultant" enctype="multipart/form-data" id="AddConsultant" onsubmit="return validateForm('AddConsultant');">
                    <h1>
                        Add an employee</h1>
                    <ul>
                        <li>
                            <label for="Name">
                                Name</label>
                            <%:Html.TextBox("Name", new Validation("Name").Required())%>
                        </li>
                        <li>
                            <label for="Designation">
                                Designation</label>
                            <%:Html.TextBox("Designation", new Validation("Designation").Required())%>
                        </li>
                        <li>
                            <label for="LinkedinProfile">
                                LinkedIn Url</label>
                            <%:Html.TextBox("LinkedinProfile", "", new { style = "width: 284px;" }, new Validation("LinkedIn Url").Required())%>
                        </li>
                        <li>
                            <label for="Picture">
                                Picture</label>
                            <input type="file" id="Picture" name="Picture" />
                        </li>
                        <li class="credentials">
                            <label for="Blog">
                                Blog Url</label>
                            <%:Html.TextBox("Blog", "", new { style = "width: 284px;" })%>
                        </li>
                        <li class="credentials">
                            <label for="Blog">
                                Stackoverflow Profile Url</label>
                            <%:Html.TextBox("Stackoverflow", "", new { style = "width: 284px;" })%>
                        </li>
                        <li class="credentials">
                            <label for="Blog">
                                Hacker News Profile Url</label>
                            <%:Html.TextBox("Hackernews", "", new { style = "width: 284px;" })%>
                        </li>
                        <li class="credentials">
                            <label for="Blog">
                                Github Profile Url</label>
                            <%:Html.TextBox("Github", "", new { style = "width: 284px;" })%>
                        </li>
                    </ul>
                    <p class="otherCreds">
                        <a href="javascript:showCreds();" class="credShowLink">Add other links</a> like
                        blog, github or stackoverflow profiles.
                    </p>
                    <input type="submit" value="Add" />
                    </form>
                </div>
                <div class="right">
                    <h1>
                        Current Employees</h1>
                    <ul class="list">
                        <%foreach (var person in Model.Company.Consultants)
                          { %>
                        <li>
                            <img alt="<%:person.Name %>" src="<%:person.Picture_80%>" />
                            <h4>
                                <%:person.Name%>
                            </h4>
                            <p>
                                <span>
                                    <%:person.Designation%>
                                </span>
                                <br />
                                <a class="mainCredential" href="<%:person.LinkedinProfile %>">LinkedIn</a>
                                <%foreach (var cred in person.Credentials)
                                  { %>
                                <a class="credential" href="<%:cred.Link %>">
                                    <%:cred.Type%></a>
                                <%} %>
                            </p>
                            <p>
                                <a href="javascript:showEditConsultant('<%:person.Id %>', '<%:person.Name %>', '<%:person.Designation %>', '<%:person.LinkedinProfile %>', '<%:person.Blog%>', '<%:person.Github %>', '<%:person.Stackoverflow %>', '<%:person.HackerNews %>','<%:person.Picture%>');"
                                    class="smallLink green">Edit</a>
                                <%if (Model.Company.Consultants.Count > 1)
                                  {%>
                                <a href="javascript:displayConfirmDialog('Delete Consultant - <%:person.Name %>', '/<%: Model.Company.Account.Username %>/DeleteConsultant?id=<%:person.Id %>', 'Are you sure you want to remove consultant - <%:person.Name %>?');"
                                    class="smallLink red">Delete</a>
                                <%} %>
                            </p>
                        </li>
                        <%} %>
                    </ul>
                </div>
                <div class="clear">
                </div>
                <%}
                  else
                  {
                      var consultantInfo = Model.Company.Consultants.Single();
                %>
                <div class="form standard">
                    <form action="/<%= Model.Company.Account.Username %>/EditConsultant" method="post"
                    name="EditConsultantForm" enctype="multipart/form-data" id="EditConsultantForm" onsubmit="return validateForm('EditConsultantForm');">
                    <%:Html.Hidden("Id", consultantInfo.Id)%>
                    <ul>
                        <li>
                            <label for="LinkedinProfile">
                                LinkedIn Url</label>
                            <%:Html.TextBox("LinkedinProfile", consultantInfo.LinkedinProfile,  new { style = "width: 284px;" }, new Validation("LinkedIn Url").Required())%>
                        </li>
                        <li>
                            <label for="Blog">
                                Blog Url</label>
                            <%:Html.TextBox("Blog", consultantInfo.Blog, new { style = "width: 284px;" })%>
                        </li>
                        <li>
                            <label for="Blog">
                                Stackoverflow Profile Url</label>
                            <%:Html.TextBox("Stackoverflow", consultantInfo.Stackoverflow, new { style = "width: 284px;" })%>
                        </li>
                        <li>
                            <label for="Blog">
                                HackerNews Profile Url</label>
                            <%:Html.TextBox("Hackernews", consultantInfo.HackerNews, new { style = "width: 284px;" })%>
                        </li>
                        <li>
                            <label for="Blog">
                                Github Profile Url</label>
                            <%:Html.TextBox("Github", consultantInfo.Github, new { style = "width: 284px;" })%>
                        </li>
                        <li>
                            <label for="Picture">
                                Picture</label>
                            <img alt="Pic" src="<%:consultantInfo.Picture %>" class="logoShow" id="CurrentPicture"
                                name="CurrentPicture" />
                            <input type="file" class="validated" name="Picture" id="File3" validation="filetypes(jpg|png|jpeg)" />
                        </li>
                    </ul>
                    <input type="submit" value="Save" />
                    </form>
                </div>
                <%} %>
            </div>
            <%foreach (var portfolioEntry in Model.Company.Portfolio)
              {%>
            <div id="Portfolio<%:portfolioEntry.Id %>">
                <h1 style="float: left;">
                    Edit Page</h1>
                <p style="height: 32px; padding-top: 6px; margin-left: 750px">
                    <a href="javascript:displayConfirmDialog('Delete Page', '/<%: Model.Company.Account.Username %>/DeletePortfolioEntry?id=<%:portfolioEntry.Id %>', 'Are you sure you want to remove page - <%:portfolioEntry.Title %>');"
                        class="red">Delete this page</a></p>
                <% if (portfolioEntry.Type == PORTFOLIO_ENTRY_TYPE.IMAGE)
                   { %>
                <div class="form standard">
                    <form action="/<%= Model.Company.Account.Username %>/EditImageAndDescriptionPage"
                    method="post" enctype="multipart/form-data" id="PortfolioEntryEdit<%:portfolioEntry.Id %>"
                    onsubmit="return validateForm('PortfolioEntryEdit<%:portfolioEntry.Id %>');">
                    <%:Html.Hidden("PortfolioEntryId", portfolioEntry.Id)%>
                    <ul>
                        <li>
                            <label for="Title">
                                Title</label>
                            <%:Html.TextBox("Title", portfolioEntry.Title, new { size = "40" }, new Validation("Title").Required())%>
                        </li>
                        <li>
                            <label for="Description">
                                Description</label>
                            <%:Html.TextArea("Description", portfolioEntry.Description, new { rows = 4, cols = 70 }, new Validation("Description").Required().MaximumLength(5000))%>
                        </li>
                        <li>
                            <label for="Image">
                                Image</label>
                            <img alt="<%:portfolioEntry.Title %>" src="<%:portfolioEntry.Image_564 %>" />
                            <input style="margin: 8px 0 0 172px" type="file" class="validated" name="Logo" id="File1"
                                validation="filetypes(jpg|png|jpeg)" />
                            <p class="subText" style="margin-left: 172px">
                                Recommended size is 564x394 pixels (fully compatible with our themes).</p>
                        </li>
                    </ul>
                    <input type="submit" value="Save" />
                    </form>
                </div>
                <% }
                   else
                   { %>
                <div class="form standard htmlEditor">
                    <form action="/<%= Model.Company.Account.Username %>/EditHtmlPage" method="post"
                    enctype="multipart/form-data" id="EditPortfolioHtml<%: portfolioEntry.Id %>" onsubmit="return validateForm('EditPortfolioHtml<%:portfolioEntry.Id %>');">
                    <%:Html.Hidden("PortfolioEntryId", portfolioEntry.Id)%>
                    <ul>
                        <li>
                            <label for="EditHtmlPageTitle" style="width: 100px">
                                Title</label>
                            <%:Html.TextBox("Title", portfolioEntry.Title, new { size = "40" }, new Validation("Title").Required())%>
                        </li>
                        <li>Html Content<br />
                            <%:Html.TextArea("Html", portfolioEntry.Description, new { rows=12, cols= 70 })%></li>
                    </ul>
                    <input type="submit" value="Save" />
                    </form>
                </div>
                <% } %>
            </div>
            <%} %>
            <div id="PortfolioNew">
                <div id="addImageAndDescription">
                    <div class="pageType">
                        Image with Description <span style="color: Gray">|</span> <a href="javascript:flipPanel('#addImageAndDescription', '#addHtml')">
                            HTML Editor</a>
                    </div>
                    <div class="form standard">
                        <form action="/<%= Model.Company.Account.Username %>/AddImageAndDescriptionPage"
                        method="post" enctype="multipart/form-data" id="PortfolioEntryFrom" onsubmit="return validateForm('PortfolioEntryFrom');">
                        <ul>
                            <li>
                                <label for="Title">
                                    Title</label>
                                <%:Html.TextBox("Title", "", new { size = "40" }, new Validation("Title").Required())%>
                            </li>
                            <li>
                                <label for="Description">
                                    Description</label>
                                <%:Html.TextArea("Description", new { rows=4, cols= 70 })%>
                            </li>
                            <li>
                                <label for="Image">
                                    Image</label>
                                <input type="file" class="validated" name="Logo" id="File2" validation="required,filetypes(jpg|png|jpeg)" />
                                <p class="subText" style="margin-left: 172px">
                                    Recommended size is 564x394 pixels (fully compatible with our themes).</p>
                            </li>
                        </ul>
                        <input type="submit" value="Add Page" />
                        </form>
                    </div>
                </div>
                <div id="addHtml" class="htmlEditor" style="display: none">
                    <div class="pageType">
                        <a href="javascript:flipPanel('#addHtml', '#addImageAndDescription')">Image with Description</a>
                        <span style="color: Gray">| </span>HTML Editor
                    </div>
                    <div class="form standard">
                        <form action="/<%= Model.Company.Account.Username %>/AddHtmlPage" method="post" enctype="multipart/form-data"
                        id="AddPortfolioHtmlForm" onsubmit="return validateForm('AddPortfolioHtmlForm');">
                        <ul>
                            <li>
                                <label for="AddPortfolioHtmlTitle" style="width: 100px">
                                    Page Title</label>
                                <%:Html.TextBox("AddPortfolioHtmlTitle", "", new { size = "40" }, new Validation("Title").Required())%>
                            </li>
                            <li>Html Content<br />
                                <%:Html.TextArea("AddPortfolioHtml", new { rows=12, cols= 70 })%></li>
                        </ul>
                        <input type="submit" value="Add Page" />
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="changePassword">
        <% Html.RenderPartial("~/Views/canyoucode.com/Shared/ChangePassword.ascx", Model); %>
    </div>
</asp:Content>
