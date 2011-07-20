<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Projects.EditProject>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%if (Model.Project != null && Model.Project.Id > 0)
      { %>Edit
    <%}
      else
      { %>Add
    <%} %>Project: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/Scripts/tiny_mce/jquery.tinymce.js"></script>
    <script type="text/javascript">
        var tags = <%=Model.Tags %>;
        $(document).ready(function () {
            $('#ClosingDate').datepicker({ dateFormat: 'mm/dd/yy' });
            $(".date").mask("99/99/9999");

            $("#SearchTags").autocomplete(tags, {
                dataType: 'json',
                multiple: true,
                mustMatch: false,
                autoFill: true,
                parse: function (dat) {
                    var rows = new Array();
                    for (var i = 0; i < dat.length; i++) {
                        rows[i] = { data: dat[i], value: dat[i].Value, result: dat[i].Value };
                    }
                    return rows;
                },
                formatItem: function (data) {
                    return data.Value;
                }
            }).result(function (event, data) {
                var tagIds = $('#SelectedTags').val().split(',');
                tagIds.push(data.Key);
                $('#SelectedTags').val(tagIds.join(','));
            });

            $("#SearchTags").change(function () {
                if ($("#SelectedTags").val().length > 0) {
                    var selectedtags = $("#SearchTags").val().split(',');

                    var tagIds = [];
                    $.each(selectedtags, function (index, val) {
                        $.each(tags, function (i, v) {
                            if ($.trim(v.Value.toLowerCase()) == $.trim(val.toLowerCase())) {
                                tagIds.push(v.Key);
                            }
                        });
                    });

                    $("#SelectedTags").val(tagIds.join(","))
                }
            });

            $('#Description').tinymce({
			    script_url : '/Scripts/tiny_mce/tiny_mce.js',
                content_css : "/Styles/tinymce-custom.css",
			    mode : "textareas",
                theme : "simple"
		    });
        });
    </script>
    <div class="content register">
        <div class="form standard wider standout">
            <form action="/<%:Model.LoggedInAccount.Username %>/AddProject" method="post" enctype="multipart/form-data"
            id="ProjectForm" name="ProjectForm" onsubmit="return validateForm('ProjectForm');">
            <h1>
                Submit a Project</h1>
            <div class="section">
                <ul>
                    <li>
                        <label for="Name">
                            Title</label>
                        <%:Html.TextBox("Name", Model.Project.Title, new { size = 50, maxlength = 80 }, new Validation("Name").Required().MaximumLength(80))%>
                    </li>
                    <li>
                        <label for="Description">
                            Description</label>
                        <%:Html.TextArea("Description", Model.Project.Description, new { rows = "20", cols = "62" }, new Validation("Description").Required().MinimumLength(6))%>
                        <span id="usernameAvailability"></span>
                        <br />
                    </li>
                    <li>
                        <label for="Attachment">
                            Attachment</label>
                        <input type="file" class="validated" name="Attachment" id="Attachment" validation="filetypes(pdf|doc|docx|odf|ppt)" />
                        <p class="inputHint">
                            you can upload PDF, DOC & DOCX, ODF, PPT</p>
                    </li>
                    <li>
                        <label for="Name">
                            Categories</label>
                        <%:Html.TextArea("SearchTags", string.Join(",", Model.Project.Tags.Select(t => t.Name)), new { rows = 2, cols = 35 })%>
                        <%:Html.Hidden("SelectedTags", string.Join(",", Model.Project.Tags.Select(t => t.Id)))%>
                        <p class="inputHint">
                            Web Design, PHP, Java, LISP etc</p>
                    </li>
                    <li>
                        <label for="Name">
                            Budget</label>
                        <%:Html.DropDownList("Budget", UIHelper.GetProjectBudgetSelectItemList(Model.Project.Id <= 0 ? 1000 : Model.Project.Budget), new Validation("Budget").Required()) %>
                        $
                        <p class="inputHint">
                            the project should have a minimum budget of $1000</p>
                    </li>
                    <li>
                        <label for="Name">
                            Closing Date for Bids</label>
                        <%:Html.TextBox("ClosingDate", Model.Project.Id > 0 ? Model.Project.ClosingDate.ToShortDateString() : DateTime.Now.AddDays(30).ToShortDateString(), new { size = 32, @class = "date" }, new Validation("Closing Date").Date().Required())%>
                    </li>
                </ul>
            </div>
            <div class="buttonBar">
                <a class="button" href="javascript:document.ProjectForm.submit()">
                    <img src="/images/buttons/create-project.png" alt="Create Project" /></a> <a class="link red"
                        href="/<%:Model.LoggedInAccount.Username %>/Projects">Cancel</a>
                <div class="clear">
                </div>
            </div>
            </form>
        </div>
    </div>
</asp:Content>
