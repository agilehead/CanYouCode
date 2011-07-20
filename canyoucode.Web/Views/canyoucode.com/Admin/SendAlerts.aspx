<%@ Page Title="" Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SendAlerts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content">
        <div class="form standard wide standout">
            <form action="/Admin/SendAlerts" method="post" enctype="multipart/form-data"
            id="CreateForm" onsubmit="return validateForm('CreateForm');">
            <h1>
                Send Alerts</h1>
            <div class="section">
                <ul>
                    <li>
                        <label for="Type">To</label>
                        
                        <%:Html.RadioButton("Type", ACCOUNT_TYPE.COMPANY, true)%>
                        Company
                        <%:Html.RadioButton("Type", ACCOUNT_TYPE.EMPLOYER, false)%>
                        Employer 
                    </li>
                    <li>
                        <label for="CompanyName">
                            Message</label>
                        <%:Html.TextBox("Message", "", new { size = 32 }, new Validation("CompanyName").Required())%>
                    </li>
                </ul>
            </div>
            <input type="submit" value="Send" />
            </form>
        </div>
    </div>
</asp:Content>

