using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgileFx.MVC.ViewModels;
using System.Web.Mvc;
using canyoucode.Core.Models;
using canyoucode.Web.Utils;

namespace canyoucode.Web.ViewModels.Companies
{
    public class Edit : CanYouCodeViewModel
    {        
        public string TagList { get; set; }
        public Company Company { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Currency { get; set; }
        public IEnumerable<SelectListItem> MinimumRates { get; set; }
    }
}