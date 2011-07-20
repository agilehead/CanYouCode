using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using canyoucode.Core.Utils;

namespace canyoucode.Core.Models
{
    public partial class Token
    {
        public static Token Create(string type, string data, long tenantId)
        {
            var token = new Token();
            token.Key = Common.RandomString(12);
            token.Type = type;
            token.Data = data;
            token.CreatedDate = DateTime.Now;
            token.TenantId = tenantId;
            return token;
        }

        public void UpdateKey()
        {
            Key = Common.RandomString(12);
        }
    }
}
