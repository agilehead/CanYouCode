using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileFx.ORM;

namespace canyoucode.Core.Models
{
    public static class ModelExtensions
    {
        public static Entities DbContext(this EntityBase item)
        {
            return item.EntityContext as Entities;
        }
    }
}
