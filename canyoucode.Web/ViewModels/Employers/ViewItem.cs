using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using canyoucode.Core.Models;

namespace canyoucode.Web.ViewModels.Employers
{
    public class ViewItem : CanYouCodeViewModel
    {
        public Employer Employer { get; set; }
    }
}