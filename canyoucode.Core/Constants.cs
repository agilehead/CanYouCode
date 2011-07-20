using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace canyoucode.Core
{
    public struct ACCOUNT_STATUS
    {
        public const string ACTIVE = "Active";
        public const string DISABLED = "Disabled";
    }

    public struct ACCOUNT_TYPE
    {
        public const string COMPANY = "Company";
        public const string EMPLOYER = "Employer";
        public const string ADMIN = "Admin";
    }

    public struct BID_STATUS
    {
        public const string NEW = "New";
        public const string READ = "Read";
        public const string ACCEPTED = "Accepted";
        public const string WITHDRAWN = "Withdrawn";
    }

    public struct BID_TYPES
    {
        public const string INVITED = "Invited";
        public const string NORMAL = "Normal";
    }

    public struct COMPANY_PROJECTS
    {
        public const string WON = "Won";
        public const string LOST = "Lost";
    }

    public struct CONSULTANT_CREDENTIAL_TYPE
    {
        public const string MAIN = "Main";
        public const string OTHER = "Other";
    }

    public struct CREDENTIAL_TYPE
    {
        public const string STACKOVERFLOW = "Stackoverflow";
        public const string GITHUB = "Github";
        public const string BLOG = "Blog";
        public const string HACKERNEWS = "Hackernews";
    }

    public struct CURRENCY
    {
        public const string USD = "USD";
        public const string EURO = "EURO";
    }

    public struct COMPANY_TYPE
    {
        public const string INDIVIDUAL = "Individual";
        public const string COMPANY = "Company";
        
        public static string[] GetAll()
        {
            return new[] { INDIVIDUAL, COMPANY };
        }
    }

    public struct DEFAULT_IMAGES
    {
        public const string COMPANY_LOGO = "/images/company-logo.png";
        public const string PROFILE_PICTURE = "/images/profile-pic.png";
        public const string PAGE_PLACEHOLDER = "/images/page-placeholder.png";
        public const string PAGE_PLACEHOLDER2 = "/images/page-placeholder2.png";
        public const string PAGE_PLACEHOLDER3 = "/images/page-placeholder3.png";
    }

    public struct EMAIL_TEMPLATES
    {
        public const string PROJECT_INVITE_TEMPLATE = "NEW_PROJECT_INVITE";
        public const string NEW_BID_TEMPLATE = "NEW_BID";
        public const string BID_WITHDRAWN = "BID_WITHDRAWN";
        public const string RESET_PASSWORD = "RESET_PASSWORD";
        public const string CONTACT_COMPANY = "CONTACT_COMPANY";
        public const string CYC_INVITE = "CYC_INVITE";
    }

    public struct MARKETING_CAMPAIGN_STATUS
    {
        public const string NEW = "New";
        public const string ACCEPTED = "Accepted";
        public const string DECLINED = "Declined";
    }

    public struct PORTFOLIO_ENTRY_TYPE
    {
        public const string HTML = "Html";
        public const string IMAGE = "Image";
    }

    public struct PROJECT_INVITE_STATUS
    {
        public const string NEW = "New";
        public const string ACCEPTED = "Accepted";
        public const string DECLINED = "Declined";
    }
    
    public struct PROJECT_STATUS
    {
        public const string NEW = "NEW";
        public const string CLOSED = "CLOSED";
    }

    public struct PUBLIC_STORE
    {
        public const string PROJECT = "Project";
        public const string COMPANY = "Company";
        public const string PORTFOLIO = "Portfolio";
        public const string PROFILE = "Profile";
    }

    public struct SEND_MESSAGE_TYPE
    {
        public const string FEEDBACK = "Feedback";
        public const string INVITE = "Invite people to CanYouCode.com";
    }

    public struct TAG_TYPE
    {
        public const string COMPANIES = "Companies";
        public const string PROJECTS = "Projects";
    }

    public struct TIMEFRAME
    {
        public const string WEEKS_1 = "1 Week";
        public const string WEEKS_2 = "2 Weeks";
        public const string WEEKS_3 = "3 Weeks";
        public const string WEEKS_4 = "4 Weeks";
        public const string WEEKS_6 = "6 Weeks";
        public const string MONTHS_2 = "2 Months";
        public const string MONTHS_3 = "3 Months";
        public const string MONTHS_6 = "6 Months";
        public const string UNSPECIFIED = "Unspecified Timeframe";

        public static string[] GetAll()
        {
            return new[] { WEEKS_1, WEEKS_2, WEEKS_3, WEEKS_4, WEEKS_6, MONTHS_2, MONTHS_3, MONTHS_6, UNSPECIFIED };
        }
    }

    public struct PORTFOLIO_STYLE
    {
        public const string SIMPLE = "Simple";
        public const string ZURB_ORBIT = "Zurb_Orbit";

        public static string[] GetAll()
        {
            return new[] { SIMPLE };
        }
    }

    public struct TOKEN_TYPE
    {
        public const string PASSWORD_RESET = "PASSWORD_RESET";
        public const string MARKETING_INVITE = "MARKETING_INVITE";
    }

    public struct ALERT_STATUS
    {
        public const string NEW = "NEW";
        public const string VIEWED = "VIEWED";
    }

    public struct ACCOUNT_ALERT_TYPE
    {
        public const string NORMAL = "Normal";
    }
}
