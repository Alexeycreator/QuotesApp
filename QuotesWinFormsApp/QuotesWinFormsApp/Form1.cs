using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using NLog;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Threading.Tasks;

namespace QuotesWinFormsApp
{
    public partial class MainForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static string dateGetQuetos = DateTime.Now.ToShortDateString();
        private static DateTime timeChart = DateTime.Now;
        private readonly string filePathCSV = $@"C:\Users\Алексей\Desktop\Учеба\github\QuotesApp\QuotesConsoleApp\QuotesConsoleApp\QuotesFiles\Quotes{dateGetQuetos}.csv";
        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
        }
        private void btnAddDataGraph_Click(object sender, EventArgs e)
        {
            try
            {
                //Для определения максимального количества потоков
                ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads);
                //Для значения количества потоков
                string quantityPool = ConfigurationManager.AppSettings["quantity"];
                logger.Info($"Максимальное количество потоков: {workerThreads}, выбранное количество потоков: {quantityPool}.");
                List<DataCoordinates> dataCoordinatesList = DataRecording();
                ShowDataChart(Convert.ToInt32(quantityPool), dataCoordinatesList);
                /*ThreadPool.QueueUserWorkItem(state =>
                {
                    try
                    {
                        int i = 1;
                        logger.Info($"Зашли в цикл, номер итерации: {i}");
                        foreach (var line in dataRows)
                        {
                            Action updateChart = () =>
                            {
                                if (i != 30)
                                {
                                    if (radioBtn_Ask.Checked)
                                    {
                                        logger.Info($"Начало отрисовки точки графика, номер итерации: {i}");
                                        if (dataRows[i].ask == dataRows[i + 1].ask)
                                        {
                                            chartGraph.ChartAreas[0].AxisY.Title = "Ask";
                                            chartGraph.Series[0].Points.AddY(dataRows[i + 1].ask);
                                            chartGraph.Invalidate();
                                            i++;
                                        }
                                        else
                                        {
                                            chartGraph.Series[0].Points.AddY(dataRows[i].ask);
                                            chartGraph.Invalidate();
                                            i++;
                                        }
                                        logger.Info($"Точка {dataRows[i].ask} отрисована на графике");
                                    }
                                    if (radioBtn_Bid.Checked)
                                    {
                                        logger.Info($"Начало отрисовки точки графика, номер итерации: {i}");
                                        if (dataRows[i].bid == dataRows[i + 1].bid)
                                        {
                                            chartGraph.Series[0].Points.AddY(dataRows[i + 1].bid);
                                            chartGraph.Invalidate();
                                            i++;
                                        }
                                        else
                                        {
                                            chartGraph.Series[0].Points.AddY(dataRows[i].bid);
                                            chartGraph.Invalidate();
                                            i++;
                                        }
                                        logger.Info($"Точка {dataRows[i].bid} отрисована на графике");
                                    }
                                }
                            };
                            Invoke(updateChart);
                            Thread.Sleep(100);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                });*/
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        //Расчет значения mid
        private double MidValueCalculation(int bid, int ask)
        {
            double res = (bid + ask) / 2;
            return res;
        }
        //Заполнение данных из файла
        private List<DataCoordinates> DataRecording()
        {
            List<DataCoordinates> dataRow = new List<DataCoordinates>();
            try
            {
                logger.Info("Файл существует.");
                //Считываем данные с файла
                string[] quetos = File.ReadAllLines(filePathCSV);
                logger.Info($"Данные получилось считать.");
                //Добавляем данные в модель данных
                List<QuetosModel> dataRows = quetos.Select(s => s.Split(';')).Select(row => new QuetosModel { name = row[0], bid = row[1], ask = row[2] }).ToList();
                //Добавление данных в структуру графика
                for (int i = 1; i < dataRows.Count; i++)
                {
                    DataCoordinates coordinates = new DataCoordinates()
                    {
                        name = dataRows[i].name,
                        bid = Convert.ToInt32(dataRows[i].bid),
                        ask = Convert.ToInt32(dataRows[i].ask),
                        mid = MidValueCalculation(Convert.ToInt32(dataRows[i].bid), Convert.ToInt32(dataRows[i].ask)),
                    };
                    dataRow.Add(coordinates);
                    logger.Info($"Данные добавлены {i} из 30.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return dataRow;
        }
        //настройка Series
        private void SetupSeries(List<string> name)
        {
            chartGraph.Series.Clear();
            foreach (var item in name)
            {
                Series dataSeries = new Series()
                {
                    Name = item,
                    ChartType = SeriesChartType.Line,
                    Color = Color.Blue,
                    //MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 5,
                };
                chartGraph.Series.Add(dataSeries);
            }
        }
        //отображение данных на графике
        private void ShowDataChart(int defaultPool, List<DataCoordinates> dataCoordinates)
        {
            logger.Info("Сбор данных для отрисовки графика.");
            List<string> nameChartGraph = new List<string>();
            List<int> askChartGraph = new List<int>();
            List<int> bidChartGraph = new List<int>();
            List<int> midChartGraph = new List<int>();
            foreach (var item in dataCoordinates)
            {
                if (!string.IsNullOrEmpty(item.name))
                {
                    nameChartGraph.Add(item.name);
                    askChartGraph.Add(item.ask);
                    bidChartGraph.Add(item.bid);
                    midChartGraph.Add(Convert.ToInt32(item.mid));
                }
            }
            //SetupSeries(nameChartGraph);
            /*List<int> selectedData = null;
            string selectedDataName = string.Empty;
            string nameGraph = string.Empty;*/
            //проверка на кнопку
            if (radioBtn_Ask.Checked)
            {
                logger.Info("Выбрана кнопка для отображения данных ask.");
                /*selectedData = askChartGraph;
                selectedDataName = "ask";
                nameGraph = "nameChartGraph";*/
            }
            else if (radioBtn_Bid.Checked)
            {
                logger.Info("Выбрана кнопка для отображения данных bid.");
                chartGraph.Series["Name"].Points.Clear();
                for (int i = 0; i < defaultPool; i++)
                {
                    chartGraph.Series["Name"].Points.AddXY(0, bidChartGraph[i]);
                    if (i < nameChartGraph.Count)
                    {
                        chartGraph.Series["Name"].Points[i].Label = nameChartGraph[i];
                    }
                    logger.Info($"Данные отрисованы, их значение: {bidChartGraph[i]} с именем {nameChartGraph}");
                }
                logger.Info("Получилось отрисовать данные!");
            }
            else if (radioBtn_Mid.Checked)
            {
                logger.Info("Выбрана кнопка для отображения данных mid.");
            }
            else
            {
                logger.Error("Вы не выбрали данные для отображения на графике!");
                return;
            }
        }
        /*private void DrawChart(List<int> data, int defaultPool, string dataName, string name)
        {
            if (data == null || data.Count == 0)
            {
                return;
            }
            int pointCount = Math.Min(defaultPool, data.Count);
            List<int> points = new List<int>();
            for (int i = 0; i < pointCount; i++) {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    int value = data[i];
                    LogPoint(value, name);
                    lock (points) {
                        points.Add(value);
                    }
                });
            }
        }
        private void LogPoint(int value, string name)
        {
            if (chartGraph.InvokeRequired)
            {
                chartGraph.Invoke((MethodInvoker)(() => AddPointToChart(value, name)));
            }
            else
            {
                AddPointToChart(value, name);
            }
        }
        private void AddPointToChart(int value, string name)
        {
            var series = chartGraph.Series[name];
            series.Points.Add(value);
        }*/
    }
}
//старый код
//DataCoordinates dataRow = new DataCoordinates();
//for (int i = 1; i < quetos.Length; i++)
//{
//    dataRow.name = dataRows[i].name;
//    dataRow.x = Convert.ToInt32(dataRows[i].x);
//    dataRow.y = Convert.ToInt32(dataRows[i].y);
//    dataRow.mid = MidValueCalculation(dataRow.x, dataRow.y);
//    logger.Info($"Данные добавлены {i} из 30.");
//    //Отрисовка точек на графике
//    if (radioBtn_Bid.Checked)
//    {
//        chartGraph.Series[0].Points.AddY(dataRow.x);
//        logger.Info($"Отросовался график с данными Bid.");
//    }
//    else if (radioBtn_Ask.Checked)
//    {
//        chartGraph.Series[0].Points.AddY(dataRow.y);
//        logger.Info($"Отросовался график с данными Ask.");
//    }
//    else if (radioBtn_Mid.Checked)
//    {
//        chartGraph.Series[0].Points.AddY(dataRow.mid);
//        logger.Info($"Отросовался график с данными Mid.");
//    }
//    logger.Info($"Точки добавлены на график.");
//}
//Добавление данных в структуру графика
//for(int i = 1; i < quetos.Length; i++)
//{
//    dataRow.name = dataRows[i].name;
//    dataRow.x = Convert.ToInt32(dataRows[i].x);
//    dataRow.y = Convert.ToInt32(dataRows[i].y);
//    dataRow.mid = MidValueCalculation(dataRow.x, dataRow.y);
//    logger.Info($"Данные добавлены {i} из 30.");
//}
