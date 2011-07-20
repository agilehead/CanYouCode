using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AgileFx.ORM;
using AgileFx.MVC.Utils;
using canyoucode.Core;
using canyoucode.Web.ViewModels;
using canyoucode.Web.ViewModels.Employers;
using canyoucode.Core.Models;
using Microsoft.Security.Application;
using canyoucode.Web.Utils;
using canyoucode.Web.ViewModels.Projects;

namespace canyoucode.Web.Controllers
{
    public class EmployersController : CanYouCodeControllerBase
    {
        public ActionResult Signup()
        {
            return View<Edit>(GetViewName("Signup"));
        }

        [HttpPost]
        public ActionResult Signup(string name, string city, string country, string username, string password, string email, string phone)
        {
            var employer = Employer.Create(name, city, country, username,
                password, email, phone, Tenant.Id, DbContext);

            DbContext.SaveChanges();

            Response.Cookies.Add(AuthHelper.GetAuthTicketWithRoles(employer.Account.Username, employer.Account.Type, true, new TimeSpan(0, 30, 0)));
            return Redirect(string.Format("/{0}/AddProject", employer.Account.Username));
        }

        [Authorize(Roles = ACCOUNT_TYPE.EMPLOYER)]
        public ActionResult Projects(string id, string view)
        {
            var projectStatus = !string.IsNullOrEmpty(view) && view.ToLowerInvariant() == "closed" ?
                PROJECT_STATUS.CLOSED : PROJECT_STATUS.NEW;

            var employer = DbContext.Employer.Single(e => e.AccountId == LoggedInAccount.Id);

            var projects = DbContext.Project.LoadRelated(p => p.Bids, p => p.Attachments)
                        .LoadRelatedInCollection(p => p.Bids, b => b.Company.Account, b => b.Company.Tags)
                        .Where(p => p.EmployerId == employer.Id && p.Status == projectStatus);

            return View<Projects>(GetViewName("Projects"), v =>
                {
                    v.TopMenuSelectedItem = Utils.TopMenuSelection.EMPLOYERS_PROJECTS;
                    v.ProjectList = projects;
                    v.SelectedTab = projectStatus;
                });
        }

        [Authorize(Roles = ACCOUNT_TYPE.EMPLOYER)]
        [CheckReferrer]
        public ActionResult AcceptBid(long BidId)
        {
            var employer = DbContext.Employer.Single(e => e.AccountId == LoggedInAccount.Id);

            var bid = DbContext.Bid.LoadRelated(b => b.Project.Employer.Account).Single(b => b.Id == BidId);
            if (bid.Project.EmployerId == employer.Id)
            {
                bid.Accept();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }

            DbContext.SaveChanges();

            return Redirect(string.Format("/Projects/{0}", bid.ProjectId));
        }

        [Authorize(Roles=ACCOUNT_TYPE.EMPLOYER)]
        public ActionResult Edit(string id)
        {
            var employer = DbContext.Employer.LoadRelated(e => e.Account).Single(e => e.AccountId == LoggedInAccount.Id);
            return View<Edit>(GetViewName("Edit"), v =>
                {
                    v.TopMenuSelectedItem = Utils.TopMenuSelection.EMPLOYERS_PROFILE_EDIT;
                    v.Employer = employer;
                });
        }

        [HttpPost]
        [Authorize(Roles = ACCOUNT_TYPE.EMPLOYER)]
        public ActionResult Edit(FormCollection coll)
        {
            var employer = DbContext.Employer.LoadRelated(e => e.Account).Single(e => e.AccountId == LoggedInAccount.Id);
            UpdateModel(employer, new[] { "Name", "City", "Country"});
            employer.Account.Phone = coll["Phone"];
            employer.Account.Email = coll["Email"];

            DbContext.SaveChanges();

            return RedirectToAction("Edit", new { Message = MessageCodes.PROFILE_UPDATED });
        }

        public ActionResult ViewItem(string username)
        {
            var employer = DbContext.Employer.LoadRelated(e => e.Projects, e => e.Account).Single(e => e.Account.Username == username);
            return View<ViewItem>(GetViewName("ViewItem"), v => 
                {
                    v.TopMenuSelectedItem = Utils.TopMenuSelection.EMPLOYERS_PROFILE_VIEW;
                    v.Employer = employer;
                });
        }

        [Authorize(Roles = ACCOUNT_TYPE.EMPLOYER)]
        public ActionResult AddProject()
        {
            return View<EditProject>("~/Views/" + Tenant.ViewStore + "/Projects/EditProject.aspx", v =>
            {
                v.Project = new Project();
            });
        }

        [Authorize(Roles = ACCOUNT_TYPE.EMPLOYER)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddProject(FormCollection coll)
        {
            var employer = DbContext.Employer.Single(e => e.AccountId == LoggedInAccount.Id);
            var postedFile = Request.Files[0] as HttpPostedFileBase;
            var tagids = coll["SelectedTags"].Split(new char[] { ',' }).Where(x => !string.IsNullOrEmpty(x)).Select(x => Int64.Parse(x)).ToList();

            var description = AntiXss.GetSafeHtmlFragment(coll["Description"]);
            var searchTags = coll["SearchTags"];
            
            //TODO: XSRF Checks! We cannot allow img src="", link href="" etc.

            //for now just add 2 weeks as closing date.
            var project = employer.CreateProject(AntiXss.GetSafeHtmlFragment(coll["Name"]), description, Convert.ToInt32(coll["Budget"]),
                            CURRENCY.USD, DateTime.Parse(coll["ClosingDate"]), tagids,searchTags.Split(','), postedFile);
            DbContext.SaveChanges();

            return Redirect(string.Format("/Projects/{0}/SuggestCompanies", project.Id));
        }
    }
}
