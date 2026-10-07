# Neural Network from Scratch (C#)

A Windows desktop application for designing, training and evaluating a feed-forward neural network. The network, the backpropagation and the training loop are all written by hand in C#, with no machine learning libraries.

## Features
- Configure the number of layers, the size of each layer and the learning rate
- Choose an activation function per layer and a loss function
- Load a dataset from a local CSV or ZIP file, or download it from a URL
- Automatic shuffled 70% / 30% train/test split
- Min-max normalisation of every input feature, fitted on the training data only
- Training for 100 epochs with live progress in the window title
- Charts of training and testing loss and accuracy per epoch

## Activation and loss functions

| Code | Activation |
|------|------------|
| 0 | ReLU |
| 1 | Sigmoid |
| 2 | Tanh |
| 3 | Linear |
| 4 | Softmax (output layer only) |

Loss functions: mean squared error and categorical cross-entropy. Softmax with cross-entropy uses the simplified output gradient (`output - target`).

Enter one activation code per connection between layers, so a network with N layers needs N-1 codes.

## Dataset format
A CSV file with one sample per line. The first column is the class label (an integer from 0 to the number of output neurons minus 1) and the remaining columns are the input features. A header row is allowed. The layout matches the common CSV version of MNIST.

## Run it
Windows and Visual Studio are required.

1. Open `NeuralNetwork.sln` in Visual Studio.
2. Press F5 to build and run.
3. Fill in the fields and click Start.

Example setup for a 784-input digit dataset:

| Field | Value |
|-------|-------|
| Layers | 3 |
| Layer sizes | 784 64 10 |
| Learning rate | 0.01 |
| Activations | 0 4 (ReLU, then softmax) |
| Loss | Cross-entropy |

## How it works
- **Storage:** weights are stored as jagged arrays (`[layer][neuron][input]`) with one bias per neuron.
- **Initialisation:** weights use He-style scaling, `sqrt(2 / inputs)`, and biases start in the range -0.1 to 0.1.
- **Forward pass:** each layer computes a weighted sum plus bias, then applies its activation. Softmax subtracts the maximum value for numerical stability.
- **Training:** stochastic gradient descent with backpropagation, updating weights after every sample. The samples are reshuffled each epoch.
- **Evaluation:** after every epoch the network is scored on the test set, and the results are plotted.

## Known limitations
- The number of epochs is fixed at 100.
- Training updates after every sample (no mini-batches), so large datasets are slow.
- Softmax is meant for the output layer only.
