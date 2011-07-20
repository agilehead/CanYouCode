using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using m=canyoucode.Core.Models;

namespace canyoucode.Core.Utils
{
    public class DataContext
    {
        public static string ConnectionString;

        public static m.Entities Get()
        {
            return new m.Entities(ConnectionString);
        }

        public static void Configure(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
