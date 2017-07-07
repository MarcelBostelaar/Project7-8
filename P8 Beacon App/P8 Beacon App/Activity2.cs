using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Android.Graphics;
using Java.Security;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;


namespace P8_Beacon_App
{
    [Activity(Label = "Activity2")]
    public class Activity2 : Activity
    {
        static GetData data = new GetData();
        static DataObject[] objects = data.getData();
        bool initbool = false;
        string ai_data;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.Window.SetTitle("Drukte in de kantine");

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.layout2);
            PlotView view = FindViewById<PlotView>(Resource.Id.plot_view);
            ai_data = Intent.GetStringExtra("ai_data");

            var adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, StringtoDateTime(objects));

            var spinner = FindViewById<Spinner>(Resource.Id.spinner1);
            spinner.Adapter = adapter;

            spinner.ItemSelected += Spinner_ItemSelected;
            var button = FindViewById<Button>(Resource.Id.button1);
            button.Click += Button_Click;

            view.Model = CreatePlotModel(objects);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            objects = data.getData();
            PlotView view = FindViewById<PlotView>(Resource.Id.plot_view);
            TextView text = FindViewById<TextView>(Resource.Id.textView2);
            text.Text = "";
            view.Model = CreatePlotModel(objects);
        }

        public DateTime[] StringtoDateTime(DataObject[] objects)
        {
            string[] dates = new string[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {

                dates[i] = objects[i].Date;
            }
            string[] newDates = dates.Distinct().ToArray();

            string[] ordered = new string[newDates.Length];

            for (int i = 0; i < ordered.Length; i++)
            {
                ordered[i] = newDates[i].Replace(":", "/");
            }
            System.Globalization.CultureInfo usGB = new System.Globalization.CultureInfo("en-US");
            var oordered = Array.ConvertAll(ordered, x => Convert.ToDateTime(x, usGB).Date).OrderByDescending(x => x).ToArray();

            return oordered;
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (initbool)
            {
                Spinner spinnert = (Spinner)sender;
                string current = spinnert.GetItemAtPosition(e.Position).ToString();
                string[] currentsplitted = current.Split(null);
                string[] newsplit = currentsplitted[0].Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                string[] neworder = new string[] { newsplit[2], newsplit[1], newsplit[0] };
                current = neworder[0] + ":" + neworder[1] + ":" + neworder[2];
                DataObject[] newObjects = objects.Where(x => x.Date == current).ToArray();
                PlotView view = FindViewById<PlotView>(Resource.Id.plot_view);

                view.Model = CreatePlotModel(newObjects);
            }
            initbool = true;


            //this.CreatePlotModel() met alleen objects van goede datum.
        }

        public PlotModel CreatePlotModel(DataObject[] objects)
        {
            TextView text = FindViewById<TextView>(Resource.Id.textView2);
            var plotModel = new PlotModel { Title = "" };
            plotModel.Axes.Add(new TimeSpanAxis
            {
                Position = AxisPosition.Bottom,
                FormatAsFractions = true,
                MinorStep = 15 * 60,
                Minimum = 10 * 3600,
                Maximum = 15 * 3600
                //IsPanEnabled = false,
                //IsZoomEnabled = false,

            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,

                //IsZoomEnabled = false,
                //IsPanEnabled = false
            });
            //new LinearAxis{Position = AxisPosition.Bottom, Minimum = 0, IsZoomEnabled = false, IsPanEnabled = false});

            var series1 = new LineSeries()
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White,
                TrackerFormatString = "{1}:{2},{3}:{4}",
                CanTrackerInterpolatePoints = true
            };



            int[,] amount =
            {{0, 11 * 3600}, {0, 11 * 3600 + 15 * 60}, {0, 11 * 3600 + 30 * 60}, {0, 11 * 3600 + 45 * 60}, {0,
                12 * 3600}, {0, 12 * 3600 + 15 * 60}, {0, 12 * 3600 + 30 * 60}, {0, 12 * 3600 + 45 * 60}, {0,
                13 * 3600 }, {0, 13 * 3600 + 15 * 60}, {0, 13 * 3600 + 30 * 60}, {0, 13 * 3600 + 45 * 60}
            };



            for (int i = 0; i < objects.Length; i++)
            {
                string[] timeinn = objects[i].Time_In.Split(':');

                switch (timeinn[0])
                {

                    case "11":

                        if (Int32.Parse(timeinn[1]) < 15)
                        {
                            amount[0, 0]++;

                        }
                        else if (Int32.Parse(timeinn[1]) < 30)
                        {
                            amount[1, 0]++;

                        }
                        else if (Int32.Parse(timeinn[1]) < 45)
                        {
                            amount[2, 0]++;

                        }
                        else
                        {
                            amount[3, 0]++;

                        }
                        break;
                    case "12":
                        if (Int32.Parse(timeinn[1]) < 15)
                        {
                            amount[4, 0]++;

                        }
                        else if (Int32.Parse(timeinn[1]) < 30)
                        {
                            amount[5, 0]++;

                        }
                        else if (Int32.Parse(timeinn[1]) < 45)
                        {
                            amount[6, 0]++;

                        }
                        else
                        {
                            amount[7, 0]++;

                        }
                        break;
                    case "13":
                        if (Int32.Parse(timeinn[1]) < 15)
                        {
                            amount[8, 0]++;

                        }
                        else if (Int32.Parse(timeinn[1]) < 30)
                        {
                            amount[9, 0]++;

                        }
                        else if (Int32.Parse(timeinn[1]) < 45)
                        {
                            amount[10, 0]++;

                        }
                        else
                        {
                            amount[11, 0]++;

                        }
                        break;
                }
            }

            for (int i = 0; i < amount.Length / 2; i++)
            {
                series1.Points.Add(new DataPoint(amount[i, 1], amount[i, 0]));
            }

            series1.TouchStarted += (s, e) =>
            {
                //x = (s as LineSeries).InverseTransform(e.Position).X;
                TrackerHitResult result = series1.GetNearestPoint(e.Position, true);
                Console.WriteLine(result.Item.ToString());
                foreach (var point in series1.Points)
                {
                    TrackerHitResult news = new TrackerHitResult();
                    news.Item = point;
                    if (news.Item.ToString() == result.Item.ToString())
                    {
                        Console.WriteLine("i did it");
                        string[] xandy = news.Item.ToString().Split();
                        TimeSpan time = TimeSpan.FromSeconds(Convert.ToSingle(xandy[0]));
                        text.Text = "There were " + xandy[1] + " people at " + time.ToString();
                    }
                }


            };



            plotModel.Series.Add(series1);
            return plotModel;
        }
    }
}