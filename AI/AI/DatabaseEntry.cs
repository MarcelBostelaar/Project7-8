using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    public class LearningEntry
    {
        public LearningEntry()
        {
            amountOfPeople = 1;
        }

        public const int timeIntervalMinutes = 15;

        public int timeslot { get; set; }
        public DateTime exactDateSlot { get; set; }
        public int vacation { get; set; }
        public int rain { get; set; }
        public int temprature { get; set; }
        public int amountOfPeople { get; set; }
    }

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
        public int rain { get; set; }
        public int temperature { get; set; }
        public DateTime time_in { get; set; }
        public DateTime time_out { get; set; }
        public int vacation { get; set; }

        public List<LearningEntry> ToEntries()
        {
            List<LearningEntry> list = new List<LearningEntry>();
            int timeslotIn = (time_in.Hour * 60 + time_in.Minute)/LearningEntry.timeIntervalMinutes;
            int timeslotOut = (time_out.Hour * 60 + time_out.Minute)/LearningEntry.timeIntervalMinutes;

            for (int i = timeslotIn; i <= timeslotOut; i++)
            {
                var newentry = new LearningEntry();
                newentry.timeslot = i;
                newentry.exactDateSlot = real_date + new TimeSpan(i * LearningEntry.timeIntervalMinutes / 60, i * LearningEntry.timeIntervalMinutes % 60, 0);
                newentry.vacation = vacation;
                newentry.rain = rain;
                newentry.temprature = temperature;
                list.Add(newentry);
            }
            return list;
        }
    }
}
