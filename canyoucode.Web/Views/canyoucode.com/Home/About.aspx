<%@ Page Language="C#" MasterPageFile="~/Views/canyoucode.com/Shared/Site.Master" Inherits="DefaultViewPage<canyoucode.Web.ViewModels.CanYouCodeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    About: CanYouCode.com
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content about textual">
        <div style="width: 500px; padding-right: 410px; background: url(/images/anup.jpg) no-repeat 560px 0">
            <h1>
                Our Story and Vision</h1>
            <p>
                We, <strong>Anup Kesavan</strong> and <strong>Jeswin Kumar</strong> have been running
                a successful software company called <a href="http://www.agilehead.com">AgileHead</a>
                and have experienced first hand the difficulty in finding work over the internet.
                Our experience also tells us that it is equally difficult to find talented freelancers.
            </p>
            <p>
                While there are many freelancing websites, the rewards on these websites are so
                low (often less than $10/hour) that good developers find them unattractive.
            </p>
            <p style="font-size: 18px; font-weight: normal; padding: 20px;">
                CanYouCode wants to help brilliant talent find work that rewards them fairly for
                their skills.
            </p>
            <p>
                It is important that CanYouCode does not become another low-end freelancing website.
                Which means that we need to keep low quality competition out. We manually verify
                all companies which offer services here. Like how good their team is. Or maybe look
                at their Open Source contributions. And whether they have a standards-compliant,
                well-designed website.
            </p>
            <p>
                We made this for people who are passionate about code and design. This emphasis
                on quality will be what makes this website appealing to people posting projects.
                Which in turn benefits those offering services.
            </p>
            <p style="font-size: 18px; font-weight: normal; padding: 20px;">
                The quality of median developers should be higher than other freelancing sites.
            </p>
            <p>
                To help use scale better, we intend to bring crowdsourcing into quality control.
                Though, we are not sure how to implement this in way that guarantees fairness and
                avoids collusion.
            </p>
            <hr style="margin-top:20px" />
            <p style="margin-top:20px; font-weight: bold">
                CanYouCode is looking for early-stage investors. If you want to discuss this further,
                please email <a href="mailto:team@canyoucode.com">team@canyoucode.com</a>.
            </p>
        </div>
    </div>
</asp:Content>
