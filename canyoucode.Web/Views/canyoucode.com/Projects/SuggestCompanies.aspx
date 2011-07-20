<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.Projects.SuggestCompanies>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Suggest: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function sendInvites() {
            var ids = getSelectedItems();
            if (ids.length > 0) {
                $('#CompanyIds').val(ids.join(','));
                return true;
            } else {
                alert('Select atleast one company to send the invite.');
                return false;
            }
        }

        function getSelectedItems() {
            var ids = new Array();
            $(".selectCheckBox:checked").each(function () {
                var id = $(this).attr('id');
                if (id.toLowerCase().indexOf('select_') == 0) {
                    id = id.substr(7);
                }
                ids.push(id);
            });
            return ids;
        }
    </script>
    <div class="content suggest">
        <div class="standout">
            <h1>
                Do you want to invite companies to bid?
            </h1>
            <div class="buttonBar">
                <form action="/Projects/<%:Model.ProjectId %>/SendInvites" method="post" id="SendInvitesForm"
                name="SendInvitesForm" onsubmit="return sendInvites();">
                <%:Html.Hidden("CompanyIds") %>
                <a class="button" href="javascript:document.SendInvitesForm.submit()">
                    <img src="/images/buttons/send-invites.png" alt="send invites" /></a> <a class="link"
                        href="/<%:Model.LoggedInAccount.Username %>/Projects">No, I will wait for Bids</a>
                </form>
                <div class="clear">
                </div>
            </div>
            <h3 style="padding: 20px 0 12px 0">
                Here are our recommendations...</h3>
            <table class="companies">
                <colgroup>
                    <col width="28px" />
                    <col width="600px" />
                </colgroup>
                <%foreach (var company in Model.Companies)
                  { %>
                <tr class="developer">
                    <td style="vertical-align: top;">
                        <%:Html.CheckBox("select_" + company.Id, true, new { @class = "selectCheckBox" })%>
                    </td>
                    <td>
                        <div class="logo">
                            <img src="<%= company.Logo %>" alt="<%= company.Name %>" />
                        </div>
                        <div class="companyDetails">
                            <h3>
                                <a href="/<%:company.Account.Username %>">
                                    <%:company.Name %></a>
                            </h3>
                            <span class="location">
                                <%:company.City %>,
                                <%:company.Country %></span>
                            <p class="description">
                                <%:company.Description %></p>
                            <p class="skills">
                                <%foreach (var tag in company.Tags.Take(4))
                                  { %>
                                <span class="skill">
                                    <%:tag.Name %>
                                </span>
                                <%} %>
                                <%if (company.Tags.Count > 4)
                                  { %>
                                <span class="skill">
                                    <%:company.Tags.Count - 4 %>
                                    more.. </span>
                                <%} %></p>
                        </div>
                    </td>
                </tr>
                <% } %>
            </table>
            <div class="clear">
            </div>
        </div>
    </div>
</asp:Content>
