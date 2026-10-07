# Neural Network from Scratch (C#)

A Windows desktop application for designing, training and evaluating a feed-forward neural network, written in C# without any external machine learning libraries.

## What it does
- Configure the network: number of layers, size of each layer, learning rate
- Choose an activation function for each layer and a loss function
- Load a dataset from a local CSV or ZIP file, or download it from a URL
- Split the data automatically into 70% training and 30% testing, with shuffling
- Normalise every input feature to the 0-1 range, using the training data
- Train for 100 epochs while progress is shown in the window title
- Plot training and testing accuracy and loss when training finishes

## Dataset format
CSV with one sample per line. The first column is the class label (an integer from 0 to the number of outputs minus 1), and the remaining columns are the input features. A header row is allowed. This is the same layout as the MNIST dataset in CSV form.

## Run it
Windows and Visual Studio are required.

1. Open `NeuralNetwork.sln` [check the exact file name] in Visual Studio.
2. Press F5 to build and run.
3. Enter the layer count, sizes, learning rate, activation functions and loss function, choose a dataset, and click Start.

For example, a network for [your dataset] could use: [number of layers, e.g. 3], sizes [e.g. 784 64 10], learning rate [e.g. 0.1], activations [codes].

## Activation and loss functions
[List what the numeric codes mean, e.g. 0 = sigmoid, 1 = ReLU ...]
[List the loss functions offered by the radio buttons.]

## Results
[Screenshot of the accuracy and loss graph.]
Tested on [dataset]: [accuracy] test accuracy with [the settings you used].

## How it works
[2-3 sentences from NNClass: how the weights are stored, how the forward pass works, how training updates the weights (for example backpropagation with gradient descent).]

## Notes
- The number of epochs is currently fixed at 100.
- Everything, including the matrix operations and training, is implemented by hand, with no ML libraries.
