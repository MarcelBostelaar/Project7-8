using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLearning;
using System.Net;
using Newtonsoft.Json;
using DeepLearning.MathSyntax;
using System.IO;

namespace AI
{
    class Program
    {
        static List<ArgumentValue> timeSlots = CreateTimeslots();
        static ArgumentValue vacation = new ArgumentValue(0);
        static ArgumentValue rain = new ArgumentValue(0);
        static ArgumentValue temprature = new ArgumentValue(0);

        static bool Load = false;

        static List<OutputData> outputs = CreateOutputIntervals();


        [STAThread]
        static void Main(string[] args)
        {
            Dictionary<DateTime, LearningEntry> data = new Dictionary<DateTime, LearningEntry>();
            {   //Puts data into learnable form
                string json;
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString("http://145.24.222.31:8080/db/get/kantinedata");
                }
                var i = JsonConvert.DeserializeObject<List<DatabaseEntry>>(json);


                foreach (var item in i)
                {
                    foreach (var learningEntry in item.ToEntries())
                    {
                        if (!data.ContainsKey(learningEntry.exactDateSlot))
                        {
                            data.Add(learningEntry.exactDateSlot, learningEntry);
                        }
                        else
                        {
                            data[learningEntry.exactDateSlot].amountOfPeople++;
                        }
                    }
                }
            }

            List<List<LearningEntry>> DataPerInterval = new List<List<LearningEntry>>();

            for (int i = 0; i <= maxAmountOfPeople / PeopleAmountInterval; i++)
            {
                DataPerInterval.Add(new List<LearningEntry>());
            }

            foreach (var item in data.Values)
            {
                DataPerInterval[item.amountOfPeople / PeopleAmountInterval].Add(item);
            }

            int heighestIndex = 0;
            for (int i = 0; i < DataPerInterval.Count; i++)
            {
                if (DataPerInterval[heighestIndex].Count < DataPerInterval[i].Count)
                    heighestIndex++;
            }

            for (int i = 0; i < DataPerInterval.Count; i++)
            {
                if(i != heighestIndex && DataPerInterval[i].Count != 0)
                {
                    int index = 0;
                    int startCount = DataPerInterval[i].Count;
                    while(DataPerInterval[i].Count < DataPerInterval[heighestIndex].Count)
                    {
                        DataPerInterval[i].Add(DataPerInterval[i][index % startCount]);
                        index++;
                    }
                }
            }

            List<LearningEntry> allData = new List<LearningEntry>();
            foreach (var item in DataPerInterval)
            {
                allData.AddRange(item);
            }
            List<LearningEntry> randomised = new List<LearningEntry>(allData.Count);
            {
                var RNG = new Random(0);
                while (allData.Count > 0)
                {
                    var i = RNG.Next(allData.Count);
                    randomised.Add(allData[i]);
                    allData.RemoveAt(i);
                }
            }

            List<LearningEntry> learningdata, testdata;
            learningdata = new List<LearningEntry>();
            testdata = new List<LearningEntry>();

            StreamWriter results = new StreamWriter("Results.csv");

            //devide data into training and testing
            {
                var RNG = new Random(0);
                foreach (var item in randomised)
                {
                    if(RNG.Next(1, 11) > 3)
                    {
                        learningdata.Add(item);
                    }
                    else
                    {
                        testdata.Add(item);
                    }
                }
            }

            List<ArgumentValue> inputs =  new List<ArgumentValue> { vacation, rain};
            inputs.AddRange(timeSlots);

            NeuralNetwork KantineVoorspelling;
            if (!Load)
                KantineVoorspelling = new NeuralNetwork(inputs, outputs, new int[] { 30 });
            else
                KantineVoorspelling = NeuralNetwork.LoadMatrix(inputs, outputs);

            for (int times = 0; times < 1; times++)
            {
                for (int i = 0; i < learningdata.Count; i++)
                {
                    SetLearningValues(learningdata[i]);
                    KantineVoorspelling.Learn();

                    if (i % 100 == 0)
                    {
                        int right = 0;
                        int wrong = 0;
                        foreach (var item in testdata)
                        {
                            SetLearningValues(item);
                            KantineVoorspelling.CalculateResults();
                            var highest = ReturnHighestOutput();


                            if (item.amountOfPeople / PeopleAmountInterval == highest || (item.amountOfPeople >= maxAmountOfPeople && highest == outputs.Count - 1))
                                right++;
                            else
                                wrong++;
                        }
                        float percentage = ((float)right / (float)(right + wrong)) * 100f;
                        Console.WriteLine((i + times * learningdata.Count).ToString() + " ; " + percentage.ToString());
                    }
                }
            }
            results.Write("Date;Rain;Temprature;Vacation;AmountOfPeople;;");
            for (int k = 0; k <= maxAmountOfPeople / PeopleAmountInterval; k++)
            {
                results.Write(k * PeopleAmountInterval + ";");
            }
            results.Write("\n");
            foreach (var item in testdata)
            {
                SetLearningValues(item);
                KantineVoorspelling.CalculateResults();

                results.Write(item.exactDateSlot.TimeOfDay.ToString() + ";" + item.rain + ";" + item.temprature + ";" + item.vacation + ";" + item.amountOfPeople + ";;");

                var highest = ReturnHighestOutput();
                double highestValue = Math.Abs(outputs[highest].Value);

                foreach (var result in outputs)
                {
                    results.Write((int)(result.Value/highestValue * 100) + ";");
                }
                if (item.amountOfPeople / PeopleAmountInterval == highest || (item.amountOfPeople >= maxAmountOfPeople && highest == outputs.Count - 1))
                    results.Write("1");
                else
                    results.Write("0");

                results.Write("\n");
            }
            results.Close();
            KantineVoorspelling.SaveMatrix();
        }

        static void SetLearningValues(LearningEntry entry)
        {
            SetTimeslot(entry.timeslot);
            vacation.Value = entry.vacation;
            rain.Value = entry.rain;
            temprature.Value = entry.temprature;

            foreach (var output in outputs)
            {
                output.MustBeHigh = false;
            }
            var index = entry.amountOfPeople / PeopleAmountInterval;
            if (index >= outputs.Count)
            {
                index = outputs.Count - 1;
            }
            if (index < 0)
                index = 0;
            outputs[index].MustBeHigh = true;
        }


        static void SetTimeslot(int timeslot)
        {
            for (int i = 0; i < timeSlots.Count; i++)
            {
                timeSlots[i].Value = 0;
            }
            timeSlots[timeslot].Value = 1;
        }

        static List<ArgumentValue> CreateTimeslots()
        {
            List<ArgumentValue> timeslots = new List<ArgumentValue>();
            for (int i = 0; i <= (24*60) / LearningEntry.timeIntervalMinutes; i++)
            {
                timeslots.Add(new ArgumentValue(0));
            }
            return timeslots;
        }

        const int PeopleAmountInterval = 30;
        const int maxAmountOfPeople = 80;
        static List<OutputData> CreateOutputIntervals()
        {
            List<OutputData> outputs = new List<OutputData>();
            for (int i = 0; i <= maxAmountOfPeople/PeopleAmountInterval; i++)
            {
                outputs.Add(new OutputData());
            }
            return outputs;
        }

        static int ReturnHighestOutput()
        {
            int highest = 0;
            for (int i = 0; i < outputs.Count; i++)
            {
                if(outputs[i].Value > outputs[highest].Value)
                {
                    highest = i;
                }
            }
            return highest;
        }
    }
}
