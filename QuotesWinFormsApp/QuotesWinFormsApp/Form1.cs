﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using NLog;
using System.Configuration;

namespace QuotesWinFormsApp
{
    public partial class MainForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static string dateGetQuetos = DateTime.Now.ToShortDateString();
        private readonly string filePathCSV = $@"C:\Users\Алексей\Desktop\Учеба\github\QuotesApp\QuotesConsoleApp\QuotesConsoleApp\QuotesFiles\Quotes{dateGetQuetos}.csv";
        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            chartGraph.ChartAreas[0].AxisY.Maximum = 100;
            chartGraph.ChartAreas[0].AxisY.Minimum = 0;
            chartGraph.ChartAreas[0].AxisX.Maximum = 100;
            chartGraph.ChartAreas[0].AxisX.Minimum = 0;
            chartGraph.ChartAreas[0].AxisX.Title = "Ось X";
        }
        private void btnAddDataGraph_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Info("Файл существует.");
                //Считываем данные с файла
                string[] quetos = File.ReadAllLines(filePathCSV);
                logger.Info($"Данные получилось считать.");
                //Добавляем данные в модель данных
                List<QuetosModel> dataRows = quetos.Select(s => s.Split(';')).Select(row => new QuetosModel { name = row[0], x = row[1], y = row[2] }).ToList();
                //Добавление данных в структуру графика
                DataCoordinates dataRow = new DataCoordinates();
                for (int i = 1; i < quetos.Length; i++)
                {
                    dataRow.name = dataRows[i].name;
                    dataRow.x = Convert.ToInt32(dataRows[i].x);
                    dataRow.y = Convert.ToInt32(dataRows[i].y);
                    dataRow.mid = MidValueCalculation(dataRow.x, dataRow.y);
                    logger.Info($"Данные добавлены {i} из 30.");

                    if (radioBtn_Mid.Checked)
                    {
                        chartGraph.Series[0].Points.AddY(dataRow.mid);
                        logger.Info($"Отросовался график с данными Mid.");
                    }
                }
                //Для значения количества потоков
                string path = ConfigurationManager.AppSettings["quantity"];
                //многопоточность отрисовки
                ThreadPool.QueueUserWorkItem(state =>
                {
                    try
                    {
                        int i = 1;
                        logger.Info($"Зашли в цикл, номер итерации: {i}");
                        foreach (var line in dataRows)
                        {
                            Action updateChart = () =>
                            {
                                if(i != 30)
                                {
                                    if (radioBtn_Ask.Checked)
                                    {
                                        logger.Info($"Начало отрисовки точки графика, номер итерации: {i}");
                                        if (dataRows[i].y == dataRows[i + 1].y)
                                        {
                                            chartGraph.Series[0].Points.AddY(dataRows[i + 1].y);
                                            chartGraph.Invalidate();
                                            i++;
                                        }
                                        else
                                        {
                                            chartGraph.Series[0].Points.AddY(dataRows[i].y);
                                            chartGraph.Invalidate();
                                            i++;
                                        }
                                        logger.Info($"Точка {dataRows[i].y} отрисована на графике");
                                    }
                                    if (radioBtn_Bid.Checked)
                                    {
                                        logger.Info($"Начало отрисовки точки графика, номер итерации: {i}");
                                        if (dataRows[i].x == dataRows[i + 1].x)
                                        {
                                            chartGraph.Series[0].Points.AddY(dataRows[i + 1].x);
                                            chartGraph.Invalidate();
                                            i++;
                                        }
                                        else
                                        {
                                            chartGraph.Series[0].Points.AddY(dataRows[i].x);
                                            chartGraph.Invalidate();
                                            i++;
                                        }
                                        logger.Info($"Точка {dataRows[i].x} отрисована на графике");
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
                });
            }
            catch (Exception ex)
            {
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
