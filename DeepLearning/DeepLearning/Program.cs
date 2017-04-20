using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathSyntax;


namespace DeepLearning
{
    static class Program
    {
        class CharSpace
        {
            public CharSpace(int number)
            {
                for (int i = 97; i < 123; i++)
                {
                    letters.Add(new ConstantArgumentValue(number.ToString() + "." + ((char)i).ToString()));
                }
            }
            public List<ConstantArgumentValue> letters = new List<ConstantArgumentValue>();

            public void SetLetter(char letter)
            {
                foreach(var i in letters)
                {
                    i.Value = 0;
                }
                if((int)letter <97 || (int)letter > 122)
                {
                    return;
                }
                letters[(int)letter - 97].Value = 1;
            }
        }

        static void Main()
        {
            List<string> Accuracy = new List<string>();

            var EngWord = File.ReadAllLines("EN.filtered");
            var ITWord = File.ReadAllLines("IT.filtered");

            List<CharSpace> Wordspace = new List<CharSpace>();
            for (int i = 0; i < 8; i++)
            {
                Wordspace.Add(new CharSpace(i));
            }
             
            var allinputs = new List<ConstantArgumentValue>();
            foreach(var i in Wordspace)
            {
                allinputs.AddRange(i.letters);
            }

            OutputData English, Italian;
            English = new OutputData();
            Italian = new OutputData();

            var outputs = new List<OutputData>();
            outputs.Add(English);
            outputs.Add(Italian);

            NeuralNetwork LanguageNeuralNet = new NeuralNetwork(allinputs, outputs, new int[] { 20 });


            int counterEN, counterIT;
            counterEN = 0;
            counterIT = 0;
            var random = new Random();
            int Right, Wrong;
            Right = 0;
            Wrong = 0;

            {
                var accur = CalculateAccurary(Wordspace, LanguageNeuralNet, English, Italian, EngWord, ITWord);
                Accuracy.Add((counterEN + counterIT).ToString() + ";" + accur.ToString());
            }

            while (counterEN+counterIT< EngWord.Length + ITWord.Length)
            {
                string word;
                if(random.Next()%2 == 0)
                {
                    if(counterEN != EngWord.Length)
                    {
                        word = EngWord[counterEN];
                        counterEN++;
                        English.MustBeHigh = true;
                        Italian.MustBeHigh = false;
                    }
                    else
                    {
                        word = ITWord[counterIT];
                        counterIT++;
                        English.MustBeHigh = false;
                        Italian.MustBeHigh = true;
                    }
                }
                else
                {
                    if (counterIT != ITWord.Length)
                    {
                        word = ITWord[counterIT];
                        counterIT++;
                        English.MustBeHigh = false;
                        Italian.MustBeHigh = true;
                    }
                    else
                    {
                        word = EngWord[counterEN];
                        counterEN++;
                        English.MustBeHigh = true;
                        Italian.MustBeHigh = false;
                    }
                }

                for (int characternum = 0; characternum < 8; characternum++)
                {
                    Wordspace[characternum].SetLetter(word[characternum]);
                }
                LanguageNeuralNet.Learn();
                LanguageNeuralNet.CalculateResults();
                if (English.Value > Italian.Value)
                {
                    if (English.MustBeHigh)
                        Right++;
                    else
                        Wrong++;
                }
                else
                {
                    if (Italian.MustBeHigh)
                        Right++;
                    else
                        Wrong++;
                }
                if ((counterEN + counterIT) % 50 == 0)
                {
                    var accur = CalculateAccurary(Wordspace, LanguageNeuralNet, English, Italian, EngWord, ITWord);
                    Accuracy.Add((counterEN + counterIT).ToString() + ";" + accur.ToString());
                }


                //if ((counterEN + counterIT) == 7000)
                //{
                //    double percentageright = (double)Right / (double)(Right + Wrong) * 100f;
                //    int lol = 9;
                //}
            }
            
            File.WriteAllLines("Results20NodesAccurary.txt", Accuracy.ToArray());
        }

        static double CalculateAccurary(List<CharSpace> wordspace, NeuralNetwork network, OutputData english, OutputData italian, string[] englishwords, string[] italianwords)
        {
            double right = 0;
            double wrong = 0;
            for (int i = 0; i < englishwords.Length; i++)
            {
                for (int j = 0; j < wordspace.Count; j++)
                {
                    wordspace[j].SetLetter(englishwords[i][j]);
                }
                network.CalculateResults();
                if (english.Value > italian.Value)
                    right++;
                else
                    wrong++;
            }
            for (int i = 0; i < italianwords.Length; i++)
            {
                for (int j = 0; j < wordspace.Count; j++)
                {
                    wordspace[j].SetLetter(italianwords[i][j]);
                }
                network.CalculateResults();
                if (english.Value < italian.Value)
                    right++;
                else
                    wrong++;
            }

            return right / (right + wrong) * 100;
        }
    }
}
