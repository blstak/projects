# Decision Tree Classifier (C#)

A decision tree classifier written from scratch in C#, with no external libraries. It is a console application that trains a tree on a small generated dataset, reports accuracy, and prints the learned tree as text.

## Features
- Binary splits on numeric features, chosen by Gini impurity
- Exhaustive threshold search: candidate splits are the midpoints between neighbouring distinct feature values
- Configurable stopping rules: maximum depth, minimum samples to split, minimum samples per leaf
- Optional random feature subsets at each split (all features, square root of the total, or a fixed number)
- Class probabilities from every leaf, not just the predicted class
- Accuracy and log loss on both training and test data
- Text printout of the whole tree, plus its depth and node count

## What the demo does
`Program.cs` generates 400 random points in the unit square. Each point gets one of four classes based on its quadrant: the class is 1 if x > 0.5, plus 2 if y > 0.5. It uses an 80/20 train/test split, trains a tree, and then classifies the point (0.75, 0.25), which should be class 1.

Because the classes are defined by two simple thresholds, a correct tree should be shallow and score close to 100%.

## Example output
```
[Paste your real console output here: depth, node count, accuracies and the printed tree]
```

## Using the classifier
```csharp
var tree = new DecisionTreeClass(numClasses: 4, maxDepth: 5, minSamplesSplit: 2, minSamplesLeaf: 1);
tree.Train(trainX, trainY, testX, testY);

int predicted = tree.Predict(new double[] { 0.75, 0.25 });
double[] probabilities = tree.PredictProba(new double[] { 0.75, 0.25 });
tree.PrintTree();
```

| Parameter | Meaning |
|-----------|---------|
| `numClasses` | Number of classes. Labels must be integers from 0 to `numClasses - 1` |
| `maxDepth` | Maximum tree depth, 0 for unlimited |
| `minSamplesSplit` | Minimum samples a node needs to be split (default 2) |
| `minSamplesLeaf` | Minimum samples allowed in a leaf (default 1) |
| `numFeatures` | 0 uses all features, -1 uses the square root of the feature count, n uses n random features per split |

## How it works
1. At each node, every feature and every candidate threshold is tried.
2. For each candidate, the samples are divided into a left and right group, and the weighted Gini impurity of the two groups is computed.
3. The split with the lowest impurity is chosen, and the process repeats on each group.
4. A node becomes a leaf when it is pure, too small, or at the depth limit. The leaf stores the majority class and the class probabilities.
5. Predicting a sample means following the thresholds from the root to a leaf.

## Limitations
- Numeric features only
- No pruning
- The split search is simple and not optimised, so it is slow on large datasets
- `epochs` exists for symmetry with the neural network project, but the tree is deterministic with all features, so extra epochs just rebuild the same tree
