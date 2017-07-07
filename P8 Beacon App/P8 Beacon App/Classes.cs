using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace P8_Beacon_App
{
    //class ClosestBeaconTracker
    //{
    //    public string LastBcnOver15;
    //    public bool NewLastBcnOver15;
    //    List<string> bcnList; //to keep track of beaconindexes for stopwatch list (know which stopwatch is which beacon)
    //    List<Stopwatch> swList;
    //    public ClosestBeaconTracker()
    //    {
    //        LastBcnOver15 = null;
    //        NewLastBcnOver15 = false;
    //        bcnList = new List<string>() { null };
    //        swList = new List<Stopwatch>() { new Stopwatch() };
    //    }
    //    public void Update(string cur_closest_bcn)
    //    {
    //        if (LastBcnOver15 != cur_closest_bcn)
    //        {
    //            // check if cur_closest_bcn is the new LastBcnOver15, and more
    //            if (!bcnList.Contains(cur_closest_bcn))
    //            {
    //                bcnList.Add(cur_closest_bcn);
    //                Stopwatch cur_sw = new Stopwatch();
    //                swList.Add(cur_sw);
    //                cur_sw.Start();
    //            }
    //            else
    //            {
    //                if (swList[bcnList.IndexOf(cur_closest_bcn)].Elapsed.Minutes > 15)
    //                {
    //                    LastBcnOver15 = cur_closest_bcn;
    //                    NewLastBcnOver15 = true;
    //                }
    //            }

    //            int index = 0;
    //            foreach (Stopwatch sw in swList)
    //            {
    //                //
    //                //if (sw.Elapsed.Minutes >)
    //                //{

    //                //}
    //                index++;
    //            }

    //            Stopwatch sw_cur_closest = swList[bcnList.IndexOf(cur_closest_bcn)];
    //            if (!sw_cur_closest.IsRunning)
    //                sw_cur_closest.Restart();

                
    //        }
    //        if (true)
    //        {
    //            NewLastBcnOver15 = true;
    //        }
    //    }
    //}

    class KantineVisit
    {
        //145.24.222.31:8080/db/post/kantinedata Post example:  {"id":"2", "timein":"19:20:36",
        //"timeout":"19:40:42", "date":"2017:06:24", "vakantiedag":"0", "temperatuur":"21",
        //"regen":"0" }

        public string id;
        public string timein;
        public string timeout;
        public string date;
        public string vakantiedag;
        public string temperatuur;
        public string regen;
        public bool Started;
        public bool regenInit;
        public KantineVisit(string phone_id)
        {
            id = phone_id;
            timein = "00:00:00";
            timeout = "00:00:00";
            date = GetDateToday();
            vakantiedag = "0";
            temperatuur = "20";
            regen = "0";
            regenInit = false;

            Started = false;
        }

        public void StartVisit()
        {
            string today = GetDateToday();
            if (date != today)
            {
                date = today;
                vakantiedag = GetVakantiedag(date);
            }

            timein = ConvertToRightTime(DateTime.Now.ToLocalTime());
            timeout = "00:00:00";

            Started = true;
        }
        private string GetVakantiedag(string date)
        {
            return "";
        }
        private string GetDateToday()
        {
            DateTime now = DateTime.Now.ToLocalTime(); // "04/07/2017 13:54:34"
            return ConvertDateToRightFormat(now); //"jjjj:mm:dd"
        }
        public string ConvertDateToRightFormat(DateTime date)
        {
            string yr = date.Year.ToString();
            string month;
            if (date.Month < 10)
                month = "0" + date.Month.ToString();
            else
                month = date.Month.ToString();
            string day;
            if (date.Day < 10)
                day = "0" + date.Day.ToString();
            else
                day = date.Day.ToString();

            return yr + ":" + month + ":" + day;
        }
        public string ConvertToRightTime(DateTime dt)
        {
            string hr = dt.Hour.ToString();
            if (Int32.Parse(hr) < 10)
                hr = "0" + hr;
            string min = dt.Minute.ToString();
            if (Int32.Parse(min) < 10)
                min = "0" + min;
            string sec = dt.Second.ToString();
            if (Int32.Parse(sec) < 10)
                sec = "0" + sec;

            string time = hr + ":" + min + ":" + sec;
            return time;
        }
        public bool MoreThan7MinDif(string timein, string timeout)
        {
            if (timein != null && timeout != null){
                string[] splitIn = timein.Split(Char.Parse(":"));
                int minIn = Int32.Parse(splitIn[1]);

                string[] splitOut = timeout.Split(Char.Parse(":"));
                int minOut = Int32.Parse(splitOut[1]);

                int lowerBound = minIn + 7;
                if (lowerBound > 60)
                    lowerBound -= 60;
                if (lowerBound > minOut)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        public void Send()
        {

        }
        public void IsVakantiedag(bool bl)
        {
            if (bl)
                vakantiedag = "1";
            else
                vakantiedag = "0";
        }
        public string GetVakDagString()
        {
            if (vakantiedag == "0")
                return "nee";
            else if (vakantiedag == "1")
                return "ja";
            else
                return "ongeldig";
        }
        //private void loltest()
        //{
        //    KantineVisit kv = new KantineVisit("");
        //    kv.
        //}
        public async void GetWeather()
        {
            Weather w = await WeatherCore.GetWeather("3013");
            string ff = w.Temperature.Remove(w.Temperature.Length - 5);
            int f = Int32.Parse(ff);
            double fff = (double)f;
            double celc = Activity1.Celcius(fff);
            int c = (int)celc;

            regen = "0";
            regenInit = true;
            temperatuur = c.ToString();
        }
    }
    
}