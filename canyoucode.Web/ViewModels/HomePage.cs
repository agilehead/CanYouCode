using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgileFx.MVC.ViewModels;
using canyoucode.Core.Models;

namespace canyoucode.Web.ViewModels
{
    public class HomePage : CanYouCodeViewModel
    {
        public List<Company> Pane1 { get; set; }
        public List<Company> Pane2 { get; set; }
    }
}