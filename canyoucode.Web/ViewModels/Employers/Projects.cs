using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using canyoucode.Core.Models;

namespace canyoucode.Web.ViewModels.Employers
{
    public class Projects : CanYouCodeViewModel
    {
        public IQueryable<Project> ProjectList { get; set; }
        public string SelectedTab { get; set; }
    }
}