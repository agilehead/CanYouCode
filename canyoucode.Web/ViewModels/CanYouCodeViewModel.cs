using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgileFx.MVC.ViewModels;
using canyoucode.Core.Models;
using canyoucode.Web.Utils;

namespace canyoucode.Web.ViewModels
{
    public class CanYouCodeViewModel: DefaultViewModel
    {
        public TopMenuSelection TopMenuSelectedItem { get; set; }
        public Account LoggedInAccount { get; set; }
        public bool ShowFixedSidePane { get; set; }
        public string AccountAlert { get; set; }
    }
}