using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace canyoucode.Web.ViewModels
{
    public class ResetPassword : CanYouCodeViewModel
    {
        public bool IsValidToken { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}