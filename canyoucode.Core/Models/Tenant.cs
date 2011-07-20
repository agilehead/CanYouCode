using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using canyoucode.Core.Utils;

namespace canyoucode.Core.Models
{
    public class TenantStore
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ViewStore { get; set; }
        public string DomainName { get; set; }
    }

    partial class Tenant
    {
        static IEnumerable<TenantStore> tenants;
        public static IEnumerable<TenantStore> GetAll()
        {
            if (tenants == null)
            {
                var entities = canyoucode.Core.Utils.DataContext.Get();
                tenants = entities.Tenant.ToList().Select(t => new TenantStore() 
                            { Id = t.Id, DomainName = t.DomainName, Name = t.Name, ViewStore = t.ViewStore });
            }

            return tenants;
        }
    }
}
