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

using System.Threading.Tasks;

namespace P8_Beacon_App
{
    public class WeatherCore
    {
        public static async Task<Weather> GetWeather(string zipCode)
        {
            //Sign up for a free API key at http://openweathermap.org/appid  
            string key = "299fc83039b9f47a13796da58a565e66"; // "YOUR KEY HERE";
            string queryString = "http://api.openweathermap.org/data/2.5/weather?zip=" + zipCode + ",nl&appid=" + key + "&units=imperial";

            var results = await DataService.getDataFromService(queryString).ConfigureAwait(true);

            if (results["weather"] != null)
            {
                Weather weather = new Weather();
                //weather.Title = (string)results["name"];
                weather.Temperature = (string)results["main"]["temp"] + " F";
                //weather.Wind = (string)results["wind"]["speed"] + " mph";
                //weather.Humidity = (string)results["main"]["humidity"] + " %";
                //weather.Visibility = (string)results["weather"][0]["main"];
                //weather.Rain = (string)results["rain"][0];

                //DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                //DateTime sunrise = time.AddSeconds((double)results["sys"]["sunrise"]);
                //DateTime sunset = time.AddSeconds((double)results["sys"]["sunset"]);
                //weather.Sunrise = sunrise.ToString() + " UTC";
                //weather.Sunset = sunset.ToString() + " UTC";
                return weather;
            }
            else
            {
                return null;
            }
        }
    }
}