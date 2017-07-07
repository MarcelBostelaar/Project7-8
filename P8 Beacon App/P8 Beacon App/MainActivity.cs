using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;

namespace P8_Beacon_App
{
    [Activity(Label = "P8_Beacon_App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        //MachineLearning AI;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //AI = new MachineLearning();
            //TimePicker timepicker = FindViewById<TimePicker>(Resource.Id.timePicker1);

            Button myB = FindViewById<Button>(Resource.Id.MyButton);
            //myB.Text = count.ToString();
            myB.Click += delegate
            {
                //count++;
                //myB.Text = count.ToString();
                StartActivity(new Intent(BaseContext, typeof(Activity1)));
            };

            Button b1 = FindViewById<Button>(Resource.Id.button1);
            b1.Click += async delegate
            {
                //AI.tijd = (60*12).ToString();
                //AI.regent = "0";
                //AI.vakantiedag = "0";

                //List<string> aidata = await AI.GetData();

                //Intent intent = new Intent(BaseContext, typeof(Activity2));
                //intent.PutExtra("ai_data", aidata);

                StartActivity(new Intent(BaseContext, typeof(Activity2)));
            };

            //Button b2 = FindViewById<Button>(Resource.Id.button2);
            //b1.Click += delegate
            //{
            //    AI.vakantiedagSwitch();
            //    if (AI.vakantiedag == "0")
            //        b2.Text = "vakantiedag: nee";
            //    else
            //        b2.Text = "vakantiedag: ja";
            //};

            //Button b3 = FindViewById<Button>(Resource.Id.button3);
            //b1.Click += delegate
            //{
            //    AI.regentSwitch();
            //    if (AI.regent == "0")
            //        b2.Text = "regen: nee";
            //    else
            //        b2.Text = "regen: ja";
            //};

            //timepicker.TimeChanged += Timepicker_TimeChanged;
        }

        //private void Timepicker_TimeChanged(object sender, TimePicker.TimeChangedEventArgs e)
        //{
        //    int h = e.HourOfDay;
        //    int m = e.Minute;
        //    string tijd = (m + (h * 60)).ToString(); //dit posten we

        //    AI.tijd = tijd;
        //}

        protected override void OnStart()
        {
            base.OnStart();
            var i = 9;
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
            var i = 9;
        }

        protected override void OnStop()
        {
            base.OnStop();
            var i = 9;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            var i = 9;
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            var i = 9;
        }
    }


}

