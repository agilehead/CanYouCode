<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/javascript">
    function displayConfirmDialog(title, getURL, message) {
            $('#ConfirmMessage').html(message);
            $('.confirmDialog').dialog({
                autoOpen: false,
                resizable: false,
                height: 180,
                width:522,
                modal: true,
                title: title,
                buttons: {
                    Confirm: function () {
                        window.location = getURL;
                    },
                    Cancel: function () {
                        $(this).dialog('close');
                    }
                }
            });
            $('.confirmDialog').dialog('open');
        }
</script>

<div class="confirmDialog" style="display:none;">
    <p>
        <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span>
        <span id="ConfirmMessage"></span></p>
</div>
