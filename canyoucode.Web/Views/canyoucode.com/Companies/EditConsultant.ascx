<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/javascript">
    function showEditConsultant(id, name, designation, linkedinProfile, blog, github, stackoverflow, hackernews, picPath) {
        $('#Id').val(id);
        $('#Name').val(name);
        $('#Designation').val(designation);
        $('#LinkedinProfile').val(linkedinProfile);
        $('#Blog').val(blog);
        $('#Github').val(github);
        $('#Stackoverflow').val(stackoverflow);
        $('#Hackernews').val(hackernews);
        $('#CurrentPicture').attr('src', picPath);
        $('#editConsultant').dialog({
            modal: true,
            autoOpen: false,
            width: 640,
            title: 'Edit Consultant'
        });
        $('#editConsultant').dialog('open');
    }
</script>
<div id="editConsultant" class="form standard" style="display: none;">
    <form action="/<%= Model.Company.Account.Username %>/EditConsultant" method="post"
    name="EditConsultantForm" enctype="multipart/form-data" id="EditConsultantForm" onsubmit="return validateForm('EditConsultantForm');">
    <%:Html.Hidden("Id") %>
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
            <%:Html.TextBox("LinkedinProfile", "",  new { style = "width: 284px;" }, new Validation("LinkedIn Url").Required())%>
        </li>
        <li>
            <label for="Blog">
                Blog Url</label>
            <%:Html.TextBox("Blog", "", new { style = "width: 284px;" })%>
        </li>
        <li>
            <label for="Blog">
                Stackoverflow Profile Url</label>
            <%:Html.TextBox("Stackoverflow", "", new { style = "width: 284px;" })%>
        </li>
        <li>
            <label for="Blog">
                HackerNews Profile Url</label>
            <%:Html.TextBox("Hackernews", "", new { style = "width: 284px;" })%>
        </li>
        <li>
            <label for="Blog">
                Github Profile Url</label>
            <%:Html.TextBox("Github", "", new { style = "width: 284px;" })%>
        </li>
        <li>
            <label for="Picture">
                Picture</label>
            <img alt="Pic" src="" class="logoShow" id="CurrentPicture" name="CurrentPicture" />
            <input type="file" class="validated" name="Picture" id="Picture" validation="filetypes(jpg|png|jpeg)" />
        </li>
    </ul>
    <input type="submit" value="Save" />
    <input type="button" value="Cancel" onclick="$('#editConsultant').dialog('close');" />
    </form>
</div>
