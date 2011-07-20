using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using canyoucode.Core.Models;
using canyoucode.Web.ViewModels.Search;
using s = canyoucode.Web.ViewModels.Search;
using canyoucode.Core.Utils;
using Newtonsoft.Json;

using AgileFx.ORM;
using canyoucode.Core;
using canyoucode.Web.Utils;

namespace canyoucode.Web.Controllers
{
    public class SearchController : CanYouCodeControllerBase
    {
        //
        // GET: /Search/

        public ActionResult Experts()
        {
            var companies = Company.ActiveCompanies(Tenant.Id, DbContext).LoadRelated(c => c.Tags, c => c.Account)
                .OrderByDescending(c => c.Id).ToList();
            return View<Experts>(GetViewName("Experts"), v =>
                {
                    v.TopMenuSelectedItem = TopMenuSelection.SEARCH_COMPANIES;
                    v.Companies = companies;
                });
        }

        [HttpPost]
        public ActionResult Experts(string selectedTags, string country, string searchTags)
        {
            var query = Company.ActiveCompanies(Tenant.Id, DbContext).LoadRelated(c => c.Tags, c => c.Account);

            if (country != UIHelper.ALL_COUNTRIES)
            {
                query = query.Where(c => c.Country.ToLower() == country.Trim().ToLower());
            }
            if (!string.IsNullOrEmpty(selectedTags))
            {
                var tagIds = selectedTags.Split(new char[] { ',' }).Where(t => !string.IsNullOrEmpty(t)).Select(id => long.Parse(id)).ToList();
                query = query.Where(c => c.Tags.Any(t => tagIds.Contains(t.Id)));
            }

            var companies = query.OrderByDescending(c => c.Id).ToList();

            return View<Experts>(GetViewName("Experts"), v =>
            {
                v.TopMenuSelectedItem = TopMenuSelection.SEARCH_COMPANIES;
                v.Companies = companies;
                v.SelectedCountry = country;
                v.SelectedTagIds = selectedTags;
                v.SelectedTags = searchTags;
            });
        }

        public ActionResult Work()
        {
            var projects = DbContext.Project.LoadRelated(p => p.Tags, p => p.Attachments, p => p.Employer.Account)
                .Where(p => p.ClosingDate >= DateTime.Now).OrderByDescending(p => p.DateAdded);

            return View<Work>(GetViewName("Work"), v =>
                {
                    v.TopMenuSelectedItem = TopMenuSelection.SEARCH_WORK;
                    v.Projects = projects; 
                });
        }

        [HttpPost]
        public ActionResult Work(string selectedTags, string country, string searchTags)
        {
            var query = DbContext.Project.LoadRelated(p => p.Tags, p => p.Attachments, p => p.Employer, p => p.Employer.Account)
                .Where(p => p.ClosingDate >= DateTime.Now);
            
            if (country != UIHelper.ALL_COUNTRIES)
            {
                query = query.Where(p => p.Employer.Country.ToLower() == country.Trim().ToLower());
            }

            if (!string.IsNullOrEmpty(selectedTags))
            {
                var tagIds = selectedTags.Split(new char[] { ',' }).Where(t => !string.IsNullOrEmpty(t)).Select(id => long.Parse(id)).ToList();
                query = query.Where(p => p.Tags.Any(t => tagIds.Contains(t.Id)));
            }
            var projects = query.OrderByDescending(p => p.DateAdded).ToList();

            return View<Work>(GetViewName("Work"), v =>
                {
                    v.TopMenuSelectedItem = TopMenuSelection.SEARCH_WORK;
                    v.Projects = projects;
                    v.SelectedCountry = country;
                    v.SelectedTagIds = selectedTags;
                    v.SelectedTags = searchTags;
                });
        }

        public ActionResult Tags(string type, string searchkey)
        {
            if (type == TAG_TYPE.PROJECTS)
            {
                var projects = DbContext.Tag.LoadRelated(t => t.Projects)
                    .LoadRelatedInCollection(t => t.Projects, p => p.Tags, p => p.Attachments)
                    .Single(t => t.Slug == searchkey).Projects;
                return View<s.Tag>(GetViewName("ProjectTag"), v => v.Projects = projects);
            }
            else if (type == TAG_TYPE.COMPANIES)
            {
                var companies = DbContext.Tag.LoadRelated(t => t.Companies)
                    .LoadRelatedInCollection(t => t.Companies, c => c.Tags, c => c.Account)
                    .Single(t => t.Slug == searchkey).Companies;
                return View<s.Tag>(GetViewName("CompanyTag"), v => v.Companies = companies);
            }
            else throw new NotImplementedException();
        }

    }
}
