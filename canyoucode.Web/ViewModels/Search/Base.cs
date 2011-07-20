using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using m=canyoucode.Core.Models;
using canyoucode.Core.Utils;
using Newtonsoft.Json;

namespace canyoucode.Web.ViewModels.Search
{
    public class Base : CanYouCodeViewModel
    {
        public Base()
        {
            Tags = JsonConvert.SerializeObject(m.Tag.GetAll().Select(t => new
                {
                    Value = t.Name,
                    Key = t.Id
                }));
        }
        public string Tags { get; set; }
        public string SelectedTags { get; set; }
        public string SelectedTagIds { get; set; }
        public string SelectedCountry { get; set; }
    }
}