using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using canyoucode.Core.Models;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace canyoucode.Web.ViewModels.Projects
{
    public class EditProject : CanYouCodeViewModel
    {
        public EditProject()
        {
            Tags = JsonConvert.SerializeObject(Tag.GetAll().Select(t => new
            {
                Value = t.Name,
                Key = t.Id
            }));
        }

        public Project Project { get; set; }
        public string Tags { get; set; }
    }
}