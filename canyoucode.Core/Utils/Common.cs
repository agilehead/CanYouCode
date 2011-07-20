using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace canyoucode.Core.Utils
{
    public static class Common
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            int i = 0;
            var splits = from name in list
                         group name by i++ % parts into part
                         select part.AsEnumerable();
            return splits;
        }

        public static string FixUrl(string url)
        {
            if (!string.IsNullOrEmpty(url)) return url.StartsWith("http://") || url.StartsWith("https://") ? url : "http://" + url;
            else return url;
        }

        public static string GetFancyDate(this DateTime date)
        {
            int number = date.Day;
            string suffix = String.Empty;

            int ones = number % 10;
            int tens = (int)Math.Floor(number / 10M) % 10;

            if (tens == 1)
            {
                suffix = "th";
            }
            else
            {
                switch (ones)
                {
                    case 1:
                        suffix = "st";
                        break;

                    case 2:
                        suffix = "nd";
                        break;

                    case 3:
                        suffix = "rd";
                        break;

                    default:
                        suffix = "th";
                        break;
                }
            }
            return String.Format("{0}{1}", number, suffix) + " " + date.ToString("MMMM");
        }

        public static string RandomString(int size = 12)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString().ToLower();
        }
    }
}
