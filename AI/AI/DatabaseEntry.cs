using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    public class DatabaseEntry
    {
        public DateTime real_date { get; set; }
        public string date { get { return real_date.ToString("yyyy:MM:dd"); }
            set {
                List<string> formats = new List<string> { "yyyy:MM:dd", "yyyy:M:dd", "yyyy:MM:d", "yyyy:M:d" };
                var time = new DateTime();
                int index = 0;
                try
                {
                    while (!DateTime.TryParseExact(value, formats[index], CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                    {
                        index++;
                    }
                }
                catch
                {
                    Debug.WriteLine(value);
                }
                real_date = time;
            }
        }
        public string id { get; set; }
        public bool rain { get; set; }
        public int temperature { get; set; }
        public DateTime time_in { get; set; }
        public DateTime time_out { get; set; }
        public bool vacation { get; set; }
    }
}
