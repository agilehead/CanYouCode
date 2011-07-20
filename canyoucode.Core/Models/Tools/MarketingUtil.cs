using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AgileFx.ORM;

namespace canyoucode.Core.Models.Tools
{
    public static class MarketingUtil
    {
        public static MarketingCampaign CreateMarketingCampaign(string companyName, string website, string city, string country,
            string username, string email, string phone, HttpFileCollectionBase postedFiles, string description,
            HttpPostedFileBase logoFile, string referringURL, Action<EntityContext, Company> createPortfolios)
        {
            var context = new Entities();

            if (Account.Exists(username))
                throw new Exception("The username already exists.");

            var account = new Account();
            account.Username = username;
            account.Password = AgileFx.Security.CryptoUtil.HashPassword(Guid.NewGuid().ToString());
            account.Status = ACCOUNT_STATUS.ACTIVE;
            account.LastLoginDate = DateTime.Now;
            account.DateAdded = DateTime.Now;
            account.Type = ACCOUNT_TYPE.COMPANY;
            account.Email = email;

            var company = new Company();
            company.Account = account;
            company.Name = companyName;
            company.Website = website;
            company.City = city;
            company.Country = country;
            company.Phone = phone;

            if (logoFile.ContentLength > 0)
                company.SaveLogo(logoFile);

            company.Description = description;

            foreach (var tag in context.Tag.Where(t => t.Name == "Web Design" || t.Name == ""))
                company.Tags.Add(tag);

            createPortfolios(context, company);

            context.AddObject(company);
            context.SaveChanges();

            var marketingCamp = new MarketingCampaign();
            marketingCamp.Account = account.Id;
            marketingCamp.DateCreated = DateTime.UtcNow;
            marketingCamp.DateModified = DateTime.UtcNow;
            marketingCamp.ReferringURL = referringURL;
            marketingCamp.Status = MARKETING_CAMPAIGN_STATUS.NEW;
            marketingCamp.Token = Guid.NewGuid();

            context.AddObject(marketingCamp);
            context.SaveChanges();

            return marketingCamp;
        }
    }
}