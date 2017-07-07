using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Org.Apache.Http.Client.Params;
//using System.Json;
using Java.Lang;
using Newtonsoft.Json;
using Org.Apache.Http.Impl.IO;

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
    public class DataObject
    {
        // time/date are strings, so it's easier to save from the results of the api call.
        public string Date { get; set; }
        public string Id { get; set; }
        public int Rain { get; set; }
        public int temperature { get; set; }
        public string Time_In { get; set; }
        public string Time_Out { get; set; }
        public int Vacation { get; set; }
    }

    public class GetData
    {
        // base URL
        private const string URL = "http://145.24.222.31:8080/";
        // only one api call now, so the paramater of the url is hard coded.
        private string urlParam = "db/get/kantinedatalast1000";


        public DataObject[] getData()
        {
            var request = HttpWebRequest.Create(URL + urlParam);
            request.ContentType = "application/json";
            request.Method = "GET";
            DataObject[] initData = new DataObject[] { };

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        //gets the content from the http request.

                        var content = reader.ReadToEnd();
                        string half = content.Substring(Convert.ToInt32(content.Length * 0.1 + 4));
                        // counter to save index numbers
                        int countIndex = 0;
                        // index where Open bracket is
                        int open = 0;
                        // index where close bracket is
                        int close = 0;

                        string[] contents = new string[] { "initstring" };

                        //this foreach takes each individual json object out of the string acquired from the httprequest, and puts them as strings in an array
                        foreach (var character in content)
                        {
                            if (character == '{')
                            {
                                open = countIndex;
                            }
                            if (character == '}')
                            {
                                close = countIndex;
                                var sb2 = new Java.Lang.StringBuilder(close - open);

                                for (int i = open; i < close + 1; i++)
                                {
                                    sb2.Append(content[i]);
                                }
                                string newstring = sb2.ToString();
                                string[] tempContents = new string[contents.Length + 1];

                                for (int i = 0; i < contents.Length; i++)
                                {
                                    tempContents[i] = contents[i];
                                }
                                tempContents[tempContents.Length - 1] = newstring;
                                contents = tempContents;


                                //not sure what i thought here... //contents[contents.Length -1] = newstring + "}";
                            }
                            countIndex++;
                        }
                        contents = contents.Where(val => val != "initstring").ToArray();
                        //for each string in the array created above, deserialize the sting to an DataObject
                        DataObject[] objects = new DataObject[contents.Length];
                        for (int i = 0; i < objects.Length; i++)
                        {
                            objects[i] = JsonConvert.DeserializeObject<DataObject>(contents[i]);
                        }
                        initData = objects;

                    }
                }


            }
            return initData;
        }
    }

    //public class MachineLearning
    //{
    //    public string tijd;
    //    public string vakantiedag;
    //    public string regent;
    //    HttpClient http_client;


    //    public MachineLearning()
    //    {
    //        tijd = "0";
    //        vakantiedag = "0";
    //        regent = "0";
    //        http_client = new HttpClient();
    //   }

    //    //public void VulIn(string tijd, string vakantiedag, string regent)
    //    //{
    //    //    this.tijd = tijd;
    //    //    this.vakantiedag = vakantiedag;
    //    //    this.regent = regent;
    //    //}
    //    public void vakantiedagSwitch()
    //    {
    //        if (vakantiedag == "0")
    //            vakantiedag = "1";
    //        else
    //            vakantiedag = "0";
    //    }
    //    public void regentSwitch()
    //    {
    //        if (regent == "0")
    //            regent = "1";
    //        else
    //            regent = "0";
    //    }

    //    public async Task<List<string>> GetData()
    //    {
    //        if (tijd != "0")
    //        {
    //            List<string> s = new List<string>();
    //            for (int t = Int32.Parse(tijd); t <= (14*60) ; t+=(15))
    //            {
    //                Dictionary<string, string> values = new Dictionary<string, string>()
    //                {
    //                    { "tijd", tijd },
    //                    { "vakantiedag", vakantiedag },
    //                    { "regent", regent }
    //                };

    //                var response = await http_client.PostAsync("http://145.24.222.31:8080/getprediction",
    //                    new StringContent(JsonConvert.SerializeObject(values).ToString(), Encoding.UTF8, "application/json"));

    //                s.Add(await response.Content.ReadAsStringAsync());
    //            }


    //            //var responseString = await response.Content.ReadAsStringAsync();

    //            return s;
    //        }
    //        else
    //        {
    //            return new List<string>();
    //        }
    //    }

    //}
}



