using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using canyoucode.Core.Models;

namespace canyoucode.Web.ViewModels.Projects
{
    public class SuggestCompanies : CanYouCodeViewModel
    {
        public IEnumerable<Company> Companies { get; set; }
        public long ProjectId { get; set; }
    }
}