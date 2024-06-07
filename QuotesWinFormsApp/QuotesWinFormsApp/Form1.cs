using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NLog;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuotesWinFormsApp
{
    public partial class MainForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static string dateGetQuetos = DateTime.Now.ToShortDateString();
        private readonly string filePath = $@"C:\Users\Алексей\Desktop\Учеба\github\QuotesApp\QuotesConsoleApp\QuotesConsoleApp\QuotesFiles\Quotes{dateGetQuetos}.csv";
        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            chartGraph.ChartAreas[0].AxisY.Maximum = 101;
            chartGraph.ChartAreas[0].AxisY.Minimum = -5;
            chartGraph.ChartAreas[0].AxisX.Maximum = 101;
            chartGraph.ChartAreas[0].AxisX.Minimum = -5;
        }
        private void btnAddDataGraph_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Info("Файл существует.");
                //Считываем данные с файла
                string[] quetos = File.ReadAllLines(filePath);
                logger.Info($"Данные получилось считать.");
                //Добавляем данные в структуру
                List<QuetosModel> dataRows = quetos.Select(s => s.Split(';')).Select(row => new QuetosModel { name = row[0], x = row[1], y = row[2] }).ToList();
                DataCoordinates dataRow = new DataCoordinates();
                for (int i = 1; i < quetos.Length; i++)
                {
                    dataRow.name = dataRows[i].name;
                    dataRow.x = Convert.ToInt32(dataRows[i].x);
                    dataRow.y = Convert.ToInt32(dataRows[i].y);
                    dataRow.mid = MidValueCalculation(dataRow.x, dataRow.y);
                    logger.Info($"Данные добавлены {i} из 30");
                    //Отрисовка точек на графике
                    chartGraph.Series[0].Points.AddXY(dataRow.x, dataRow.y);
                    logger.Info($"Точка добавлена на график");
                }
                
            }
            catch (Exception ex)
            {
                logger.Info("Файла на существует!");
                logger.Error(ex);
            }
        }
        //Расчет значения mid
        private double MidValueCalculation(int x, int y)
        {
            double res = (x + y) / 2;
            return res;
        }
    }
}
