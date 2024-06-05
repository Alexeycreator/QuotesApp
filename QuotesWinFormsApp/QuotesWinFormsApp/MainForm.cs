using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesWinFormsApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadDataFileCsv();
        }
        private void LoadDataFileCsv()
        {
            string filePath = "";
            List<double> xValues = new List<double>();
            List<double> yValues = new List<double>();
            string[] lines = File.ReadAllLines(filePath);
            foreach(var line in lines)
            {
                string[] result = line.Split(',');
                xValues.Add(Convert.ToDouble(result[1]));
                yValues.Add(Convert.ToDouble(result[2]));
            }
            Task.Run(() =>
            {
                Parallel.For(0, xValues.Count, i =>
                {
                    double x = xValues[i];
                    double y = yValues[i];
                });
            });
        }
    }
}
