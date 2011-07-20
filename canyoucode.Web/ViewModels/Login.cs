using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgileFx.MVC.ViewModels;
using System.Web.Mvc;
using AgileFx.MVC.Controls;

namespace canyoucode.Web.ViewModels
{
    public class Login : CanYouCodeViewModel
    {
        public string Username { get; set; }
    }
}