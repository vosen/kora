namespace UAM.Kora.Forms
{
    partial class ChartForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.Chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.ProgressBarPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).BeginInit();
            this.ProgressBarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Chart
            // 
            chartArea2.AxisX.Title = "Ilość elementów";
            chartArea2.AxisY.Title = "Czas (ms)";
            chartArea2.Name = "Default";
            this.Chart.ChartAreas.Add(chartArea2);
            this.Chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.Chart.Legends.Add(legend2);
            this.Chart.Location = new System.Drawing.Point(0, 0);
            this.Chart.Name = "Chart";
            this.Chart.Size = new System.Drawing.Size(784, 562);
            this.Chart.TabIndex = 0;
            this.Chart.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.progressBar1.Location = new System.Drawing.Point(3, 16);
            this.progressBar1.MarqueeAnimationSpeed = 50;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(500, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(500, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Przetwarzanie danych ...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ProgressBarPanel
            // 
            this.ProgressBarPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ProgressBarPanel.AutoSize = true;
            this.ProgressBarPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ProgressBarPanel.Controls.Add(this.label1);
            this.ProgressBarPanel.Controls.Add(this.progressBar1);
            this.ProgressBarPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ProgressBarPanel.Location = new System.Drawing.Point(139, 260);
            this.ProgressBarPanel.Name = "ProgressBarPanel";
            this.ProgressBarPanel.Size = new System.Drawing.Size(506, 42);
            this.ProgressBarPanel.TabIndex = 3;
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.ProgressBarPanel);
            this.Controls.Add(this.Chart);
            this.Name = "ChartForm";
            this.Text = "Wyniki";
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).EndInit();
            this.ProgressBarPanel.ResumeLayout(false);
            this.ProgressBarPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Chart;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel ProgressBarPanel;
    }
}