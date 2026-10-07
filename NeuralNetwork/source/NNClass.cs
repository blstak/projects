using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class TrainingResults
    {
        public List<double> TrainingLoss { get; set; }
        public List<double> TrainingAccuracy { get; set; }
        public List<double> TestingLoss { get; set; }
        public List<double> TestingAccuracy { get; set; }

        public TrainingResults()
        {
            TrainingLoss = new List<double>();
            TrainingAccuracy = new List<double>();
            TestingLoss = new List<double>();
            TestingAccuracy = new List<double>();
        }
    }

    public class NNClass
    {
        private int numLayers;
        private int[] layerSizes;
        private int[] activationFunctions;
        private double learningRate;
        private double[][][] weights;
        private double[][] biases;
        private double[][] activations;
        private double[][] zValues;
        private int lossFunction;
        public TrainingResults Results { get; private set; }

        public NNClass(int numLayers, int[] sizes, double rate, int[] activations, int loss)
        {
            this.numLayers = numLayers;
            this.layerSizes = sizes;
            this.activationFunctions = activations;
            this.learningRate = rate;
            this.lossFunction = loss;
            this.Results = new TrainingResults();
            initializeNetwork();
        }

        private void initializeNetwork()
        {
            Random rand = new Random();
            weights = new double[numLayers - 1][][];
            biases = new double[numLayers - 1][];
            activations = new double[numLayers][];
            zValues = new double[numLayers - 1][];

            for (int i = 0; i < numLayers - 1; i++)
            {
                weights[i] = new double[layerSizes[i + 1]][];
                biases[i] = new double[layerSizes[i + 1]];
                zValues[i] = new double[layerSizes[i + 1]];

                for (int j = 0; j < layerSizes[i + 1]; j++)
                {
                    weights[i][j] = new double[layerSizes[i]];
                    biases[i][j] = rand.NextDouble() * 0.2 - 0.1;
                    double scale = Math.Sqrt(2.0 / layerSizes[i]);
                    for (int k = 0; k < layerSizes[i]; k++)
                    {
                        weights[i][j][k] = (rand.NextDouble() * 2 - 1) * scale;
                    }
                }
            }

            for (int i = 0; i < numLayers; i++)
            {
                activations[i] = new double[layerSizes[i]];
            }
        }

        private double[] forward(double[] input)
        {
            Array.Copy(input, activations[0], input.Length);

            for (int layer = 0; layer < weights.Length; layer++)
            {
                for (int neuron = 0; neuron < layerSizes[layer + 1]; neuron++)
                {
                    double sum = biases[layer][neuron];
                    for (int prevNeuron = 0; prevNeuron < layerSizes[layer]; prevNeuron++)
                    {
                        sum += activations[layer][prevNeuron] * weights[layer][neuron][prevNeuron];
                    }
                    zValues[layer][neuron] = sum;
                }

                if (activationFunctions[layer] == 4)
                {
                    softmax(zValues[layer], ref activations[layer + 1]);
                }
                else
                {
                    for (int neuron = 0; neuron < layerSizes[layer + 1]; neuron++)
                    {
                        activations[layer + 1][neuron] =
                            activate(zValues[layer][neuron], activationFunctions[layer]);
                    }
                }
            }

            return activations[activations.Length - 1];
        }


        public void Train(double[][] trainingInputs, double[][] trainingOutputs, int epochs, double[][] testInputs, double[][] testOutputs, IProgress<string> progress = null)
        {
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                int[] indices = Enumerable.Range(0, trainingInputs.Length).ToArray();
                Random rng = new Random(epoch); 
                for (int i = indices.Length - 1; i > 0; i--)
                {
                    int j = rng.Next(i + 1);
                    int temp = indices[i];
                    indices[i] = indices[j];
                    indices[j] = temp;
                }

                progress?.Report($"Epoch {epoch + 1}/{epochs}");
                Console.WriteLine($"Epoch {epoch + 1}/{epochs}");
                double totalError = 0;
                int correctCount = 0;

                for (int sample = 0; sample < trainingInputs.Length; sample++)
                {
                    int idx = indices[sample];
                    double[] output = forward(trainingInputs[idx]);
                    totalError += calculateError(output, trainingOutputs[idx]);
                    if (isPredictionCorrect(output, trainingOutputs[idx]))
                    {
                        correctCount++;
                    }
                    backward(trainingOutputs[idx]);
                }

                double trainLoss = totalError / trainingInputs.Length;
                double trainAccuracy = (double)correctCount / trainingInputs.Length;
                Results.TrainingLoss.Add(trainLoss);
                Results.TrainingAccuracy.Add(trainAccuracy);

                double testLoss = 0;
                int testCorrect = 0;
                for (int sample = 0; sample < testInputs.Length; sample++)
                {
                    double[] output = forward(testInputs[sample]);
                    testLoss += calculateError(output, testOutputs[sample]);
                    if (isPredictionCorrect(output, testOutputs[sample]))
                    {
                        testCorrect++;
                    }
                }
                testLoss /= testInputs.Length;
                double testAccuracy = (double)testCorrect / testInputs.Length;
                Results.TestingLoss.Add(testLoss);
                Results.TestingAccuracy.Add(testAccuracy);
            }
        }

        private bool isPredictionCorrect(double[] output, double[] target)
        {
            int predictedClass = 0;
            int actualClass = 0;

            for (int i = 0; i < output.Length; i++)
            {
                if (output[i] > output[predictedClass])
                {
                    predictedClass = i;
                }
                if (target[i] > target[actualClass])
                {
                    actualClass = i;
                }
            }
            return predictedClass == actualClass;
        }

        private void backward(double[] target)
        {
            double[][] deltas = new double[numLayers - 1][];
            for (int i = 0; i < numLayers - 1; i++)
            {
                deltas[i] = new double[layerSizes[i + 1]];
            }

            int lastLayer = numLayers - 2;

            if (activationFunctions[lastLayer] == 4 && lossFunction == 1)
            {
                for (int neuron = 0; neuron < layerSizes[lastLayer + 1]; neuron++)
                {
                    deltas[lastLayer][neuron] = activations[lastLayer + 1][neuron] - target[neuron];
                }
            }
            else if (activationFunctions[lastLayer] == 4 && lossFunction == 0)
            {
                double[] errors = new double[layerSizes[lastLayer + 1]];
                for (int i = 0; i < layerSizes[lastLayer + 1]; i++)
                {
                    errors[i] = activations[lastLayer + 1][i] - target[i];
                }

                for (int i = 0; i < layerSizes[lastLayer + 1]; i++)
                {
                    double delta = 0;
                    for (int j = 0; j < layerSizes[lastLayer + 1]; j++)
                    {
                        double jacobian = (i == j) ?
                            activations[lastLayer + 1][i] * (1 - activations[lastLayer + 1][i]) :
                            -activations[lastLayer + 1][i] * activations[lastLayer + 1][j];
                        delta += errors[j] * jacobian;
                    }
                    deltas[lastLayer][i] = delta;
                }
            }
            else
            {
                for (int neuron = 0; neuron < layerSizes[lastLayer + 1]; neuron++)
                {
                    double error = activations[lastLayer + 1][neuron] - target[neuron];
                    deltas[lastLayer][neuron] = error * activateDerivative(zValues[lastLayer][neuron], activationFunctions[lastLayer]);
                }
            }

            for (int layer = lastLayer - 1; layer >= 0; layer--)
            {
                for (int neuron = 0; neuron < layerSizes[layer + 1]; neuron++)
                {
                    double error = 0;
                    for (int nextNeuron = 0; nextNeuron < layerSizes[layer + 2]; nextNeuron++)
                    {
                        error += deltas[layer + 1][nextNeuron] * weights[layer + 1][nextNeuron][neuron];
                    }
                    deltas[layer][neuron] = error * activateDerivative(zValues[layer][neuron], activationFunctions[layer]);
                }
            }

            for (int layer = 0; layer < numLayers - 1; layer++)
            {
                for (int neuron = 0; neuron < layerSizes[layer + 1]; neuron++)
                {
                    biases[layer][neuron] -= learningRate * deltas[layer][neuron];
                    for (int prevNeuron = 0; prevNeuron < layerSizes[layer]; prevNeuron++)
                    {
                        weights[layer][neuron][prevNeuron] -= learningRate * deltas[layer][neuron] * activations[layer][prevNeuron];
                    }
                }
            }
        }

        private double calculateError(double[] output, double[] target)
        {
            double error = 0;
            switch (lossFunction)
            {
                case 0:
                    
                    for (int i = 0; i < output.Length; i++)
                    {
                        error += Math.Pow(target[i] - output[i], 2);
                    }
                    return error / 2;
                    
                case 1:
                    for (int i = 0; i < output.Length; i++)
                    {
                        if (target[i] == 1.0)
                        {
                            error -= Math.Log(Math.Max(output[i], 1e-15));
                        }
                    }
                    return error;

                default:
                    return error;
            }
        }

        private void softmax(double[] z, ref double[] output)
        {
            double max = z.Max();
            double sum = 0.0;
            for (int i = 0; i < z.Length; i++)
            {
                output[i] = Math.Exp(z[i] - max);
                sum += output[i];
            }
            for (int i = 0; i < output.Length; i++)
            {
                output[i] /= sum;
            }
        }

        private double activate(double x, int function)
        {
            switch (function)
            {
                case 0:
                    return Math.Max(0, x);
                case 1:
                    return 1.0 / (1.0 + Math.Exp(-x));
                case 2:
                    return Math.Tanh(x);
                case 3:
                    return x;
                default:
                    return x;
            }
        }

        private double activateDerivative(double x, int function)
        {
            switch (function)
            {
                case 0:
                    return x > 0 ? 1 : 0;
                case 1:
                    double sig = 1.0 / (1.0 + Math.Exp(-x));
                    return sig * (1 - sig);
                case 2:
                    double tanh = Math.Tanh(x);
                    return 1 - tanh * tanh;
                case 3:
                    return 1;
                default:
                    return 1;
            }
        }
    }
}