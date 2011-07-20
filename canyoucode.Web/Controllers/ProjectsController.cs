using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using canyoucode.Core;
using canyoucode.Core.Models;
using u = canyoucode.Core.Utils;
using canyoucode.Web.ViewModels;

using AgileFx;
using AgileFx.ORM;
using canyoucode.Web.ViewModels.Projects;
using canyoucode.Web.Utils;
using canyoucode.Web.ViewModels.Employers;

namespace canyoucode.Web.Controllers
{
    public class ProjectsController : CanYouCodeControllerBase
    {
        public ActionResult ViewItem(long id)
        {
            var project = DbContext.Project
                .LoadRelated(p => p.Bids, p => p.Attachments, p => p.Employer.Account, p => p.Tags)
                .LoadRelatedInCollection(p => p.Bids, b => b.Company.Account, b => b.Company.Tags)
                .Single(p => p.Id == id);

            project.Employer.TotalProjects = DbContext.Project.Where(p => p.Employer == project.Employer).Count();

            if (LoggedInAccount == null)
                return View<ProjectView>(GetViewName("PublicView"), v =>
                {
                    v.Project = project;
                });

            switch (LoggedInAccount.Type)
            {
                case ACCOUNT_TYPE.COMPANY:
                    return View<ProjectView>(GetViewName("CompanyView"), v =>
                        {
                            v.Project = project;
                            v.MessageCode = LoggedInAccount.Status == ACCOUNT_STATUS.DISABLED ? MessageCodes.ACTIVATE_ACCOUNT_TO_BID : string.Empty;
                        });
                case ACCOUNT_TYPE.EMPLOYER:
                    return View<ProjectView>(GetViewName("EmployerView"), v =>
                    {
                        v.Project = project;
                    });
                default:
                    throw new InvalidOperationException();
            }
        }

        [Authorize(Roles = ACCOUNT_TYPE.EMPLOYER)]
        public ActionResult Edit(long id)
        {
            var project = DbContext.Project.LoadRelated(p => p.Attachments).Single(p => p.Id == id);
            return View<EditProject>(GetViewName("EditProject"), v =>
            {
                v.Project = project;
            });
        }

        [Authorize(Roles = ACCOUNT_TYPE.EMPLOYER)]
        [HttpPost]
        public ActionResult Edit(long id, FormCollection coll)
        {
            var project = DbContext.Project.LoadRelated(p => p.Attachments).Single(p => p.Id == id);

            var employer = DbContext.Employer.Single(e => e.AccountId == LoggedInAccount.Id);
            var tagids = coll["TagIds"].Split(new char[] { ',' }).Select(x => Int64.Parse(x));

            if (project.EmployerId == employer.Id)
            {
                var postedFile = Request.Files[0] as HttpPostedFileBase;
                project.Update(coll["Name"], coll["Description"], Convert.ToInt32(coll["Budget"]),
                            CURRENCY.USD, DateTime.Parse(coll["ClosingDate"]), tagids, postedFile);

                return Redirect("/");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        [Authorize(Roles = ACCOUNT_TYPE.EMPLOYER)]
        [HttpPost]
        public ActionResult SendInvites(long id, FormCollection coll)
        {
            var project = DbContext.Project.LoadRelated(p => p.Attachments).Single(p => p.Id == id);

            var employer = DbContext.Employer.Single(e => e.AccountId == LoggedInAccount.Id);

            if (project.EmployerId == employer.Id)
            {
                var companyIds = coll["CompanyIds"].GetListFromCSV<long>();
                companyIds.ForEach(cId => project.SendInvite(cId));

                DbContext.SaveChanges();
                return Redirect(string.Format("/{0}/Projects?Message={1}", LoggedInAccount.Username, MessageCodes.INVITES_SENT));
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public ActionResult SuggestCompanies(long Id)
        {
            //? need to show the tags of the company that are relevant to the project.
            var companies = Project.SuggestCompanies(Id, 10);
            return View<SuggestCompanies>(GetViewName("SuggestCompanies"), v =>
            {
                v.Companies = companies;
                v.ProjectId = Id;
            });

        }
    }
}
