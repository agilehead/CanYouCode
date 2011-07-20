using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AgileFx.ORM;
using AgileFx.Security;
using canyoucode.Web.ViewModels;
using canyoucode.Core;
using canyoucode.Core.Models;
using u = canyoucode.Core.Utils;
using m = canyoucode.Core.Models;
using canyoucode.Web.Utils;
using Newtonsoft.Json;
using System.Transactions;
using canyoucode.Web.ViewModels.Companies;
using canyoucode.Core.Models.Tools;

namespace canyoucode.Web.Controllers
{
    public class InternalMarketing : CanYouCodeControllerBase
    {
        public InternalMarketing()
        {
        }

        private ActionResult PortfolioViewForMarketing(string username, string key)
        {
            var account = DbContext.Account.Single(a => a.Username == username && a.Type == ACCOUNT_TYPE.COMPANY);
            var marketingCampaign = DbContext.MarketingCampaign.
                SingleOrDefault(m => m.Account == account.Id && m.Token == new Guid(key) && m.Status == MARKETING_CAMPAIGN_STATUS.NEW);

            if (marketingCampaign != null)
            {
                return View<ViewItem>("IndexForMarketing", v =>
                {
                    v.Key = key;
                    v.Company = DbContext.Company
                        .LoadRelated(c => c.Portfolio, c => c.Consultants, c => c.Tags, c => c.Account)
                        .Where(c => c.Account.Username == username).Single();
                });
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        //This is for marketing, to create company accounts that are not active.
        public ActionResult CreateForMarketing()
        {
            return View<Signup>("CreateForMarketing");
        }

        //This is for marketing, to create company accounts that are not active.
        [HttpPost]
        public ActionResult CreateForMarketing(FormCollection coll)
        {
            var marketingCampaign = MarketingUtil.CreateMarketingCampaign(coll["CompanyName"], coll["Website"], coll["City"], coll["Country"], coll["Username"],
                coll["Email"], coll["Phone"], Request.Files, coll["Description"], Request.Files["Logo"],
                coll["ReferringURL"], (context, company) =>
                {

                    var portfolioEntryIds = coll.AllKeys.Where(k => k.StartsWith("Title_")).Select(t => Convert.ToInt32(t.Replace("Title_", ""))).ToList();
                    portfolioEntryIds.ForEach(id =>
                    {
                        company.AddToPortfolio(coll["Title_" + id], coll["Description_" + id], Request.Files["Image_" + id]);
                    });
                });

            return View<Signup>("CreateForMarketing", v =>
            {
                // can't pass parameters to show a message, so using the Add Error
                v.AddError(MessageCodes.ACCOUNT_CREATED_FOR_MARKETING,
                    string.Format("http://canyoucode.com/{0}?key={1}", coll["Username"], marketingCampaign.Token.ToString()),
                    coll["Email"]);
            });
        }

        public ActionResult Activate(string key, string username, string password, int minimumRate, string currency)
        {
            var account = DbContext.Account.LoadRelated(a => a.Company).Single(a => a.Username == username && a.Type == ACCOUNT_TYPE.COMPANY);
            var marketingCamp = DbContext.MarketingCampaign.SingleOrDefault(m => m.Account == account.Id && m.Token == new Guid(key));

            if (marketingCamp != null)
            {
                account.Password = CryptoUtil.HashPassword(password);
                account.Company.MinimumRate = minimumRate;
                account.Company.Currency = currency;
                account.Status = ACCOUNT_STATUS.ACTIVE;
                marketingCamp.Status = MARKETING_CAMPAIGN_STATUS.ACCEPTED;
                DbContext.SaveChanges();

                return Redirect("/" + username + "/Projects?Message=" + MessageCodes.ACCOUNT_ACTIVATED);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public ActionResult Deactivate(string key, string username)
        {
            var account = DbContext.Account.LoadRelated(a => a.Company, a => a.Company.Portfolio)
                .Single(a => a.Username == username);
            var marketingCamp = DbContext.MarketingCampaign.SingleOrDefault(m => m.Account == account.Id && m.Token == new Guid(key));

            if (marketingCamp != null)
            {
                
                marketingCamp.Status = MARKETING_CAMPAIGN_STATUS.DECLINED;
                DbContext.SaveChanges();

                return Redirect("/");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
