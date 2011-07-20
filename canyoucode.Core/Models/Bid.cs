using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using canyoucode.Core.Utils;
using AgileFx.ORM;

namespace canyoucode.Core.Models
{
    public partial class Bid
    {
        private const string BID_ACCEPTED_TEMPLATE = "BID_ACCEPTED";

        public void Accept()
        {
            this.Status = BID_STATUS.ACCEPTED;

            if (!this.IsLoaded(b => b.Company))
            {
                this.Load(this.EntityContext, b => b.Company);
                this.Load(this.EntityContext, b => b.Company.Account);
            }

            this.Project.Status = PROJECT_STATUS.CLOSED;

            string subject = string.Format("Your Quote for Project {0} has been accepted. Please contact the customer", this.Project.Title);
            NotificationUtil.SendSystemEmailWithTemplate(this.Company.Account.Email, subject, BID_ACCEPTED_TEMPLATE,
                this.Company.Name, GetQuote() , this.Project.Title, this.ProjectId.ToString(),
                this.Project.Employer.Account.Email, this.Project.Employer.Account.Phone);
        }

        public string GetQuote()
        {
            if (!this.IsLoaded(b => b.Project)) this.Load(this.EntityContext, b => b.Project);

            if (!(MinQuote.HasValue && MaxQuote.HasValue))
                return "Unspecified";
            else
                if (this.MaxQuote > 10000)
                    return Utils.I18NUtil.GetCurrencyChar(this.Project.Currency) + this.MinQuote / 1000 + "-" + this.MaxQuote / 1000 + "K";
                else
                    return Utils.I18NUtil.GetCurrencyChar(this.Project.Currency) + this.MinQuote + "-" + this.MaxQuote;
        }
    }
}
