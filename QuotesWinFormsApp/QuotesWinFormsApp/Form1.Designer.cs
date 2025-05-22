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
      System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
      System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
      System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
      this.btnAddDataGraph = new System.Windows.Forms.Button();
      this.chartGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.radioBtn_Bid = new System.Windows.Forms.RadioButton();
      this.radioBtn_Ask = new System.Windows.Forms.RadioButton();
      this.radioBtn_Mid = new System.Windows.Forms.RadioButton();
      this.seriesCheckedListBox = new System.Windows.Forms.CheckedListBox();
      this.btnInputAll = new System.Windows.Forms.Button();
      this.btnClearAll = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.chartGraph)).BeginInit();
      this.SuspendLayout();
      // 
      // btnAddDataGraph
      // 
      this.btnAddDataGraph.Location = new System.Drawing.Point(950, 547);
      this.btnAddDataGraph.Name = "btnAddDataGraph";
      this.btnAddDataGraph.Size = new System.Drawing.Size(199, 47);
      this.btnAddDataGraph.TabIndex = 0;
      this.btnAddDataGraph.Text = "Нарисовать график";
      this.btnAddDataGraph.UseVisualStyleBackColor = true;
      this.btnAddDataGraph.Click += new System.EventHandler(this.btnAddDataGraph_Click);
      // 
      // chartGraph
      // 
      chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
      chartArea1.AxisX.InterlacedColor = System.Drawing.Color.White;
      chartArea1.AxisX.Interval = 1D;
      chartArea1.AxisX.Minimum = 0D;
      chartArea1.AxisX.ScaleView.MinSize = 1D;
      chartArea1.AxisX.ScaleView.Position = 0D;
      chartArea1.AxisX.ScaleView.Size = 10D;
      chartArea1.AxisX.ScaleView.SizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
      chartArea1.AxisX.ScrollBar.BackColor = System.Drawing.Color.White;
      chartArea1.AxisX.ScrollBar.ButtonColor = System.Drawing.Color.Gray;
      chartArea1.AxisX.ScrollBar.IsPositionedInside = false;
      chartArea1.AxisX.TitleFont = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      chartArea1.AxisY.Interval = 10D;
      chartArea1.AxisY.Maximum = 100D;
      chartArea1.AxisY.Minimum = 0D;
      chartArea1.Name = "ChartArea1";
      this.chartGraph.ChartAreas.Add(chartArea1);
      legend1.Name = "Legend1";
      this.chartGraph.Legends.Add(legend1);
      this.chartGraph.Location = new System.Drawing.Point(12, 12);
      this.chartGraph.Name = "chartGraph";
      series1.BorderWidth = 3;
      series1.ChartArea = "ChartArea1";
      series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
      series1.Color = System.Drawing.Color.Red;
      series1.Legend = "Legend1";
      series1.Name = "Name";
      this.chartGraph.Series.Add(series1);
      this.chartGraph.Size = new System.Drawing.Size(932, 629);
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
      this.radioBtn_Bid.CheckedChanged += new System.EventHandler(this.radioBtn_Bid_CheckedChanged);
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
      this.radioBtn_Ask.CheckedChanged += new System.EventHandler(this.radioBtn_Ask_CheckedChanged);
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
      this.radioBtn_Mid.CheckedChanged += new System.EventHandler(this.radioBtn_Mid_CheckedChanged);
      // 
      // seriesCheckedListBox
      // 
      this.seriesCheckedListBox.FormattingEnabled = true;
      this.seriesCheckedListBox.Location = new System.Drawing.Point(950, 12);
      this.seriesCheckedListBox.Name = "seriesCheckedListBox";
      this.seriesCheckedListBox.Size = new System.Drawing.Size(199, 378);
      this.seriesCheckedListBox.TabIndex = 5;
      // 
      // btnInputAll
      // 
      this.btnInputAll.Location = new System.Drawing.Point(950, 600);
      this.btnInputAll.Name = "btnInputAll";
      this.btnInputAll.Size = new System.Drawing.Size(98, 47);
      this.btnInputAll.TabIndex = 7;
      this.btnInputAll.Text = "Выделить все";
      this.btnInputAll.UseVisualStyleBackColor = true;
      this.btnInputAll.Click += new System.EventHandler(this.btnInputAll_Click);
      // 
      // btnClearAll
      // 
      this.btnClearAll.Location = new System.Drawing.Point(1051, 600);
      this.btnClearAll.Name = "btnClearAll";
      this.btnClearAll.Size = new System.Drawing.Size(98, 47);
      this.btnClearAll.TabIndex = 8;
      this.btnClearAll.Text = "Очистить все";
      this.btnClearAll.UseVisualStyleBackColor = true;
      this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1161, 653);
      this.Controls.Add(this.btnClearAll);
      this.Controls.Add(this.btnInputAll);
      this.Controls.Add(this.seriesCheckedListBox);
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
    private System.Windows.Forms.CheckedListBox seriesCheckedListBox;
    private System.Windows.Forms.Button btnInputAll;
    private System.Windows.Forms.Button btnClearAll;
  }
}

