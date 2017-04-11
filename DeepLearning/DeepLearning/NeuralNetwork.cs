using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathSyntax;

namespace DeepLearning
{
    class NeuralNetwork
    {
        List<InputNeuron> Input;
        List<OutputNeuron> Output;
        List<List<Neuron>> InbetweenLayers;

        List<Tuple<OutputData, SyntaxBlock, List<Tuple<VariableArgumentValue, SyntaxBlock>>>> OutputFormulas; //<output-node data, formula for result of output node, [<node-connection, partial derivative>]>

        /// <summary>
        /// Builds the equations for the network. It first calculates the formula for the result of an output node and saves it with the corresponding output node VariableArgumentValue (the output node's value).
        /// Then it calculates all the partial derivatives for that function and saves it per derived variable.
        /// </summary>
        private void BuildEquations()
        {
            foreach(var outputneuron in Output)
            {
                var resultformula = outputneuron.BuildEquation();
                resultformula = resultformula.Simplify();
                var partial_deritatives = Derivatives.CalculatePartialDerivatives(resultformula);

                OutputFormulas.Add(new Tuple<OutputData, SyntaxBlock, List<Tuple<VariableArgumentValue, SyntaxBlock>>>(outputneuron.Value, resultformula, partial_deritatives));
            }
        }

        /// <summary>
        /// Creates a new neural network with random values between -1 and 1, according to specifications.
        /// </summary>
        /// <param name="Inputs">List of ConstantArgumentValue's which are used as the input nodes.</param>
        /// <param name="Outputs">List of VariableArgumentValue's which are used as the output nodes.</param>
        /// <param name="InbetweenLayersSize">An array of integers. Each element represents one inbetween layer and its value represents the amount of nodes in that layer.</param>
        public NeuralNetwork(List<ConstantArgumentValue> Inputs, List<OutputData> Outputs, int[] InbetweenLayersSize)
        {
            Input = new List<InputNeuron>();
            Output = new List<OutputNeuron>();
            InbetweenLayers = new List<List<Neuron>>();
            OutputFormulas = new List<Tuple<OutputData, SyntaxBlock, List<Tuple<VariableArgumentValue, SyntaxBlock>>>>();

            foreach (var i in Inputs)//Fill the network with nodes
            {
                Input.Add(new InputNeuron(i));
            }
            foreach (var i in Outputs)
            {
                Output.Add(new OutputNeuron(i));
            }
            foreach(var i in InbetweenLayersSize)
            {
                var list = new List<Neuron>();
                for (int x = 0; x < i; x++)
                {
                    list.Add(new Neuron());
                }
            }
            
            foreach(var i in Input) //link the input layer to the first middle layer
            {
                foreach(var x in InbetweenLayers[0])
                {
                    i.LinkTo(x, RandomVariableValue());
                }
            }

            foreach(var i in InbetweenLayers.Last()) //link the last middle layer to the output layer
            {
                foreach(var x in Output)
                {
                    i.LinkTo(x, RandomVariableValue());
                }
            }

            if (InbetweenLayers.Count > 1) //if theres more than one inbetween layer
            {
                for (int i = 0; i < InbetweenLayers.Count - 1; i++) //for each layer except the last one
                {
                    foreach (var x in InbetweenLayers[i]) //for each node in that layer
                    {
                        foreach (var y in InbetweenLayers[i + 1]) // for each node in the next layer
                        {
                            x.LinkTo(y, RandomVariableValue()); //link them
                        }
                    }
                }
            }

            BuildEquations();
        }


        Random random = new Random();
        /// <summary>
        /// Builds a new VariableArgumentValue with a random value between -1 and 1, to be used for inbetween nodes.
        /// </summary>
        /// <returns>VariableArgumentValue with a random value between -1 and 1</returns>
        private VariableArgumentValue RandomVariableValue()
        {
            var i = new VariableArgumentValue("");
            i.Value = (random.NextDouble()-0.5)*2;
            return i;
        }

        public void CalculateResults()
        {
            foreach(var i in OutputFormulas)
            {
                i.Item1.Value = i.Item2.Calculate();
            }
        }


    }
}
