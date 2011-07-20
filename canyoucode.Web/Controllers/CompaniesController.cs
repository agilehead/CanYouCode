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
using AgileFx.MVC.Utils;
using canyoucode.Core.Utils;
using System.Web.Security;
using Microsoft.Security.Application;

namespace canyoucode.Web.Controllers
{
    public partial class CompaniesController : CanYouCodeControllerBase
    {
        Company Company;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (User.Identity.IsAuthenticated && this.LoggedInAccount != null && this.LoggedInAccount.Type == ACCOUNT_TYPE.COMPANY)
            {
                this.Company = DbContext.Company.Where(c => c.Account == LoggedInAccount && c.TenantId == Tenant.Id)
                    .LoadRelated(c => c.Portfolio, c => c.Account, c => c.Consultants, c => c.Tags)
                    .Single();
            }
        }

        public ActionResult Signup()
        {
            return View<Signup>(GetViewName("Signup"));
        }

        [HttpPost]
        public ActionResult Signup(FormCollection coll)
        {
            var type = coll["Type"];
            var companyName = type == COMPANY_TYPE.INDIVIDUAL ? coll["Fullname"] : coll["CompanyName"];

            var company = Company.Create(companyName, Common.FixUrl(coll["Website"]), coll["City"], coll["Country"], coll["Username"], coll["Password"],
                coll["MinimumRate"].ToNullable<int>(), coll["Currency"], coll["Email"], coll["Phone"], type, Tenant.Id, DbContext);
            company.AddConsultant(coll["Fullname"], coll["Designation"], Common.FixUrl(coll["LinkedIn"]), null, null, null, null);

            DbContext.SaveChanges();

            Response.Cookies.Add(AuthHelper.GetAuthTicketWithRoles(company.Account.Username, company.Account.Type, true, new TimeSpan(0, 30, 0)));
            return Redirect(string.Format("/{0}/Edit", company.Account.Username));
        }

        //This displays the company's profile.
        public ActionResult ViewItem(string username, string style)
        {
            var company = DbContext.Company
                    .LoadRelated(c => c.Portfolio, c => c.Account, c => c.Consultants, c => c.Tags)
                    .LoadRelatedInCollection(c => c.Consultants, con => con.Credentials)
                    .Where(c => c.Account.Username == username).Single();

            
            if (company.Account.Status == ACCOUNT_STATUS.DISABLED)
                if (!(LoggedInAccount != null && LoggedInAccount.Username == username))
                    return Redirect(string.Format("/?Message={0}", MessageCodes.ACCOUNT_NOT_ACTIVE));

            return View<ViewItem>(GetViewName("ViewItem-" + (!string.IsNullOrEmpty(style) ? style : company.Style.ToLower())), v =>
                {
                    v.TopMenuSelectedItem = TopMenuSelection.COMPANIES_PROFILE_VIEW;
                    v.Company = company;
                    v.ShowFixedSidePane = !(LoggedInAccount != null && LoggedInAccount.Type == ACCOUNT_TYPE.COMPANY);
                });
        }

        [Authorize(Roles = ACCOUNT_TYPE.COMPANY)]
        public ActionResult ChangeStyle(string style)
        {
            var company = DbContext.Company.Where(c => c.Account.Username == LoggedInAccount.Username).Single();
            company.ChangeStyle(style);

            DbContext.SaveChanges();

            return Redirect("/" + LoggedInAccount.Username);
        }

        //Profile Edit.
        [Authorize(Roles = ACCOUNT_TYPE.COMPANY)]
        public ActionResult Edit(string id)
        {
            Company.LoadInCollection(DbContext, c => c.Consultants, con => con.Credentials);
            return View<Edit>(GetViewName("Edit"), v =>
            {
                v.Company = Company;
                v.TopMenuSelectedItem = TopMenuSelection.COMPANIES_PROFILE_EDIT;
                v.Countries = UIHelper.GetCountrySelectItemList(Company.Country);
                v.Currency = UIHelper.GetCurrencySelectItemList(Company.Currency);
                v.MinimumRates = UIHelper.GetMinimumRatesSelectItemList(Company.MinimumRate.HasValue ? Company.MinimumRate.ToString() : "Unspecified");
                v.ShowFixedSidePane = !(LoggedInAccount != null && LoggedInAccount.Type == ACCOUNT_TYPE.COMPANY);
            });
        }

        [Authorize(Roles = ACCOUNT_TYPE.COMPANY)]
        public ActionResult Save(string city, string country, string description, string phone, string email,
            string selectedTags, string searchTags, string minimumRate, string name)
        {
            int? minRate = null;
            if (minimumRate.ToLower() != "unspecified")
            {
                minRate = int.Parse(minimumRate);
            }
            
            var file = Request.Files.Count > 0 ?  Request.Files[0] as HttpPostedFileBase : null;

            //we will take only 20.
            var tagIds = selectedTags.Split(new char[] { ',' }).Where(t => !string.IsNullOrEmpty(t)).Select(x => Int64.Parse(x)).Take(20);
            Company.Update(city, country, description, phone, email, tagIds, searchTags.Split(','), minRate, file, name);
            
            DbContext.SaveChanges();

            return Redirect(string.Format("/{0}/Edit?Message={1}#Summary", LoggedInAccount.Username, MessageCodes.SUMMARY_SAVED));
        }

        [Authorize(Roles = ACCOUNT_TYPE.COMPANY)]
        public ActionResult AddImageAndDescriptionPage(FormCollection coll)
        {
            var portfolio = Company.Add_ImageAndDescription_Page(coll["Title"], coll["Description"], Request.Files[0]);
            DbContext.SaveChanges();

            return Redirect(string.Format("/{0}/Edit?Message={1}#Portfolio{2}", LoggedInAccount.Username,
                MessageCodes.PORTFOLIO_ENTRY_SAVED, portfolio.Id));
        }

        [Authorize(Roles = ACCOUNT_TYPE.COMPANY)]
        public ActionResult EditImageAndDescriptionPage(long portfolioEntryId, string title, string description)
        {
            Company.Update_ImageAndDescription_Page(portfolioEntryId, title, description, Request.Files[0]);
            DbContext.SaveChanges();

            return Redirect(string.Format("/{0}/Edit?Message={1}#Portfolio{2}", LoggedInAccount.Username,
                MessageCodes.PORTFOLIO_ENTRY_SAVED, portfolioEntryId));
        }

        [Authorize(Roles = ACCOUNT_TYPE.COMPANY)]
        [ValidateInput(false)]
        public ActionResult AddHtmlPage(FormCollection coll)
        {
            var portfolio = Company.Add_Html_Page(AntiXss.GetSafeHtmlFragment(coll["AddPortfolioHtmlTitle"]), AntiXss.GetSafeHtmlFragment(coll["AddPortfolioHtml"]));
            DbContext.SaveChanges();

            return Redirect(string.Format("/{0}/Edit?Message={1}#Portfolio{2}", LoggedInAccount.Username,
                MessageCodes.PORTFOLIO_ENTRY_SAVED, portfolio.Id));
        }

        [Authorize(Roles = ACCOUNT_TYPE.COMPANY)]
        [ValidateInput(false)]
        public ActionResult EditHtmlPage(long portfolioEntryId, string title, string html)
        {
            Company.Update_Html_Page(portfolioEntryId, title, AntiXss.GetSafeHtmlFragment(html));
            DbContext.SaveChanges();

            return Redirect(string.Format("/{0}/Edit?Message={1}#Portfolio{2}", LoggedInAccount.Username,
                MessageCodes.PORTFOLIO_ENTRY_SAVED, portfolioEntryId));
        }

        [CheckReferrer]
        [Authorize(Roles = ACCOUNT_TYPE.COMPANY)]
        public ActionResult DeletePortfolioEntry(long Id)
        {
            Company.DeletePortfolioEntry(Id);
            DbContext.SaveChanges();

            string redirectTab = Company.Portfolio.Count > 0 ? "Portfolio" + Company.Portfolio.Last().Id : "PortfolioNew";

            return Redirect(string.Format("/{0}/Edit?Message={1}#{2}", LoggedInAccount.Username,
                MessageCodes.PORTFOLIO_ENTRY_SAVED, redirectTab));
        }

        [Authorize(Roles = ACCOUNT_TYPE.COMPANY)]
        public ActionResult AddConsultant(string name, string designation, string linkedinProfile, string blog, string hackernews, string stackoverflow, string github)
        {
            var consultant = Company.AddConsultant(name, designation, Common.FixUrl(linkedinProfile),
                Common.FixUrl(blog), Common.FixUrl(hackernews), Common.FixUrl(stackoverflow), Common.FixUrl(github));

            if (Request.Files.Count > 0)
                consultant.SavePicture(Request.Files[0] as HttpPostedFileBase);

            DbContext.SaveChanges();

            return Redirect(string.Format("/{0}/Edit?Message={1}#People", LoggedInAccount.Username, MessageCodes.CONSULTANT_ADDED));
        }

        [Authorize(Roles = ACCOUNT_TYPE.COMPANY)]
        public ActionResult EditConsultant(long id, string name, string designation, string linkedinProfile, string blog, string hackernews, string stackoverflow, string github)
        {
            var consultant = DbContext.Consultant.LoadRelated(c => c.Company).Single(c => c.Id == id && c.CompanyId == Company.Id);

            if ((consultant.Company.Type == COMPANY_TYPE.COMPANY && string.IsNullOrWhiteSpace(name)) || 
                (consultant.Company.Type == COMPANY_TYPE.COMPANY && string.IsNullOrWhiteSpace(designation))
                || string.IsNullOrWhiteSpace(linkedinProfile)) throw new Exception();
            
            consultant.Update(name, designation, Common.FixUrl(linkedinProfile), Common.FixUrl(blog),
                Common.FixUrl(hackernews), Common.FixUrl(stackoverflow), Common.FixUrl(github),
                Request.Files[0] as HttpPostedFileBase);
            DbContext.SaveChanges();

            return Redirect(string.Format("/{0}/Edit?Message={1}#People", LoggedInAccount.Username, MessageCodes.CONSULTANT_SAVED));
        }

        [Authorize(Roles = ACCOUNT_TYPE.COMPANY)]
        [CheckReferrer]
        public ActionResult DeleteConsultant(long id)
        {
            if (Company.Consultants.Count <= 1) throw new Exception();
            var consultant = DbContext.Consultant.Single(c => c.Id == id && c.CompanyId == Company.Id);
            consultant.Delete(DbContext);
            DbContext.SaveChanges();

            return Redirect(string.Format("/{0}/Edit?Message={1}#People", LoggedInAccount.Username, MessageCodes.CONSULTANT_REMOVED));
        }

        public ActionResult Projects(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return View<Projects>(GetViewName("Projects"), v =>
                    {
                        v.TopMenuSelectedItem = TopMenuSelection.COMPANIES_PROJECTS;
                        v.Invites = Company.GetInvites();
                        v.Bids = Company.GetActiveBids();
                    });
            }
            else if (filter == COMPANY_PROJECTS.WON)
            {
                return View<Projects>(GetViewName("BidsWon"), v =>
                {
                    v.TopMenuSelectedItem = TopMenuSelection.COMPANIES_PROJECTS;
                    v.Bids = Company.GetWonBids();
                });
            }
            else if (filter == COMPANY_PROJECTS.LOST)
            {
                return View<Projects>(GetViewName("BidsLost"), v =>
                {
                    v.TopMenuSelectedItem = TopMenuSelection.COMPANIES_PROJECTS;
                    v.Bids = Company.GetLostBids();
                });
            }
            else throw new NotImplementedException();

        }

        public ActionResult PlaceBid(long biddingProjectId, int hoursOfEffort, string timeFrame, string quote, string message)
        {
            var minQuote = !string.IsNullOrEmpty(quote) ? quote.Split('-')[0].ToNullable<int>() : null;
            var maxQuote = !string.IsNullOrEmpty(quote) ? quote.Split('-')[1].ToNullable<int>() : null;
            try
            {
                this.Company.PlaceBid(biddingProjectId, hoursOfEffort, timeFrame, minQuote, maxQuote, message);
                DbContext.SaveChanges();
                return Redirect("/" + LoggedInAccount.Username + "/Projects?Message=" + MessageCodes.BID_PLACED);
            }
            catch (Exception ex)
            {
                return Redirect("/Projects/" + biddingProjectId + "/?Message=" + ex.Message);
            }
        }

        [CheckReferrer]
        public ActionResult DeclineInvite(long projectId, string returnUrl)
        {
            this.Company.DeclineInvite(projectId);
            DbContext.SaveChanges();
            return Redirect(returnUrl + "?Message=" + MessageCodes.DECLINED_INVITE);
        }

        [CheckReferrer]
        public ActionResult WithdrawBid(long bidId, string returnUrl)
        {
            this.Company.WithdrawBid(bidId);
            DbContext.SaveChanges();
            return Redirect(returnUrl + "?Message=" + MessageCodes.BID_WITHDRAWN);
        }

        public ActionResult Preview(string key)
        {
            var token = DbContext.Token.FirstOrDefault(t => t.Key == key && t.Type == TOKEN_TYPE.MARKETING_INVITE);

            if (token != null)
            {
                var account = DbContext.Account.Single(a => a.Username == token.Data);
                Response.Cookies.Add(AuthHelper.GetAuthTicketWithRoles(account.Username, account.Type, true, new TimeSpan(0, 30, 0)));
                Response.Cookies.Add(new HttpCookie("PreviewMode", token.Key) { Expires = DateTime.Now.Add(new TimeSpan(0, 30, 0)) } );

                try
                {
                    NotificationUtil.SendAdminEmail(string.Format("Marketing Account, Preview - Username {0}", account.Username), "");
                }
                catch { }

                return Redirect("/" + account.Username);
            }
            else
            {
                return Redirect("/Login");
            }
        }

        public ActionResult ActivateAccount(string key)
        {
            var token = DbContext.Token.FirstOrDefault(t => t.Key == key && t.Type == TOKEN_TYPE.MARKETING_INVITE);
            
            if (token != null)
            {
                token.Delete(DbContext);
                var account = DbContext.Account.Single(a => a.Username == token.Data);
                account.Status = ACCOUNT_STATUS.ACTIVE;
                DbContext.SaveChanges();
                Response.Cookies["PreviewMode"].Expires = DateTime.Now;

                try
                {
                    NotificationUtil.SendAdminEmail(string.Format("Marketing Account, Activate - Username {0}", account.Username), "");
                }
                catch { }

                return Redirect("/" + account.Username + "/SetupAccount");
            }
            else
            {
                return Redirect("/Login");
            }
        }

        public ActionResult DeleteAccount(string key)
        {
            var token = DbContext.Token.FirstOrDefault(t => t.Key == key && t.Type == TOKEN_TYPE.MARKETING_INVITE);

            if (token != null)
            {
                var account = DbContext.Account.LoadRelated(a => a.Company.Portfolio, a => a.Company.Consultants, a => a.Company.Bids)
                    .Single(a => a.Username == token.Data);
                account.Company.Portfolio.ToList().ForEach(p => p.Delete(DbContext));
                account.Company.Consultants.ToList().ForEach(c => c.Delete(DbContext));
                account.Company.Bids.ToList().ForEach(b => b.Delete(DbContext));
                account.Company.Delete(DbContext);
                account.Delete(DbContext);
                token.Delete(DbContext);
                DbContext.SaveChanges();
                Response.Cookies["PreviewMode"].Expires = DateTime.Now;
                FormsAuthentication.SignOut();

                try
                {
                    NotificationUtil.SendAdminEmail(string.Format("Marketing Account, Delete - Username {0}", account.Username), "");
                }
                catch { }

                return Redirect(string.Format("/?Message={0}", MessageCodes.ACCOUNT_REMOVED));
            }
            else
            {
                return Redirect("/Login");
            }
        }


        [Authorize]
        public ActionResult SetupAccount()
        {
            return View(GetViewName("SetupAccount"));
        }

        [HttpPost]
        [Authorize]
        public ActionResult SetupAccount(string password)
        {
            //We should allow this only if the password is set as ""
            //This condition will exist only when we send out marketing invites.
            if (LoggedInAccount.Password != "")
                throw new Exception("Cannot overwrite password.");

            LoggedInAccount.Password = CryptoUtil.HashPassword(password);
            DbContext.SaveChanges();
            return Redirect("/" + LoggedInAccount.Username);
        }

        [Authorize]
        [CheckReferrer]
        public ActionResult ChangeStatus()
        {
            Company.ChangeStatus();
            DbContext.SaveChanges();
            return Redirect(string.Format("/{0}/Edit?Message={1}", LoggedInAccount.Username, 
                Company.Account.Status == ACCOUNT_STATUS.ACTIVE ? MessageCodes.ACCOUNT_ACTIVATED:MessageCodes.ACCOUNT_DEACTIVATED));
        }
    }
}
