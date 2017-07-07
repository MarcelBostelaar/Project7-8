using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EstimoteSdk;
using System.Threading;
using Newtonsoft.Json;

namespace P8_Beacon_App
{
    [Activity(Label = "Activity1")]
    public class Activity1 : Activity, BeaconManager.IServiceReadyCallback
    {
        int count = 0;
        //static readonly int NOTIFICATION_ID = 123321;
        BeaconManager beaconManager;
        Region region;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.Window.SetTitle("Kantinebezoek");

            // Create your application here
            SetContentView(Resource.Layout.layout1);

            region = new Region("region1", null, null, null);
            beaconManager = new BeaconManager(this);
            KantineVisit newKantineVisit = new KantineVisit("1234");
            //ClosestBeaconTracker c_bcn_tracker = new ClosestBeaconTracker();
            HttpClient http_client = new HttpClient();


            //Button b1 = FindViewById<Button>(Resource.Id.button1);
            //b1.Text = count.ToString();
            //b1.Click += delegate
            //{
            //    count++;
            //    b1.Text = count.ToString();
            //    Refractored.Xam.Vibrate.CrossVibrate.Current.Vibration();
            //};

            //Button b2 = FindViewById<Button>(Resource.Id.button2);
            //b2.Text = "Start count";
            //bool swbegan = false;
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //b2.Click += delegate
            //{
            //    if (sw.IsRunning)
            //        sw.Stop();
            //    else
            //    {
            //        sw.Restart();
            //        swbegan = true;
            //    }
            //};

            //Button b3 = FindViewById<Button>(Resource.Id.button3);
            //bool isRanging = true;
            //b3.Click += delegate
            //{
            //    if (isRanging) {
            //        //beaconManager.StopRanging(region);
            //        beaconManager.Disconnect();
            //        isRanging = false;
            //        b3.Text = "Start Ranging";
            //    } else {
            //        //beaconManager.Connect(this);
            //        beaconManager.StartRanging(region);
            //        isRanging = true;
            //        b3.Text = "Stop Ranging";
            //    }
            //};



            TextView t8 = FindViewById<TextView>(Resource.Id.textView8);
            TextView t9 = FindViewById<TextView>(Resource.Id.textView9);
            TextView t10 = FindViewById<TextView>(Resource.Id.textView10);
            TextView t11 = FindViewById<TextView>(Resource.Id.textView11);
            TextView t12 = FindViewById<TextView>(Resource.Id.textView12);
            TextView t13 = FindViewById<TextView>(Resource.Id.textView13);
            TextView t14 = FindViewById<TextView>(Resource.Id.textView14);
            TextView t15 = FindViewById<TextView>(Resource.Id.textView15);
            TextView t16 = FindViewById<TextView>(Resource.Id.textView16);
            //TextView t17 = FindViewById<TextView>(Resource.Id.textView17);

            Button b4 = FindViewById<Button>(Resource.Id.button4);
            if (newKantineVisit.vakantiedag == "0")
                b4.Text = "vandaag is een vakantiedag: nee";
            else if (newKantineVisit.vakantiedag == "1")
                b4.Text = "vandaag is een vakantiedag: ja";
            b4.Click += delegate
            {
                if (newKantineVisit.vakantiedag == "1")
                {
                    newKantineVisit.IsVakantiedag(false);
                    b4.Text = "vandaag is een vakantiedag: nee";
                }
                else
                {
                    newKantineVisit.IsVakantiedag(true);
                    b4.Text = "vandaag is een vakantiedag: ja";
                }
                t16.Text = "verzonden: nee";
            };



            // Send data
            Button b5 = FindViewById<Button>(Resource.Id.button5);
            b5.Click += async delegate
            {
                var kv = newKantineVisit;

                newKantineVisit.timein = newKantineVisit.ConvertToRightTime(DateTime.Now.ToLocalTime());
                newKantineVisit.timeout = newKantineVisit.ConvertToRightTime(DateTime.Now.ToLocalTime());

                Dictionary<string, string> values = new Dictionary<string, string>()
                {
                    { "id", newKantineVisit.id },
                    { "timein", newKantineVisit.timein },
                    { "timeout", newKantineVisit.timeout },
                    { "date", newKantineVisit.date },
                    { "vakantiedag", newKantineVisit.vakantiedag },
                    { "temperatuur", newKantineVisit.temperatuur },
                    { "regen", newKantineVisit.regen }
                };

                t8.Text = "Waiting 1";

                var response = await http_client.PostAsync("http://145.24.222.31:8080/db/post/kantinedata",
                    new StringContent(JsonConvert.SerializeObject(values).ToString(), Encoding.UTF8, "application/json"));

                t8.Text = "Waiting 2";

                var responseString = await response.Content.ReadAsStringAsync();

                t8.Text = responseString;
                t16.Text = "verzonden: ja";
            };

            Button b6 = FindViewById<Button>(Resource.Id.button6);
            WeatherCore WeatherCore = new WeatherCore();
            b6.Click += async delegate
            {
                Weather w = await WeatherCore.GetWeather("3013");
                string lv = "Luchtvochtigheid: " + w.Humidity;
                string ff = w.Temperature.Remove(w.Temperature.Length - 5);
                int f = Int32.Parse(ff);
                double fff = (double)f;
                double celc = Celcius(fff);
                int c = (int)celc;
                string tp = "Temperatuur: " + c.ToString();
                string r = "Regen: nee";// + w.Rain;
                //t17.Text = tp + "\n" + r; //lv + "\n" + tp + "\n" + r;

                newKantineVisit.temperatuur = c.ToString();
                newKantineVisit.regen = "0";

                t14.Text = "temperatuur: " + newKantineVisit.temperatuur;
                if (newKantineVisit.regen == "1")
                    t15.Text = "regen: ja";
                else
                    t15.Text = "regen: nee";
            };

            //Button b7 = FindViewById<Button>(Resource.Id.button7);
            //b7.Click += delegate
            //{

            //};


            TextView tv2 = FindViewById<TextView>(Resource.Id.textView2);
            TextView tv3 = FindViewById<TextView>(Resource.Id.textView3);
            TextView tv4 = FindViewById<TextView>(Resource.Id.textView4);
            TextView tv5 = FindViewById<TextView>(Resource.Id.textView5);
            TextView tv6 = FindViewById<TextView>(Resource.Id.textView6);
            //TextView tv7 = FindViewById<TextView>(Resource.Id.textView7);


            string cur_bcn = null;
            string prev_bcn = null;
            int rangeCnt = 0;
            bool regenInit = false;
            beaconManager.Ranging += (sender, e) =>
            {
                //show range count
                //reset beacon texts
                //put new beacon texts
                ////beaconManager.StopRanging(region);
                ////beaconManager.StartRanging(region);

                rangeCnt++;
                tv6.Text = rangeCnt.ToString();

                //if (swbegan)
                //    b2.Text = sw.Elapsed.Seconds.ToString();

                t9.Text  = "id: " + newKantineVisit.id;
                if (!(newKantineVisit.timein == "00:00:00"))
                    t10.Text = "timein: " + newKantineVisit.timein;
                if (!(newKantineVisit.timeout == "00:00:00"))
                    t11.Text = "timeout: " + newKantineVisit.timeout;
                t12.Text = "date: " + newKantineVisit.date;
                t13.Text = "vakantiedag: " + newKantineVisit.GetVakDagString();
                //t14.Text = "temperatuur: " + newKantineVisit.temperatuur;
                //t15.Text = "regen: " + newKantineVisit.regen;


                List<TextView> tva = new List<TextView> { tv2, tv3, tv4, tv5 };
                List<string> outside_kantine_bcns = new List<string>() { "none", "56232", "17133" };
                if (!(e.Beacons.Count < 1))
                {
                    //Update TextViews
                    foreach (TextView tv in tva)
                    {
                        tv.Text = "No beacon";
                    }
                    foreach (Beacon b in e.Beacons)
                    {
                        tva[e.Beacons.IndexOf(b)].Text = b.MacAddress.ToString() + "  rssi " + b.Rssi + " power " + b.MeasuredPower;
                    }

                    //Save the right beacondata
                    cur_bcn = e.Beacons[0].Minor.ToString(); //current closest beacon
                    if (outside_kantine_bcns.Contains(prev_bcn) && !outside_kantine_bcns.Contains(cur_bcn))
                    {
                        newKantineVisit.StartVisit();
                        t16.Text = "verzonden: nee";
                    }
                    if (outside_kantine_bcns.Contains(cur_bcn) && !outside_kantine_bcns.Contains(prev_bcn) && newKantineVisit.Started)
                    {
                        newKantineVisit.timeout = newKantineVisit.ConvertToRightTime(DateTime.Now.ToLocalTime());
                        if (newKantineVisit.MoreThan7MinDif(newKantineVisit.timein, newKantineVisit.timeout))
                        {
                            //newKantineVisit.GetWeather();
                            newKantineVisit.Send();
                            //t16.Text = "verzonden: ja";
                        }
                    }
                    prev_bcn = cur_bcn;
                }
                else
                {
                    foreach (TextView tv in tva)
                    {
                        tv.Text = "No beacon";
                    }

                    cur_bcn = "none"; //current closest beacon
                    if (outside_kantine_bcns.Contains(prev_bcn) && !outside_kantine_bcns.Contains(cur_bcn))
                    {
                        newKantineVisit.StartVisit();
                        t16.Text = "verzonden: nee";
                    }
                    if (outside_kantine_bcns.Contains(cur_bcn) && !outside_kantine_bcns.Contains(prev_bcn) && newKantineVisit.Started)
                    {
                        newKantineVisit.timeout = newKantineVisit.ConvertToRightTime(DateTime.Now.ToLocalTime());
                        if (newKantineVisit.MoreThan7MinDif(newKantineVisit.timein, newKantineVisit.timeout))
                        {
                            newKantineVisit.Send();
                            t16.Text = "verzonden: ja";
                        }
                    }
                    prev_bcn = cur_bcn;
                }
            };

            //beaconManager.SetBackgroundScanPeriod(1000, 0);
            //beaconManager.EnteredRegion += (sender, e) =>
            //{
            //    tv4.Text = "UUID?: " + e.Beacons.ElementAt(0).ProximityUUID.ToString();
            //    tv2.Text = "Mj: " + e.Beacons.ElementAt(0).Major.ToString();
            //    tv3.Text = "Mn: " + e.Beacons.ElementAt(0).Minor.ToString();
            //    // Do something as the device has entered in region for the Estimote.
            //};
            //beaconManager.ExitedRegion += (sender, e) =>
            //{
            //    // Do something as the device has left the region for the Estimote.
            //    tv4.Text = "Out";
            //    tv2.Text = "Mj: " + region.Major.ToString();
            //    tv3.Text = "Mn: " + region.Minor.ToString();
            //    //b1.Text = region.ProximityUUID.ToString();
            //};
            //beaconManager.StartMonitoring(region);
            //beaconManager.StartRanging(region);
            //beaconManager.Ranging += (sender, e) =>
            //{
            //    var lolder = 2;
            //};
        }

        public void OnServiceReady()
        {
            // This method is called when BeaconManager is up and running.
            beaconManager.StartMonitoring(region);

            beaconManager.StartRanging(region);
        }

        protected override void OnStart()
        {
            base.OnStart();
            var i = 9;
        }

        protected override void OnResume()
        {
            base.OnResume();
            beaconManager.Connect(this);
            //int cnt = 0;
            //while (true)
            //{
            //    Button b1 = FindViewById<Button>(Resource.Id.button2);
            //    b1.Text = cnt++;
            //};
        }

        protected override void OnPause()
        {
            //beaconManager.StopMonitoring(region);
            beaconManager.StopRanging(region);
            base.OnPause();
        }

        protected override void OnStop()
        {
            base.OnStop();
            var i = 9;
        }

        protected override void OnDestroy()
        {
            // Make sure we disconnect from the Estimote.
            beaconManager.Disconnect();
            base.OnDestroy();
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            var i = 9;
        }

        public static double Celcius(double f)
        {
            double c = 5.0 / 9.0 * (f - 32);

            return c;
        }




    }
}