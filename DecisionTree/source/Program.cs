using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rng = new Random(42);
            int total = 400;

            double[][] inputs = new double[total][];
            int[] labels = new int[total];
            for (int i = 0; i < total; i++)
            {
                double x = rng.NextDouble();
                double y = rng.NextDouble();
                inputs[i] = new double[] { x, y };
                labels[i] = (x > 0.5 ? 1 : 0) + (y > 0.5 ? 2 : 0);
            }

            int trainSize = (int)(total * 0.8);
            double[][] trainX = inputs.Take(trainSize).ToArray();
            int[] trainY = labels.Take(trainSize).ToArray();
            double[][] testX = inputs.Skip(trainSize).ToArray();
            int[] testY = labels.Skip(trainSize).ToArray();

            DecisionTreeClass tree = new DecisionTreeClass(4);

            tree.Train(trainX, trainY, testX, testY, epochs: 1);

            Console.WriteLine($"\nTree depth: {tree.Results.TreeDepth}");
            Console.WriteLine($"Total nodes: {tree.Results.TotalNodes}");
            Console.WriteLine($"Train acc: {tree.Results.TrainingAccuracy.Last():P2}");
            Console.WriteLine($"Test  acc: {tree.Results.TestingAccuracy.Last():P2}");

            Console.WriteLine("\nTree Structure:");
            tree.PrintTree();

            double[] sample = new double[] { 0.75, 0.25 };
            int predicted = tree.Predict(sample);
            double[] probs = tree.PredictProba(sample);
            Console.WriteLine($"\nSample [{sample[0]}, {sample[1]}]");
            Console.WriteLine($"Predicted class: {predicted}  (expected 1)");
            Console.WriteLine($"Class probs: [{string.Join(", ", probs.Select(p => p.ToString("F2")))}]");

            Console.ReadKey();
        }
    }
}