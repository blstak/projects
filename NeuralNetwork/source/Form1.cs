using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.IO.Compression;

namespace NeuralNetwork
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        string urlData = "";
        string pathData = "";
        private void useLocal_CheckedChanged(object sender, EventArgs e)
        {
            if (useLocal.Checked)
            {
                urlData = dataPath.Text;
                dataPath.Text = pathData;
            }
            else
            {
                pathData = dataPath.Text;
                dataPath.Text = urlData;
            }

        }

        private int getSelectedRadioTag()
        {
            RadioButton radio = groupBox1.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked);
            return Convert.ToInt16(radio.Tag);
        }

        private void dataPath_Click(object sender, EventArgs e)
        {
            if (useLocal.Checked)
            {
                openFileDialog1.Filter = "Dataset files (*.csv;*.zip)|*.csv;*.zip|CSV files (*.csv)|*.csv|ZIP files (*.zip)|*.zip|All files (*.*)|*.*";
                openFileDialog1.Title = "Select Dataset File";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    dataPath.Text = openFileDialog1.FileName;
                }
            }
        }

        private async void startBtn_Click(object sender, EventArgs e)
        {

            /*
            TrainingResults tr = new TrainingResults();
            foreach (string item in layersN.Text.Split(' '))
            {
                tr.TrainingAccuracy.Add(Convert.ToDouble(item));
            }
            foreach (string item in layerSizes.Text.Split(' '))
            {
                tr.TrainingLoss.Add(Convert.ToDouble(item));
            }
            foreach (string item in learningRate.Text.Split(' '))
            {
                tr.TestingAccuracy.Add(Convert.ToDouble(item));
            }
            foreach (string item in layerSizes.Text.Split(' '))
            {
                tr.TestingLoss.Add(Convert.ToDouble(item));
            }
            GraphForm graphForm = new GraphForm(tr);
            graphForm.Show();
            return;
            
            */
            if (string.IsNullOrWhiteSpace(layersN.Text) || string.IsNullOrWhiteSpace(layerSizes.Text) || string.IsNullOrWhiteSpace(learningRate.Text))
            {
                MessageBox.Show("Fill all textboxes with required data", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(dataPath.Text))
            {
                MessageBox.Show("You need to input path or url to the dataset", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int numLayers;
            if (!int.TryParse(layersN.Text, out numLayers) || numLayers <= 0)
            {
                MessageBox.Show("Number of layers must be a whole number", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double rate;
            string rateText = learningRate.Text.Replace('.', ',');
            if (!double.TryParse(rateText, out rate) || rate <= 0)
            {
                MessageBox.Show("Learning rate must be a positive number", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] layersSize = layerSizes.Text.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (layersSize.Length == 0)
            {
                MessageBox.Show("You need to fill layer sizes", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (layersSize.Length != numLayers)
            {
                MessageBox.Show($"Number of layer sizes ({layersSize.Length}) doesnt match the number of layers ({numLayers}).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int[] sizes = new int[numLayers];
            for (int i = 0; i < numLayers; i++)
            {
                if (!int.TryParse(layersSize[i], out sizes[i]) || sizes[i] <= 0)
                {
                    MessageBox.Show($"Layer size {layersSize[i]} is invalid, you have to enter integer", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string[] activationFs = activationFunctions.Text.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (activationFs.Length != numLayers-1)
            {
                MessageBox.Show($"Number of activation functions ({activationFs.Length}) must be equal to {numLayers-1}", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int[] activations = new int[numLayers-1];
            for (int i = 0; i < numLayers - 1; i++)
            {
                if (!int.TryParse(activationFs[i], out activations[i]) || activations[i] < 0 || activations[i] > 4)
                {
                    MessageBox.Show($"Activation function '{activationFs[i]}' is invalid it has be 0, 1, 2, or 3", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            double[][] trainInputs, trainOutputs, testInputs, testOutputs;

            if (useLocal.Checked)
            {
                if (!LoadAndSplitDataset(dataPath.Text, sizes[numLayers - 1], out trainInputs, out trainOutputs, out testInputs, out testOutputs))
                {
                    MessageBox.Show("Loading dataset failed", "Dataset Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (!DownloadAndLoadDataset(dataPath.Text, sizes[numLayers - 1], out trainInputs, out trainOutputs, out testInputs, out testOutputs))
                {
                    MessageBox.Show("Loading dataset failed", "Dataset Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            NormalizeData(ref trainInputs, ref testInputs);
            try
            {
                int lossFunction = getSelectedRadioTag();
                if (lossFunction == -1)
                {
                    MessageBox.Show("You need to select a loss function", "Inavlid Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                NNClass network = new NNClass(numLayers, sizes, rate, activations, lossFunction);
                startBtn.Enabled = false;

                var progress = new Progress<string>(status =>
                {
                    this.Text = $"Neural Network - {status}";
                });

                await Task.Run(() =>
                {
                    network.Train(trainInputs, trainOutputs, 100, testInputs, testOutputs, progress);
                });

                this.Text = "Neural Network";
                GraphForm graphForm = new GraphForm(network.Results);
                graphForm.Show();
                startBtn.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating or training neural network: {ex.Message}", "Training Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                startBtn.Enabled = true;
            }
        }

        private bool LoadAndSplitDataset(string filePath, int numOutputs, out double[][] trainInputs, out double[][] trainOutputs, out double[][] testInputs, out double[][] testOutputs)
        {
            trainInputs = null;
            trainOutputs = null;
            testInputs = null;
            testOutputs = null;
            string csvPath = filePath;
            string extractedFolder = null;

            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Dataset not found", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (Path.GetExtension(filePath).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    extractedFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    Directory.CreateDirectory(extractedFolder);

                    try
                    {
                        ZipFile.ExtractToDirectory(filePath, extractedFolder);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error extracting ZIP: {ex.Message}", "Extraction Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (Directory.Exists(extractedFolder))
                        {
                            try
                            {
                                Directory.Delete(extractedFolder, true);
                            }
                            catch { }
                        }
                        return false;
                    }

                    string[] csvFiles = Directory.GetFiles(extractedFolder, "*.csv", SearchOption.AllDirectories);
                    if (csvFiles.Length == 0)
                    {
                        MessageBox.Show("No CSV file found in the ZIP", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (Directory.Exists(extractedFolder))
                        {
                            try
                            {
                                Directory.Delete(extractedFolder, true);
                            }
                            catch { }
                        }
                        return false;
                    }
                    csvPath = csvFiles[0];
                }

                string[] lines = File.ReadAllLines(csvPath);
                if (lines.Length == 0)
                {
                    MessageBox.Show("Dataset file is empty", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                int startIndex = lines[0].Contains(",") && !char.IsDigit(lines[0][0]) ? 1 : 0;
                int dataCount = lines.Length - startIndex;

                if (dataCount == 0)
                {
                    MessageBox.Show("No data found in the dataset", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                int trainSize = (int)(dataCount * 0.7);
                int testSize = dataCount - trainSize;

                if (trainSize == 0 || testSize == 0)
                {
                    MessageBox.Show("Dataset is too small", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                Random rng = new Random();
                string[] data = new string[dataCount];
                Array.Copy(lines, startIndex, data, 0, dataCount);

                for (int i = data.Length - 1; i > 0; i--)
                {
                    int j = rng.Next(i + 1);
                    string temp = data[i];
                    data[i] = data[j];
                    data[j] = temp;
                }

                trainInputs = new double[trainSize][];
                trainOutputs = new double[trainSize][];
                testInputs = new double[testSize][];
                testOutputs = new double[testSize][];

                for (int i = 0; i < trainSize; i++)
                {
                    if (!ParseLine(data[i], numOutputs, out trainInputs[i], out trainOutputs[i]))
                    {
                        MessageBox.Show($"Error parsing line {i + 1} ({data[i]})", "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                for (int i = 0; i < testSize; i++)
                {
                    if (!ParseLine(data[trainSize + i], numOutputs, out testInputs[i], out testOutputs[i]))
                    {
                        MessageBox.Show($"Error parsing line {trainSize + i + 1} ({data[trainSize + i + 1]})", "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dataset: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (extractedFolder != null && Directory.Exists(extractedFolder))
                {
                    try
                    {
                        Directory.Delete(extractedFolder, true);
                    }
                    catch { }
                }
            }
        }

        private bool ParseLine(string line, int numOutputs, out double[] inputs, out double[] outputs)
        {
            inputs = null;
            outputs = null;

            try
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    return false;
                }

                string[] values = line.Split(',');
                if (values.Length < 2)
                {
                    return false;
                }

                int label;
                if (!int.TryParse(values[0].Trim(), out label) || label < 0 || label >= numOutputs)
                {
                    return false;
                }

                inputs = new double[values.Length - 1];
                for (int i = 0; i < values.Length - 1; i++)
                {
                    if (!double.TryParse(values[i + 1], out inputs[i]))
                    {
                        return false;
                    }
                }

                inputs = new double[values.Length - 1];
                for (int i = 0; i < values.Length - 1; i++)
                {
                    if (!double.TryParse(values[i], out inputs[i]))
                    {
                        return false;
                    }
                }

                outputs = new double[numOutputs];
                outputs[label] = 1.0;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void NormalizeData(ref double[][] trainInputs, ref double[][] testInputs)
        {
            int numFeatures = trainInputs[0].Length;

            for (int feature = 0; feature < numFeatures; feature++)
            {
                double min = trainInputs[0][feature];
                double max = trainInputs[0][feature];

                for (int i = 0; i < trainInputs.Length; i++)
                {
                    if (trainInputs[i][feature] < min) 
                        min = trainInputs[i][feature];
                    if (trainInputs[i][feature] > max) 
                        max = trainInputs[i][feature];
                }

                double range = max - min;
                if (range == 0)
                { 
                    range = 1;
                }

                for (int i = 0; i < trainInputs.Length; i++)
                {
                    trainInputs[i][feature] = (trainInputs[i][feature] - min) / range;
                }

                for (int i = 0; i < testInputs.Length; i++)
                {
                    testInputs[i][feature] = (testInputs[i][feature] - min) / range;
                }
            }
        }

        private bool DownloadAndLoadDataset(string url, int numOutputs, out double[][] trainInputs, out double[][] trainOutputs, out double[][] testInputs, out double[][] testOutputs)
        {
            trainInputs = null;
            trainOutputs = null;
            testInputs = null;
            testOutputs = null;
            string tempFile = null;
            string extractedFolder = null;

            try
            {
                string extension = Path.GetExtension(url).ToLower();
                if (string.IsNullOrEmpty(extension))
                {
                    extension = ".csv";
                }

                tempFile = Path.GetTempFileName();
                string tempFileWithExt = Path.ChangeExtension(tempFile, extension);
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
                tempFile = tempFileWithExt;

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, tempFile);
                }

                string csvPath = tempFile;

                if (extension == ".zip")
                {
                    extractedFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    Directory.CreateDirectory(extractedFolder);

                    try
                    {
                        using (ZipArchive archive = ZipFile.OpenRead(tempFile))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (entry.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                                {
                                    string destinationPath = Path.Combine(extractedFolder, entry.Name);
                                    entry.ExtractToFile(destinationPath, true);
                                    csvPath = destinationPath;
                                    break;
                                }
                            }
                        }

                        if (csvPath == tempFile)
                        {
                            MessageBox.Show("No CSV file found in ZIP file", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error extracting ZIP file: {ex.Message}", "Extraction Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                bool success = LoadAndSplitDataset(csvPath, numOutputs, out trainInputs, out trainOutputs, out testInputs, out testOutputs);
                return success;
            }
            catch (WebException)
            {
                MessageBox.Show("Failed to download dataset", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading dataset: {ex.Message}", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (tempFile != null && File.Exists(tempFile))
                {
                    try
                    {
                        File.Delete(tempFile);
                    }
                    catch { }
                }
                if (extractedFolder != null && Directory.Exists(extractedFolder))
                {
                    try
                    {
                        Directory.Delete(extractedFolder, true);
                    }
                    catch { }
                }
            }
        }
    }
}