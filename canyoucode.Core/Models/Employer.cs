using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileFx.ORM;
using System.Web;
using System.Text.RegularExpressions;
using canyoucode.Core.Utils;

namespace canyoucode.Core.Models
{
    public partial class Employer
    {
        //This value may or may not be set.
        public int TotalProjects { get; set; }

        public static Employer Create(string name, string city, string country, string username,
            string password, string email, string phone, long tenantId, EntityContext context)
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
            account.Type = ACCOUNT_TYPE.EMPLOYER;
            account.Email = email;
            account.Phone = phone;
            account.TenantId = tenantId;

            var employer = new Employer();
            employer.Account = account;
            employer.Name = name;
            employer.City = city;
            employer.Country = country;
            employer.IsVerified = false;
            employer.TenantId = tenantId;

            context.AddObject(employer);

            try
            {
                NotificationUtil.SendAdminEmail(string.Format("Employer Signup - Username {0}", username), "");
            }
            catch { }

            return employer;
        }

        public Project CreateProject(string title, string description, int budget, string currency, DateTime closingDate, IEnumerable<long> tagIds,
            IEnumerable<string> tagNames, HttpPostedFileBase postedFile)
        {
            var project = new Project();
            project.Title = title;
            project.Description = description;
            project.Budget = budget;
            project.Currency = currency;
            project.ClosingDate = closingDate;
            project.Employer = this;
            project.Status = PROJECT_STATUS.NEW;
            project.DateAdded = DateTime.Now;
            project.DescriptionText = Regex.Replace(description, "<.*?>", string.Empty);
            project.TenantId = TenantId;
            
            if (postedFile != null && postedFile.ContentLength > 0)
                project.AddAttachment(postedFile, TenantId);

            var tags = this.DbContext().Tag.Where(t => tagIds.Contains(t.Id)).ToList();
            tags.ForEach(t => project.Tags.Add(t));

            if (tagNames != null && tagNames.Count() > 0)
            {
                var existingTagNames = tags.Select(t => t.Name);
                var newTags = tagNames.Select(n => n.Trim()).Where(tn => !string.IsNullOrEmpty(tn) && !existingTagNames.Contains(tn))
                    .Distinct().Take(10).ToList();
                if (newTags.Count() > 0) newTags.ForEach(t => project.Tags.Add(Tag.Create(t, TenantId, EntityContext)));
            }

            try
            {
                if (!this.IsLoaded(e => e.Account)) this.Load(this.EntityContext, e => e.Account);
                NotificationUtil.SendAdminEmail(string.Format("Employer Project Created - Username {0}", this.Account.Username), string.Format("project title - {0}", title));
            }
            catch { }

            return project;
        }

        public IEnumerable<Project> GetCompletedProjects()
        {
            if (!this.IsLoaded(e => e.Projects)) 
            {
                this.Load(this.EntityContext, e => e.Projects);
                this.LoadInCollection(this.EntityContext, e => e.Projects, p => p.Bids);
            }

            var projects = this.Projects.Where(p => p.Bids.Any(b => b.Status == BID_STATUS.ACCEPTED));

            foreach (var project in projects)
            {
                project.LoadInCollection(EntityContext, p => p.Bids, b => b.Company);
            }

            return projects;
        }

        public IEnumerable<Project> GetExpiredProjects()
        {
            if (!this.IsLoaded(e => e.Projects))
            {
                this.Load(this.EntityContext, e => e.Projects);
                this.LoadInCollection(this.EntityContext, e => e.Projects, p => p.Bids);
            }

            return this.Projects.Where(p => !p.Bids.Any(b => b.Status == BID_STATUS.ACCEPTED) && p.ClosingDate <= DateTime.UtcNow);
        }

        public IEnumerable<Project> GetOpenProjects()
        {
            if (!this.IsLoaded(e => e.Projects))
            {
                this.Load(this.EntityContext, e => e.Projects);
                this.LoadInCollection(this.EntityContext, e => e.Projects, p => p.Bids);
            }

            return this.Projects.Where(p => !p.Bids.Any(b => b.Status == BID_STATUS.ACCEPTED) && p.ClosingDate > DateTime.UtcNow);
        }
    }
}
