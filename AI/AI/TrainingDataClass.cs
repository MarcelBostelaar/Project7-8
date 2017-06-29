using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    class TrainingDataClass
    {

        public int amountOfPeople;
        public DayOfWeek DayOfTheWeek;
        public int timeslot;
        public bool VacationDay;
        public int Temprature;

        const int minuteintervals = 15;

        private TrainingDataClass(int amountOfPeople, DayOfWeek day, int timeslot, bool vacationday, int temprature)
        {
            this.amountOfPeople = amountOfPeople;
            this.DayOfTheWeek = day;
            this.timeslot = timeslot;
            this.VacationDay = vacationday;
            this.Temprature = temprature;
        }

        public static List<TrainingDataClass> GeneratePerTime(string[] info)
        {
            List<TrainingDataClass> list = new List<TrainingDataClass>();
            var datetimein = DateTime.Parse(info[1]);
            var datetimeout = DateTime.Parse(info[2]);
            var date = DateTime.ParseExact(info[3], "yyyy:MM:dd", CultureInfo.InvariantCulture);

            var minutesin = datetimein.Hour * 60 + datetimein.Minute;
            var minutesout = datetimeout.Hour * 60 + datetimeout.Minute;

            int timeslotin = minutesin / minuteintervals;
            int timeslotout = minutesout / minuteintervals;
            DayOfWeek dayOfWeeek = date.DayOfWeek;
            bool vacationday = false;
            int temprature = 21;

            for (int i = timeslotin; i <= timeslotout; i++)
            {
                list.Add(new TrainingDataClass(1, dayOfWeeek, i, vacationday, temprature));
            }
            return list;
        }
    }
}
