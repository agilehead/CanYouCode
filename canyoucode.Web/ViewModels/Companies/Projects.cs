using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgileFx.MVC.ViewModels;
using System.Web.Mvc;
using AgileFx.MVC.Controls;
using canyoucode.Web.Utils;
using canyoucode.Core.Models;

namespace canyoucode.Web.ViewModels.Companies
{
    public class Projects : CanYouCodeViewModel
    {
        public IEnumerable<Bid> Bids { get; set; }
        public IEnumerable<ProjectInvite> Invites { get; set; }
    }
}
