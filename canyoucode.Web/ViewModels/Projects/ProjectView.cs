using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using canyoucode.Core.Models;

namespace canyoucode.Web.ViewModels.Projects
{
    public class ProjectView : CanYouCodeViewModel
    {
        public Project Project { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}