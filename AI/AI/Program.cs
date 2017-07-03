using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLearning;
using System.Net;
using Newtonsoft.Json;

namespace AI
{
    class Program
    {
        static void Main(string[] args)
        {
            string json;
            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString("http://145.24.222.31:8080/db/get/kantinedata");
            }
            var i = JsonConvert.DeserializeObject<List<DatabaseEntry>>(json);

            /*
            List<TrainingDataClass> all = new List<TrainingDataClass>();

            foreach (var item in i)
            {
                all.AddRange(TrainingDataClass.GeneratePerTime(item));
            }*/
        }
    }
}
