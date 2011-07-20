using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AgileFx.ORM;
using canyoucode.Core.Utils;

namespace canyoucode.Core.Models
{
    public partial class Project
    {
        //This may or may not be populated.
        public int TotalBids { get; set; }
        public Bid WinningBid { get; set; }


        public void Update(string title, string description, int budget, string currency, DateTime closingDate, IEnumerable<long> tagIds,
            HttpPostedFileBase postedFile)
        {
            this.Title = title;
            this.Description = description;
            this.Budget = budget;
            this.Currency = currency;
            this.ClosingDate = closingDate;

            this.Tags.Clear();
            var tags = this.DbContext().Tag.Where(t => tagIds.Contains(t.Id)).ToList();
            tags.ForEach(t => this.Tags.Add(t));

            //? for now we only support one attachment
            this.Attachments.Clear();
            if (postedFile != null && postedFile.ContentLength > 0)
                this.AddAttachment(postedFile, TenantId);
        }

        public ProjectInvite SendInvite(long companyId)
        {
            var invite = new ProjectInvite();
            var company = this.DbContext().Company.LoadRelated(c => c.Account).Single(c => c.Id == companyId);

            invite.ProjectId = this.Id;
            invite.CompanyId = companyId;
            invite.Status = PROJECT_INVITE_STATUS.NEW;

            if (!this.IsLoaded(p => p.Attachments)) this.Load(this.EntityContext, p => p.Attachments);

            string subject = string.Format("You have been invited to give a quote for Project {0}", this.Title);
            NotificationUtil.SendSystemEmailWithTemplate(company.Account.Email, subject, EMAIL_TEMPLATES.PROJECT_INVITE_TEMPLATE,
                company.Name, this.Title, this.Id.ToString(), this.Description,
                this.Attachments.Count > 0 ? this.Attachments.Single().Token.ToString() : null,
                this.Budget.ToString());

            this.DbContext().AddObject(invite);
            return invite;
        }

        public void Invite(Company company)
        {
            var invite = new ProjectInvite();
            invite.Project = this;
            invite.Company = company;
            invite.Status = PROJECT_INVITE_STATUS.NEW;
        }

        public string GetBudget()
        {
            return Utils.I18NUtil.GetCurrencyChar(this.Currency) + this.Budget;
        }

        public string GetShortDescription(int length = 300)
        {
            if (this.DescriptionText.Length > length)
                return this.DescriptionText.Substring(0, length) + "...";
            else
                return this.DescriptionText;
        }

        public Bid GetCompanyActiveBid(long companyAccountId)
        {
            if (!this.IsLoaded(p => p.Bids))
            {
                this.Load(EntityContext, p => p.Bids);
                this.LoadInCollection(EntityContext, p => p.Bids, b => b.Company);
            }
            return this.Bids.SingleOrDefault(b => b.Company.AccountId == companyAccountId && b.Status == BID_STATUS.NEW);
        }

        public static IEnumerable<Company> SuggestCompanies(long projectId, int minCount)
        {
            var context = DataContext.Get();
            var project = context.Project.Single(p => p.Id == projectId);

            var tags = context.Tag.Where(t => t.TenantId == project.TenantId).ToList();

            var matchingTags = new List<Tag>();
            tags.ForEach(t =>
            {
                if (project.DescriptionText.ToLowerInvariant().Contains(t.Name.ToLowerInvariant()))
                {
                    matchingTags.Add(t);
                    if (!t.IsLoaded(tag => tag.Companies))
                    {
                        t.Load(context, tg => tg.Companies);
                        t.LoadInCollection(context, tg => tg.Companies, c => c.Tags, c => c.Account);
                    }
                }
            });

            var tagMatchingCompanies = matchingTags.SelectMany(t => t.Companies)
                .Where(c => c.Account.Status == ACCOUNT_STATUS.ACTIVE && c.TenantId == project.TenantId).Distinct().Take(minCount).ToList();

            var companies = tagMatchingCompanies;
            if (tagMatchingCompanies.Count() < minCount) companies
                    .AddRange(Company.ActiveCompanies(project.TenantId, context).LoadRelated(c => c.Tags, c => c.Account)
                    .Where(c => !tagMatchingCompanies.Select(tc => tc.Id).Contains(c.Id))
                    .Take(minCount - tagMatchingCompanies.Count()));

            return companies;
        }

        public Bid GetAwarededBid()
        {
            if (!this.IsLoaded(p => p.Bids))
            {
                this.Load(this.EntityContext, p => p.Bids);
                this.LoadInCollection(this.EntityContext, p => p.Bids, b => b.Company);
            }

            return (this.Bids.SingleOrDefault(b => b.Status == BID_STATUS.ACCEPTED));
        }

        public void AddAttachment(HttpPostedFileBase file, long tenantId)
        {
            var uploadFolder = "project";

            var attachment = new Attachment();
            attachment.Url = FileUtil.SaveFile(file, uploadFolder);
            attachment.DateAdded = DateTime.Now;
            attachment.OriginalFileName = file.FileName;
            attachment.Token = Guid.NewGuid();
            attachment.TenantId = tenantId;

            this.Attachments.Add(attachment);
        }
    }
}