using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using canyoucode.Core.Models;

namespace canyoucode.Web.ViewModels.Search
{
    public class Tag : CanYouCodeViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Company> Companies { get; set; }
    }
}