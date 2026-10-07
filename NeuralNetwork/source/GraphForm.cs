using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NeuralNetwork
{
    public partial class GraphForm : Form
    {
        private TrainingResults results;

        public GraphForm(TrainingResults results)
        {
            this.results = results;
            InitializeComponent();
            DrawChart();
        }

        private void DrawChart()
        {
            Series loss = new Series("Loss");
            loss.Color = Color.MediumVioletRed;
            loss.Legend = "Legend1";
            loss.ChartType = SeriesChartType.Spline;
            loss.ChartArea = "chartArea";
            Series accuracy = new Series("Accuracy");
            accuracy.Color = Color.CadetBlue;
            accuracy.Legend = "Legend1";
            accuracy.ChartType = SeriesChartType.Spline;
            accuracy.ChartArea = "chartArea";
            Series testLoss = new Series("Testing Loss");
            testLoss.Color = Color.OrangeRed;
            testLoss.Legend = "Legend1";
            testLoss.ChartType = SeriesChartType.Spline;
            testLoss.ChartArea = "chartArea";
            Series testAccuracy = new Series("Testing Accuracy");
            testAccuracy.Color = Color.PowderBlue;
            testAccuracy.Legend = "Legend1";
            testAccuracy.ChartType = SeriesChartType.Spline;
            testAccuracy.ChartArea = "chartArea";

            for (int i = 0; i < results.TestingAccuracy.Count; i++)
            {
                testAccuracy.Points.AddXY(i + 1, results.TestingAccuracy[i]);
                accuracy.Points.AddXY(i + 1, results.TrainingAccuracy[i]);
                testLoss.Points.AddXY(i + 1, results.TestingLoss[i]);
                loss.Points.AddXY(i + 1, results.TrainingLoss[i]);
            }

            dataChart.ChartAreas[0].Axes[1].Minimum = 0;
            dataChart.ChartAreas[0].Axes[3].Minimum = 0;
            dataChart.ChartAreas[0].Axes[1].Maximum = 1;
            dataChart.ChartAreas[0].Axes[3].Maximum = 1;
             dataChart.ChartAreas[0].Axes[0].Maximum = results.TestingAccuracy.Count();
            dataChart.ChartAreas[0].Axes[2].Maximum = results.TestingAccuracy.Count();
            dataChart.ChartAreas[0].Axes[0].Minimum = 1;
            dataChart.ChartAreas[0].Axes[2].Minimum = 1;
            dataChart.Series.Add(testLoss);
            dataChart.Series.Add(loss);
            dataChart.Series.Add(testAccuracy);
            dataChart.Series.Add(accuracy);
        }
    }
}