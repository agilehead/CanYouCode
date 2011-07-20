<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/javascript">
    function showChangePassword() {
        $('.changePassword').dialog({
            height: 300,
            width: 480,
            modal: true,
            autoOpen: false,
            title: 'Change Password'
        });
        $('.changePassword').dialog('open');
    }
</script>
<div class="content changePassword" style="display: none;">
    <div class="form standard">
        <form action="/users/ChangePassword" method="post"
        enctype="multipart/form-data" id="ChangePasswordForm" onsubmit="return validateForm('ChangePasswordForm');">
        <div>
            <ul>
                <li>
                    <label for="Designation">
                        CurrentPassword</label>
                    <%:Html.Password("OldPassword", "", new { size = 20, style = "width:140px" }, new Validation("Username").Required())%>
                </li>
                <li>
                    <label for="LinkedIn">
                        New Password</label>
                    <%:Html.Password("Password", "", new { size = 20, style = "width:140px" }, new Validation("New Password").Required())%>
                </li>
                <li>
                    <label for="LinkedIn">
                        Confirm Password</label>
                    <%:Html.Password("ConfirmPassword", "", new { size = 20, style = "width:140px" }, new Validation("Confirm Password").ConfirmPassword().Required())%>
                </li>
            </ul>
        </div>
        <input type="submit" value="Save" />
        </form>
    </div>
</div>
