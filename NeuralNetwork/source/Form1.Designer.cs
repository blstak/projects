
namespace NeuralNetwork
{
    partial class StartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.layersN = new System.Windows.Forms.TextBox();
            this.layerSizes = new System.Windows.Forms.TextBox();
            this.learningRate = new System.Windows.Forms.TextBox();
            this.useLocal = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dataPath = new System.Windows.Forms.TextBox();
            this.startBtn = new System.Windows.Forms.Button();
            this.activationFunctions = new System.Windows.Forms.TextBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.layerSizesToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.activationFunctionsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CatCrossEntr = new System.Windows.Forms.RadioButton();
            this.MeanSquared = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // layersN
            // 
            this.layersN.Location = new System.Drawing.Point(12, 12);
            this.layersN.Name = "layersN";
            this.layersN.Size = new System.Drawing.Size(112, 22);
            this.layersN.TabIndex = 0;
            this.layersN.Text = "number of layers";
            this.layersN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // layerSizes
            // 
            this.layerSizes.Location = new System.Drawing.Point(12, 54);
            this.layerSizes.Name = "layerSizes";
            this.layerSizes.Size = new System.Drawing.Size(112, 22);
            this.layerSizes.TabIndex = 1;
            this.layerSizes.Text = "layer sizes";
            this.layerSizes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // learningRate
            // 
            this.learningRate.Location = new System.Drawing.Point(12, 91);
            this.learningRate.Name = "learningRate";
            this.learningRate.Size = new System.Drawing.Size(112, 22);
            this.learningRate.TabIndex = 2;
            this.learningRate.Text = "learning rate";
            this.learningRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // useLocal
            // 
            this.useLocal.AutoSize = true;
            this.useLocal.Location = new System.Drawing.Point(12, 139);
            this.useLocal.Name = "useLocal";
            this.useLocal.Size = new System.Drawing.Size(137, 21);
            this.useLocal.TabIndex = 3;
            this.useLocal.Text = "use local dataset";
            this.useLocal.UseVisualStyleBackColor = true;
            this.useLocal.CheckedChanged += new System.EventHandler(this.useLocal_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dataPath
            // 
            this.dataPath.Location = new System.Drawing.Point(12, 166);
            this.dataPath.Name = "dataPath";
            this.dataPath.Size = new System.Drawing.Size(669, 22);
            this.dataPath.TabIndex = 4;
            this.dataPath.Click += new System.EventHandler(this.dataPath_Click);
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(579, 138);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(101, 22);
            this.startBtn.TabIndex = 6;
            this.startBtn.Text = "START";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // activationFunctions
            // 
            this.activationFunctions.Location = new System.Drawing.Point(144, 12);
            this.activationFunctions.Name = "activationFunctions";
            this.activationFunctions.Size = new System.Drawing.Size(165, 22);
            this.activationFunctions.TabIndex = 5;
            this.activationFunctions.Text = "layers activation functions";
            this.activationFunctions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(185, 54);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(79, 85);
            this.infoLabel.TabIndex = 7;
            this.infoLabel.Text = "0 - ReLU\r\n1 - Sigmoid\r\n2 - Tanh\r\n3 - Linear\r\n4 - SoftMax";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CatCrossEntr);
            this.groupBox1.Controls.Add(this.MeanSquared);
            this.groupBox1.Location = new System.Drawing.Point(323, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 132);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Loss Functions";
            // 
            // CatCrossEntr
            // 
            this.CatCrossEntr.AutoSize = true;
            this.CatCrossEntr.Location = new System.Drawing.Point(4, 60);
            this.CatCrossEntr.Name = "CatCrossEntr";
            this.CatCrossEntr.Size = new System.Drawing.Size(193, 21);
            this.CatCrossEntr.TabIndex = 12;
            this.CatCrossEntr.TabStop = true;
            this.CatCrossEntr.Tag = "1";
            this.CatCrossEntr.Text = "Categorical Cross Entropy";
            this.CatCrossEntr.UseVisualStyleBackColor = true;
            // 
            // MeanSquared
            // 
            this.MeanSquared.AutoSize = true;
            this.MeanSquared.Checked = true;
            this.MeanSquared.Location = new System.Drawing.Point(4, 33);
            this.MeanSquared.Name = "MeanSquared";
            this.MeanSquared.Size = new System.Drawing.Size(122, 21);
            this.MeanSquared.TabIndex = 11;
            this.MeanSquared.TabStop = true;
            this.MeanSquared.Tag = "0";
            this.MeanSquared.Text = "Mean Squared";
            this.MeanSquared.UseVisualStyleBackColor = true;
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 200);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.activationFunctions);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.dataPath);
            this.Controls.Add(this.useLocal);
            this.Controls.Add(this.learningRate);
            this.Controls.Add(this.layerSizes);
            this.Controls.Add(this.layersN);
            this.Name = "StartForm";
            this.Text = "Neural Network Config";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox layersN;
        private System.Windows.Forms.TextBox layerSizes;
        private System.Windows.Forms.TextBox learningRate;
        private System.Windows.Forms.CheckBox useLocal;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox dataPath;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.TextBox activationFunctions;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.ToolTip layerSizesToolTip;
        private System.Windows.Forms.ToolTip activationFunctionsToolTip;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton CatCrossEntr;
        private System.Windows.Forms.RadioButton MeanSquared;
    }
}

