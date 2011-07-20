using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using canyoucode.Core.Models;

namespace canyoucode.Core.Utils
{
    public class Location
    {
        static Location()
        {
            GetCities();
        }

        public static IEnumerable<City> cities;
        public static IEnumerable<City>  GetCities()
        {
            if (cities == null)
            {
                var context = DataContext.Get();
                var empCities = context.Employer.ToList().Select(e => new City { Country = e.Country, Name = e.City }).Distinct();
                var compCities = context.Company.ToList().Select(c => new City { Country = c.Country, Name = c.City }).Distinct();
                cities = empCities.Union(compCities).Distinct();
            }

            return cities;
        }

    }

    public struct City
    {
        public string Name { get; set; }
        public string Country {get;set;}
    }
}
