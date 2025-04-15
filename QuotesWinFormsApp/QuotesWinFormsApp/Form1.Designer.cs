namespace QuotesWinFormsApp
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
      System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
      System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
      System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
      this.btnAddDataGraph = new System.Windows.Forms.Button();
      this.chartGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.radioBtn_Bid = new System.Windows.Forms.RadioButton();
      this.radioBtn_Ask = new System.Windows.Forms.RadioButton();
      this.radioBtn_Mid = new System.Windows.Forms.RadioButton();
      ((System.ComponentModel.ISupportInitialize)(this.chartGraph)).BeginInit();
      this.SuspendLayout();
      // 
      // btnAddDataGraph
      // 
      this.btnAddDataGraph.Location = new System.Drawing.Point(950, 547);
      this.btnAddDataGraph.Name = "btnAddDataGraph";
      this.btnAddDataGraph.Size = new System.Drawing.Size(199, 72);
      this.btnAddDataGraph.TabIndex = 0;
      this.btnAddDataGraph.Text = "Нарисовать график";
      this.btnAddDataGraph.UseVisualStyleBackColor = true;
      this.btnAddDataGraph.Click += new System.EventHandler(this.btnAddDataGraph_Click);
      // 
      // chartGraph
      // 
      chartArea2.Name = "ChartArea1";
      this.chartGraph.ChartAreas.Add(chartArea2);
      legend2.Name = "Legend1";
      this.chartGraph.Legends.Add(legend2);
      this.chartGraph.Location = new System.Drawing.Point(12, 12);
      this.chartGraph.Name = "chartGraph";
      series2.BorderWidth = 3;
      series2.ChartArea = "ChartArea1";
      series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      series2.Color = System.Drawing.Color.Red;
      series2.Legend = "Legend1";
      series2.Name = "Name";
      this.chartGraph.Series.Add(series2);
      this.chartGraph.Size = new System.Drawing.Size(932, 604);
      this.chartGraph.TabIndex = 1;
      this.chartGraph.Text = "chart1";
      // 
      // radioBtn_Bid
      // 
      this.radioBtn_Bid.AutoSize = true;
      this.radioBtn_Bid.Location = new System.Drawing.Point(1025, 465);
      this.radioBtn_Bid.Name = "radioBtn_Bid";
      this.radioBtn_Bid.Size = new System.Drawing.Size(48, 20);
      this.radioBtn_Bid.TabIndex = 2;
      this.radioBtn_Bid.TabStop = true;
      this.radioBtn_Bid.Text = "Bid";
      this.radioBtn_Bid.UseVisualStyleBackColor = true;
      // 
      // radioBtn_Ask
      // 
      this.radioBtn_Ask.AutoSize = true;
      this.radioBtn_Ask.Location = new System.Drawing.Point(1026, 423);
      this.radioBtn_Ask.Name = "radioBtn_Ask";
      this.radioBtn_Ask.Size = new System.Drawing.Size(51, 20);
      this.radioBtn_Ask.TabIndex = 3;
      this.radioBtn_Ask.TabStop = true;
      this.radioBtn_Ask.Text = "Ask";
      this.radioBtn_Ask.UseVisualStyleBackColor = true;
      // 
      // radioBtn_Mid
      // 
      this.radioBtn_Mid.AutoSize = true;
      this.radioBtn_Mid.Location = new System.Drawing.Point(1026, 509);
      this.radioBtn_Mid.Name = "radioBtn_Mid";
      this.radioBtn_Mid.Size = new System.Drawing.Size(50, 20);
      this.radioBtn_Mid.TabIndex = 4;
      this.radioBtn_Mid.TabStop = true;
      this.radioBtn_Mid.Text = "Mid";
      this.radioBtn_Mid.UseVisualStyleBackColor = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1161, 628);
      this.Controls.Add(this.radioBtn_Mid);
      this.Controls.Add(this.radioBtn_Ask);
      this.Controls.Add(this.radioBtn_Bid);
      this.Controls.Add(this.chartGraph);
      this.Controls.Add(this.btnAddDataGraph);
      this.Name = "MainForm";
      this.Text = "QuetosApp";
      this.Load += new System.EventHandler(this.MainForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.chartGraph)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddDataGraph;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartGraph;
        private System.Windows.Forms.RadioButton radioBtn_Bid;
        private System.Windows.Forms.RadioButton radioBtn_Ask;
        private System.Windows.Forms.RadioButton radioBtn_Mid;
    }
}

