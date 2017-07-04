using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLearning;
using System.Net;
using Newtonsoft.Json;
using DeepLearning.MathSyntax;

namespace AI
{
    class Program
    {
        static List<ArgumentValue> timeSlots = CreateTimeslots();
        static ArgumentValue vacation = new ArgumentValue(0);
        static ArgumentValue rain = new ArgumentValue(0);
        static ArgumentValue temprature = new ArgumentValue(0);

        static List<OutputData> outputs = CreateOutputIntervals();


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

            List<LearningEntry> learningdata, testdata;
            learningdata = new List<LearningEntry>();
            testdata = new List<LearningEntry>();

            //devide data into training and testing
            {
                var RNG = new Random();
                foreach (var item in data.Values)
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

            List<ArgumentValue> inputs =  new List<ArgumentValue> { vacation, rain, temprature };
            inputs.AddRange(timeSlots);
            
            NeuralNetwork KantineVoorspelling = new NeuralNetwork(inputs, outputs, new int[] { 30});

            for (int times = 0; times < 10; times++)
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

        const int PeopleAmountInterval = 20;
        const int maxAmountOfPeople = 100;
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
