<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Tag>>" %>
<%
    var allTags = JsonConvert.SerializeObject(Tag.GetAll().Select(t => new
    {
        Value = t.Name,
        Key = t.Id
    }));
 %>
<script type="text/javascript">
        var tags = <%=allTags %>;
        $().ready(function () {
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
<%var tags = string.Join(", ", Model.Select(t => t.Name)); %>
<%:Html.TextArea("SearchTags", !string.IsNullOrEmpty(tags) ? tags + ", " : string.Empty, new { rows = 2, cols = 25 }, new Validation("Tags").Required().MaximumLength(200))%>
<%:Html.Hidden("SelectedTags", Model.Select(t => t.Id).GetCSV())%>
