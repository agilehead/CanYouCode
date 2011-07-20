using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileFx.ORM;
using System.Web;
using canyoucode.Core.Utils;
using System.IO;

namespace canyoucode.Core.Models
{
    public partial class Company
    {
        public static Company Create(string companyName, string website, string city, string country, string username, string password,
            int? minimumRate, string currency, string email, string phone, string type, long tenantId, Entities context)
        {
            username = username.Trim();
            if (string.IsNullOrEmpty(username))
                throw new Exception("The username cannot be empty");
            
            if (Account.Exists(username, tenantId))
                throw new Exception("The username already exists.");

            var account = new Account();
            account.Username = username;
            account.Password = AgileFx.Security.CryptoUtil.HashPassword(password);
            account.Status = ACCOUNT_STATUS.ACTIVE;
            account.LastLoginDate = DateTime.Now;
            account.DateAdded = DateTime.Now;
            account.Type = ACCOUNT_TYPE.COMPANY;
            account.Email = email;
            account.Phone = phone;
            account.TenantId = tenantId;

            var company = new Company();
            company.Account = account;
            company.Name = companyName;
            company.Website = website;
            company.City = city;
            company.Country = country;
            company.MinimumRate = minimumRate;
            company.Currency = currency;
            company.Logo = DEFAULT_IMAGES.COMPANY_LOGO;
            company.Type = type;
            company.Description = "";
            company.Style = PORTFOLIO_STYLE.SIMPLE;
            company.TenantId = tenantId;

            if (type == COMPANY_TYPE.INDIVIDUAL)
                company.Logo = DEFAULT_IMAGES.PROFILE_PICTURE;

            foreach (var tag in context.Tag.Where(t => t.Name == "Web Design" || t.Name == ""))
                company.Tags.Add(tag);

            //Add a few portfolio entries
            company.Add_ImageAndDescription_Page(PORTFOLIO_ENTRY_TYPE.IMAGE, 
                "Sample 1",
                "",
                DEFAULT_IMAGES.PAGE_PLACEHOLDER);

            company.Add_ImageAndDescription_Page(PORTFOLIO_ENTRY_TYPE.IMAGE,
                "Sample 2",
                "",
                DEFAULT_IMAGES.PAGE_PLACEHOLDER2);

            company.Add_ImageAndDescription_Page(PORTFOLIO_ENTRY_TYPE.IMAGE,
                "Sample 3",
                "",
                DEFAULT_IMAGES.PAGE_PLACEHOLDER3);

            //var projects = context.Project.Where(p => p.Employer.Account.Username == "bouncethru");
            //create an active bid for one of the sample projects.
            //var activeProject = projects.Where(p => p.ClosingDate >= DateTime.Now).First();
            //company.PlaceBid(activeProject.Id, 200, TIMEFRAME.MONTHS_2, 10000, 50000, "I am bidding on this sample project.");

            context.AddObject(company);

            try {
                NotificationUtil.SendAdminEmail(string.Format("Company Signup - Username {0}", username), "");
            }
            catch { }

            return company;
        }

        public void Update(string city, string country, string description, string phone, string email,
            IEnumerable<long> tagIds, IEnumerable<string> tagNames, int? minimumRate, HttpPostedFileBase image, string consultantName = null)
        {
            //clear existing tags
            Tags.Clear();

            City = city;
            Country = country;
            Description = description;
            Account.Phone = phone;
            Account.Email = email;

            var tags =  EntityContext.CreateQuery<Tag>().Where(t => tagIds.Contains(t.Id)).ToList();
            tags.ForEach(t => Tags.Add(t));

            if (tagNames != null && tagNames.Count() > 0)
            {
                var existingTagNames = tags.Select(t => t.Name);
                var newTags = tagNames.Select(n => n.Trim()).Where(tn => !string.IsNullOrEmpty(tn) && !existingTagNames.Contains(tn))
                    .Distinct().Take(10).ToList();

                if (newTags.Count() > 0) newTags.ForEach(t => Tags.Add(Tag.Create(t, TenantId, EntityContext)));
            }

            MinimumRate = minimumRate;
            Currency = CURRENCY.USD;

            SaveLogo(image);

            Name = consultantName;
            if (Type == COMPANY_TYPE.INDIVIDUAL)
            {
                if (!this.IsLoaded(c => c.Consultants)) this.Load(this.EntityContext, c => c.Consultants);
                Consultants.Single().Name = consultantName;
            }
        }

        public static IQueryable<Company> ActiveCompanies(long tenantId, Entities context)
        {
            return context.CreateQuery<Company>().Where(c => c.Account.Status != ACCOUNT_STATUS.DISABLED && c.TenantId == tenantId);
        }

        public IEnumerable<ProjectInvite> GetInvites()
        {
            var inviteInfo = this.DbContext().ProjectInvite
            .LoadRelated(i => i.Project.Employer.Account, i => i.Project.Tags)
            .Where(i => i.Company == this && i.Status == PROJECT_INVITE_STATUS.NEW)
            .Select(i => new
            {
                Invite = i,
                TotalBidsOnProject = i.Project.Bids.Count(),
                TotalProjectsForEmployer = i.Project.Employer.Projects.Count()
            });

            var invites = new List<ProjectInvite>();
            foreach (var info in inviteInfo)
            {
                invites.Add(info.Invite);
                info.Invite.Project.TotalBids = info.TotalBidsOnProject;
                info.Invite.Project.Employer.TotalProjects = info.TotalProjectsForEmployer;
            }
            return invites;
        }

        public IEnumerable<Bid> GetActiveBids()
        {
            var bidInfo = this.DbContext().Bid
                .LoadRelated(b => b.Project.Employer.Account, b => b.Project.Tags)
                .Where(b => b.Company == this && b.Status == BID_STATUS.NEW && b.Project.ClosingDate >= DateTime.Now)
                .OrderByDescending(b => b.DateCreated)
                .Select(b => new
                    {
                        Bid = b,
                        TotalBidsOnProject = b.Project.Bids.Count(),
                        TotalProjectsForEmployer = b.Project.Employer.Projects.Count()
                    });

            var bids = new List<Bid>();
            foreach (var info in bidInfo)
            {
                bids.Add(info.Bid);
                info.Bid.Project.TotalBids = info.TotalBidsOnProject;
                info.Bid.Project.Employer.TotalProjects = info.TotalProjectsForEmployer;
            }
            return bids;
        }

        public IEnumerable<Bid> GetWonBids()
        {
            var bidInfo = this.EntityContext.CreateQuery<Bid>()
                .LoadRelated(b => b.Project.Employer.Account, b => b.Project.Tags)
                .Where(b => b.Company == this && b.Status == BID_STATUS.ACCEPTED)
                .OrderByDescending(b => b.DateCreated)
                .Select(b => new
                {
                    Bid = b,
                    TotalBidsOnProject = b.Project.Bids.Count(),
                    TotalProjectsForEmployer = b.Project.Employer.Projects.Count()
                });

            var bids = new List<Bid>();
            foreach (var info in bidInfo)
            {
                bids.Add(info.Bid);
                info.Bid.Project.TotalBids = info.TotalBidsOnProject;
                info.Bid.Project.Employer.TotalProjects = info.TotalProjectsForEmployer;
            }
            return bids;
        }

        public IEnumerable<Bid> GetLostBids()
        {
            var bidInfo = this.DbContext().Bid
                .LoadRelated(b => b.Project.Employer.Account, b => b.Project.Tags, b => b.Project.Bids)
                .Where(b => b.Company == this && b.Status == BID_STATUS.NEW && b.Project.ClosingDate <= DateTime.Now)
                .OrderByDescending(b => b.DateCreated)
                .Select(b => new
                {
                    Bid = b,
                    TotalBidsOnProject = b.Project.Bids.Count(),
                    TotalProjectsForEmployer = b.Project.Employer.Projects.Count(),
                    WiningBid = b.Project.Bids.SingleOrDefault(wb => wb.Status == BID_STATUS.ACCEPTED)
                });

            var bids = new List<Bid>();
            foreach (var info in bidInfo)
            {
                bids.Add(info.Bid);
                info.Bid.Project.TotalBids = info.TotalBidsOnProject;
                info.Bid.Project.Employer.TotalProjects = info.TotalProjectsForEmployer;
                info.Bid.Project.WinningBid = info.WiningBid;
            }
            return bids;
        }

        public string GetMinimumRate()
        {
            return Utils.I18NUtil.GetCurrencyChar(this.Currency) + this.MinimumRate + "/hr";
        }

        public void PlaceBid(long projectId, int hoursOfEffort, string timeframe, int? minQuote, int? maxQuote, string message)
        {
            if (!this.IsLoaded(c => c.Account)) this.Load(this.EntityContext, c => c.Account);
            if (this.Account.Status == ACCOUNT_STATUS.DISABLED) throw new Exception(MessageCodes.ACTIVATE_ACCOUNT_TO_BID);

            var project = this.DbContext().Project
                .LoadRelated(p => p.Employer.Account)
                .Where(p => p.Id == projectId).Single();

            if (!this.IsLoaded(c => c.Account))
                this.Load(this.EntityContext, c => c.Account);

            if (!this.IsLoaded(c => c.ProjectInvites))
            {
                this.Load(this.EntityContext, c => c.ProjectInvites);
                this.LoadInCollection(this.EntityContext, c => c.ProjectInvites, pi => pi.Project);
            }

            var bid = new Bid();
            bid.Company = this;
            bid.Project = project;
            bid.DateCreated = DateTime.Now;
            bid.Invited = false;
            bid.Message = message;
            bid.MinQuote = minQuote;
            bid.MaxQuote = maxQuote;
            bid.Status = BID_STATUS.NEW;
            bid.HoursOfEffort = hoursOfEffort;
            bid.Timeframe = timeframe;
            bid.TenantId = TenantId;

            string subject = string.Format("You have received a new bid for Project - {0}", project.Title);
            NotificationUtil.SendSystemEmailWithTemplate(project.Employer.Account.Email, subject, EMAIL_TEMPLATES.NEW_BID_TEMPLATE,
                project.Employer.Name, this.Name, this.Id.ToString(),
                string.Format("{0}, {1} {2}", this.Name, this.City, this.Country), this.Account.Username);

            NotificationUtil.SendAdminEmail(string.Format("Bid placed by {0} ID:{1} on Project {2} ID:{3}",
                bid.Company.Name, bid.CompanyId, bid.Project.Title, bid.ProjectId), "");

            if (this.ProjectInvites.Any(pi => pi.Status == PROJECT_INVITE_STATUS.NEW && pi.ProjectId == projectId))
            {
                bid.Invited = true;
                this.ProjectInvites.Single(pi => pi.ProjectId == projectId).Status = PROJECT_INVITE_STATUS.ACCEPTED;
            }

            this.EntityContext.AddObject(bid);
        }

        public void DeclineInvite(long projectId)
        {
            if (!this.IsLoaded(c => c.ProjectInvites)) this.Load(EntityContext, c => c.ProjectInvites);

            if (this.ProjectInvites.Any(pi => pi.Status == PROJECT_INVITE_STATUS.NEW && pi.ProjectId == projectId))
            {
                this.ProjectInvites.Single(pi => pi.ProjectId == projectId).Status = PROJECT_INVITE_STATUS.DECLINED;
            }
        }

        public void WithdrawBid(long bidId)
        {
            var bid = this.DbContext().Bid
                .LoadRelated(b => b.Project, b => b.Project.Employer.Account, b => b.Company.Account)
                .Single(b => b.Id == bidId);
            bid.Status = BID_STATUS.WITHDRAWN;

            string subject = string.Format("Bid for Project - {0}, has been withdrawn by {1}", bid.Project.Title, this.Name);
            NotificationUtil.SendSystemEmailWithTemplate(bid.Project.Employer.Account.Email, subject, EMAIL_TEMPLATES.BID_WITHDRAWN,
                bid.Project.Employer.Name, bid.Project.Title, bid.ProjectId.ToString(), bid.Company.Name, bid.Company.Account.Username);
        }

        public PortfolioEntry Add_ImageAndDescription_Page(string type, string title, string description, string imagePath = null)
        {
            var entry = new PortfolioEntry();
            entry.Type = type;
            entry.Title = title;
            entry.Description = description;
            entry.TenantId = TenantId;

            if (imagePath != null)
                entry.Image = imagePath;

            Portfolio.Add(entry);

            return entry;
        }

        public PortfolioEntry Add_ImageAndDescription_Page(string title, string description, HttpPostedFileBase image)
        {
            var entry = new PortfolioEntry();
            entry.Type = PORTFOLIO_ENTRY_TYPE.IMAGE;
            entry.Title = title;
            entry.Description = description;
            entry.TenantId = TenantId;

            if (image != null && image.ContentLength > 0)
                entry.SaveImage(image);

            Portfolio.Add(entry);

            return entry;
        }

        public void Update_ImageAndDescription_Page(long id, string title, string description, HttpPostedFileBase image)
        {
            var entry = this.Portfolio.Single(e => e.Id == id);
            entry.Type = PORTFOLIO_ENTRY_TYPE.IMAGE;
            entry.Title = title;
            entry.Description = description;

            if (image != null && image.ContentLength > 0)
                entry.SaveImage(image);
        }

        public PortfolioEntry Add_Html_Page(string title, string html)
        {
            var entry = new PortfolioEntry();
            entry.Title = title;
            entry.Type = PORTFOLIO_ENTRY_TYPE.HTML;
            entry.Description = html;
            entry.TenantId = TenantId;

            //These fields are not used.
            entry.Image = "";

            Portfolio.Add(entry);
            return entry;
        }

        public void Update_Html_Page(long id, string title, string html)
        {
            var entry = this.Portfolio.Single(e => e.Id == id);
            entry.Type = PORTFOLIO_ENTRY_TYPE.HTML;
            entry.Title = title;
            entry.Description = html;
        }

        public Consultant AddConsultant(string fullName, string designation, string linkedIn, string blog, string hackernews, string stackoverflow, string github)
        {
            var consultant = new Consultant();

            consultant.Name = fullName;
            consultant.Designation = designation;
            consultant.LinkedinProfile = linkedIn;
            consultant.Picture = DEFAULT_IMAGES.PROFILE_PICTURE;
            consultant.TenantId = TenantId;

            Consultants.Add(consultant);

            if (!string.IsNullOrEmpty(blog)) consultant.AddCredential(CREDENTIAL_TYPE.BLOG, blog);
            if (!string.IsNullOrEmpty(hackernews)) consultant.AddCredential(CREDENTIAL_TYPE.HACKERNEWS, hackernews);
            if (!string.IsNullOrEmpty(stackoverflow)) consultant.AddCredential(CREDENTIAL_TYPE.STACKOVERFLOW, stackoverflow);
            if (!string.IsNullOrEmpty(github)) consultant.AddCredential(CREDENTIAL_TYPE.GITHUB, github);

            return consultant;
        }

        public void SaveLogo(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                this.Logo = FileUtil.SaveResizedImage(file, "company", 80, 80, true);
            }
        }

        public string Logo_80
        {
            get
            {
                return FileUtil.GetImage(80, this.Logo);
            }
        }

        public void DeletePortfolioEntry(long portfolioEntryId)
        {
            var portfolio = Portfolio.Single(p => p.Id == portfolioEntryId);
            portfolio.Delete(EntityContext);
        }

        public void ChangeStatus()
        {
            if (!this.IsLoaded(c => c.Account)) this.Load(this.EntityContext, c => c.Account);

            if (Account.Status == ACCOUNT_STATUS.ACTIVE) Account.Status = ACCOUNT_STATUS.DISABLED;
            else Account.Status = ACCOUNT_STATUS.ACTIVE;
        }

        public void ChangeStyle(string style)
        {
            var correctStyle = PORTFOLIO_STYLE.GetAll().Where(s => style.ToLower().Equals(s.ToLower())).SingleOrDefault();

            if (!string.IsNullOrEmpty(correctStyle))
            {
                this.Style = correctStyle;
            }
        }

        public string DisplayDescription
        {
            get
            {
                return !string.IsNullOrEmpty(Description) ? Description : "Has not set a description.";
            }
        }

        public string GetDescription(Account account)
        {
            if (!this.IsLoaded(c => c.Account)) this.Load(EntityContext, c => c.Account);

            if (string.IsNullOrEmpty(Description) && account != null && account.Type == ACCOUNT_TYPE.COMPANY
                && account.Username == this.Account.Username)
            {
                return string.Format("<a style='color:gray' href='/{0}/Edit'>Add Description</a>", this.Account.Username);
            }
            else return Description;
        }
    }
}
