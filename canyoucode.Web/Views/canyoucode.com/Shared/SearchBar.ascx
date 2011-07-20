<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<canyoucode.Web.ViewModels.Search.Base>" %>
<script type="text/javascript">
        var tags = <%=Model.Tags %>;
        $().ready(function () {
            $("#SearchTags").autocomplete(tags, {
                dataType: 'json',
                multiple: true,
                mustMatch: true,
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

            $("#SearchTags").change(function(){
                if ($("#SelectedTags").val().length > 0)
                {
                    var selectedtags = $("#SearchTags").val().split(',');

                    var tagIds = [];
                    $.each(selectedtags, function (index, val) {
                        $.each(tags, function (i, v) {
                            if ($.trim(v.Value) == $.trim(val)) {
                                tagIds.push(v.Key);
                            }
                        });
                    });

                    $("#SelectedTags").val(tagIds.join(","))      
                }      
            });
            
        });
</script>
<div class="searchbar form standard">
    <h1>
        Filter Results</h1>
    <form action="" id="SearchForm" method="post" onsubmit="return validateForm('SearchForm');">
    <p>
        <label for="Tag">
            Skill</label><br />
        <%:Html.TextArea("SearchTags", Model.SelectedTags, new { rows = 3, cols = 26 })%>
        <%:Html.Hidden("SelectedTags", Model.SelectedTagIds)%>
        <br />
        <span class="subText">Web Design, PHP, Java, LISP etc</span>
    </p>
    <p>
        <%:Html.DropDownList("Country", UIHelper.GetCountryWithAllSelectItemList(Model.SelectedCountry))%>
    </p>
    <p>
        <input type="submit" value="Filter" />
    </p>
    </form>
</div>
