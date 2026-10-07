using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree
{
    public class DecisionTreeResults
    {
        public List<double> TrainingAccuracy { get; set; }
        public List<double> TestingAccuracy { get; set; }
        public List<double> TrainingLoss { get; set; }
        public List<double> TestingLoss { get; set; }
        public int TreeDepth { get; set; }
        public int TotalNodes { get; set; }

        public DecisionTreeResults()
        {
            TrainingAccuracy = new List<double>();
            TestingAccuracy = new List<double>();
            TrainingLoss = new List<double>();
            TestingLoss = new List<double>();
        }
    }

    internal class TreeNode
    {
        public int FeatureIndex { get; set; } = -1;
        public double Threshold { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        public bool IsLeaf { get; set; }
        public int PredictedClass { get; set; }
        public double[] ClassProbs { get; set; }
    }

    public class DecisionTreeClass
    {
        private readonly int maxDepth;
        private readonly int minSamplesSplit;
        private readonly int minSamplesLeaf;
        private readonly int numFeatures;
        private readonly int numClasses;
        private TreeNode root;

        public DecisionTreeResults Results { get; private set; }

        public DecisionTreeClass(int numClasses, int maxDepth = 0, int minSamplesSplit = 2, int minSamplesLeaf = 1, int numFeatures = 0)
        {
            this.numClasses = numClasses;
            this.maxDepth = maxDepth == 0 ? int.MaxValue : maxDepth;
            this.minSamplesSplit = minSamplesSplit;
            this.minSamplesLeaf = minSamplesLeaf;
            this.numFeatures = numFeatures;
            this.Results = new DecisionTreeResults();
        }

        public void Train(double[][] trainInputs, int[] trainLabels, double[][] testInputs, int[] testLabels, int epochs = 1, IProgress<string> progress = null)
        {
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                progress?.Report($"Epoch {epoch + 1}/{epochs}");
                Console.WriteLine($"Epoch {epoch + 1}/{epochs}");

                int[] indices = Enumerable.Range(0, trainInputs.Length).ToArray();
                root = buildTree(trainInputs, trainLabels, indices, 0);

                double trainLoss;
                double trainAcc = evaluate(trainInputs, trainLabels, out trainLoss);
                Results.TrainingAccuracy.Add(trainAcc);
                Results.TrainingLoss.Add(trainLoss);

                double testLoss;
                double testAcc = evaluate(testInputs, testLabels, out testLoss);
                Results.TestingAccuracy.Add(testAcc);
                Results.TestingLoss.Add(testLoss);

                Console.WriteLine($"  Train acc: {trainAcc:P2}  loss: {trainLoss:F4}");
                Console.WriteLine($"  Test  acc: {testAcc:P2}  loss: {testLoss:F4}");
            }

            Results.TreeDepth = treeDepth(root);
            Results.TotalNodes = countNodes(root);
        }

        public int Predict(double[] input)
        {
            if (root == null)
            {
                throw new InvalidOperationException("Tree has not been trained yet.");
            }
            return traverse(root, input).PredictedClass;
        }

        public double[] PredictProba(double[] input)
        {
            if (root == null)
            {
                throw new InvalidOperationException("Tree has not been trained yet.");
            }
            return traverse(root, input).ClassProbs;
        }

        public void PrintTree()
        {
            if (root == null)
            {
                Console.WriteLine("(empty tree)");
                return;
            }
            printNode(root, "", true);
        }

        private TreeNode buildTree(double[][] X, int[] y, int[] indices, int depth)
        {
            if (indices.Length < minSamplesSplit || depth >= maxDepth || allSameClass(y, indices))
            {
                return makeLeaf(y, indices);
            }

            int bestFeature = -1;
            double bestThreshold = 0;
            double bestGini = double.MaxValue;
            int[] bestLeft = null;
            int[] bestRight = null;

            foreach (int f in selectFeatures(X[0].Length))
            {
                double[] values = indices.Select(i => X[i][f]).Distinct().OrderBy(v => v).ToArray();

                for (int t = 0; t < values.Length - 1; t++)
                {
                    double threshold = (values[t] + values[t + 1]) / 2.0;
                    int[] left = indices.Where(i => X[i][f] <= threshold).ToArray();
                    int[] right = indices.Where(i => X[i][f] > threshold).ToArray();

                    if (left.Length < minSamplesLeaf || right.Length < minSamplesLeaf)
                    {
                        continue;
                    }

                    double giniScore = weightedGini(y, left, right, indices.Length);

                    if (giniScore < bestGini)
                    {
                        bestGini = giniScore;
                        bestFeature = f;
                        bestThreshold = threshold;
                        bestLeft = left;
                        bestRight = right;
                    }
                }
            }

            if (bestFeature == -1)
            {
                return makeLeaf(y, indices);
            }

            return new TreeNode
            {
                FeatureIndex = bestFeature,
                Threshold = bestThreshold,
                Left = buildTree(X, y, bestLeft, depth + 1),
                Right = buildTree(X, y, bestRight, depth + 1)
            };
        }

        private TreeNode makeLeaf(int[] y, int[] indices)
        {
            double[] probs = new double[numClasses];
            int[] classCounts = new int[numClasses];

            foreach (int i in indices)
            {
                classCounts[y[i]]++;
            }

            int majority = 0;

            for (int c = 0; c < numClasses; c++)
            {
                probs[c] = (double)classCounts[c] / indices.Length;

                if (classCounts[c] > classCounts[majority])
                {
                    majority = c;
                }
            }

            return new TreeNode
            {
                IsLeaf = true,
                PredictedClass = majority,
                ClassProbs = probs
            };
        }

        private TreeNode traverse(TreeNode node, double[] input)
        {
            if (node.IsLeaf)
            {
                return node;
            }

            if (input[node.FeatureIndex] <= node.Threshold)
            {
                return traverse(node.Left, input);
            }
            return traverse(node.Right, input);
        }

        private double evaluate(double[][] X, int[] y, out double loss)
        {
            int correct = 0;
            loss = 0;

            for (int i = 0; i < X.Length; i++)
            {
                TreeNode leaf = traverse(root, X[i]);

                if (leaf.PredictedClass == y[i])
                {
                    correct++;
                }

                double prob = Math.Max(leaf.ClassProbs[y[i]], 1e-15);
                loss -= Math.Log(prob);
            }

            loss /= X.Length;
            return (double)correct / X.Length;
        }

        private double gini(int[] y, int[] indices)
        {
            if (indices.Length == 0)
            {
                return 0;
            }

            int[] counts = new int[numClasses];

            foreach (int i in indices)
            {
                counts[y[i]]++;
            }

            double impurity = 1.0;

            for (int c = 0; c < numClasses; c++)
            {
                double p = (double)counts[c] / indices.Length;
                impurity -= p * p;
            }

            return impurity;
        }

        private double weightedGini(int[] y, int[] left, int[] right, int total)
        {
            return ((double)left.Length / total) * gini(y, left) + ((double)right.Length / total) * gini(y, right);
        }

        private bool allSameClass(int[] y, int[] indices)
        {
            int first = y[indices[0]];
            return indices.All(i => y[i] == first);
        }

        private int[] selectFeatures(int total)
        {
            int k = numFeatures == 0 ? total : numFeatures == -1 ? (int)Math.Sqrt(total) : Math.Min(numFeatures, total);

            if (k >= total)
            {
                return Enumerable.Range(0, total).ToArray();
            }

            Random rng = new Random();
            return Enumerable.Range(0, total).OrderBy(_ => rng.Next()).Take(k).ToArray();
        }

        private int treeDepth(TreeNode node)
        {
            if (node == null || node.IsLeaf)
            {
                return 0;
            }
            return 1 + Math.Max(treeDepth(node.Left), treeDepth(node.Right));
        }

        private int countNodes(TreeNode node)
        {
            if (node == null)
            {
                return 0;
            }
            return 1 + countNodes(node.Left) + countNodes(node.Right);
        }

        private void printNode(TreeNode node, string indent, bool last)
        {
            string branch = last ? "> " : "- ";
            string child = last ? "  " : "| ";

            if (node.IsLeaf)
            {
                Console.WriteLine($"{indent}{branch}[LEAF] class={node.PredictedClass}  probs=[{string.Join(", ", node.ClassProbs.Select(p => p.ToString("F2")))}]");
                return;
            }

            Console.WriteLine($"{indent}{branch}Feature[{node.FeatureIndex}] <= {node.Threshold:F4}");
            printNode(node.Left, indent + child, false);
            printNode(node.Right, indent + child, true);
        }
    }
}