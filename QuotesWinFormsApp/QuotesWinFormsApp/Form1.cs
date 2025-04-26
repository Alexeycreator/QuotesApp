using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using NLog;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace QuotesWinFormsApp
{
  public partial class MainForm : Form
  {
    #region Fields

    private static Logger logger = LogManager.GetCurrentClassLogger();
    private static string dateGetQuetos = DateTime.Now.ToShortDateString();
    private static int maxWorkerThreads, maxCompletionPortThreads;
    private static int minWorkerThreads, minCompletionPortThreads;
    private Color[] colors = new Color[]
    {
        Color.Red, Color.Blue, Color.Green, Color.Purple, Color.Orange,
        Color.Teal, Color.Magenta, Color.Lime, Color.Brown, Color.Navy,
        Color.Olive, Color.Maroon, Color.Cyan, Color.Gold, Color.Pink,
        Color.Silver, Color.Violet, Color.Indigo, Color.Turquoise, Color.Salmon,
        Color.SkyBlue, Color.Lavender, Color.DarkGreen, Color.Beige, Color.Khaki,
        Color.Coral, Color.Plum, Color.Orchid, Color.Tan, Color.Peru
    };
    private CancellationTokenSource _cts;
    private string quantityPool;

    #endregion

    #region Settings Form

    public MainForm()
    {
      InitializeComponent();
      StartPosition = FormStartPosition.CenterScreen;
    }
    private void MainForm_Load(object sender, EventArgs e)
    {
      try
      {
        var files = GetTodayAllQuoteFiles();
        foreach (var file in files)
        {
          logger.Info($"Найден файл: {file}");
        }
      }
      catch (Exception ex)
      {
        logger.Error($"{ex.Message}");
      }
    }

    #endregion

    #region Buttons Actions

    private void btnAddDataGraph_Click(object sender, EventArgs e)
    {
      try
      {
        //Для определения максимального количества потоков
        ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
        //Для определения минимального количества потоков
        ThreadPool.GetMinThreads(out minWorkerThreads, out minCompletionPortThreads);
        var _quantityPool = QuantityPool();
        logger.Info($"Максимальное количество потоков: {maxWorkerThreads}, " +
            $"максимальное количество доступных потоков: {maxCompletionPortThreads}, " +
            $"выбранное количество потоков: {_quantityPool}.");
        logger.Info($"Минимальное количество потоков: {minWorkerThreads}, " +
            $"Минимальное количество доступных потоков: {minCompletionPortThreads}, " +
            $"выбранное количество потоков: {_quantityPool}.");
        List<DataCoordinates> dataCoordinatesList = DataRecording();
      }
      catch (Exception ex)
      {
        logger.Error(ex);
      }
    }

    #endregion

    #region Methods
    //Расчет значения mid
    private double MidValueCalculation(int bid, int ask)
    {
      return (bid + ask) / 2;
    }

    private string QuantityPool()
    {
      //Для значения количества потоков
      return quantityPool = ConfigurationManager.AppSettings["quantity"];
    }

    //сбор всех файлов за текущую дату
    private List<string> GetTodayAllQuoteFiles()
    {
      string dirCsvFilePath = $@"C:\Users\Алексей\Desktop\Учеба\github\QuotesApp\QuotesConsoleApp\QuotesConsoleApp\QuotesFiles";
      string todayDate = DateTime.Now.ToShortDateString();
      return Directory.GetFiles(dirCsvFilePath, $@"Quotes{todayDate}_*.csv").ToList();
    }

    private List<DataCoordinates> DataRecording()
    {
      List<DataCoordinates> allData = new List<DataCoordinates>();
      _cts?.Cancel();
      _cts = new CancellationTokenSource();
      try
      {
        var todayFiles = GetTodayAllQuoteFiles();
        var _quantityPool = QuantityPool();
        logger.Info($"Найдено {todayFiles.Count} файлов за дату {dateGetQuetos}");
        chartGraph.Invoke((MethodInvoker)(() =>
        {
          chartGraph.Series.Clear();
          seriesCheckedListBox.Items.Clear();
        }));
        ThreadPool.QueueUserWorkItem(state =>
        {
          logger.Info($"Запущен поток отображения данных");
          try
          {
            foreach (var fileName in todayFiles)
            {
              if (_cts.Token.IsCancellationRequested)
              {
                return;
              }
              logger.Info($"Обработка файла: {Path.GetFileName(fileName)}");
              var fileData = ProcessFillingSingleFile(fileName);
              allData.AddRange(fileData);
              chartGraph.Invoke((MethodInvoker)(() =>
              {
                var existingSeries = chartGraph.Series.Select(s => s.Name).ToList();
                var newNames = fileData.Select(d => d.name).Distinct()
                                    .Where(name => !existingSeries.Contains(name));
                foreach (var name in newNames)
                {
                  var series = new Series(name)
                  {
                    ChartType = SeriesChartType.Line,
                    Color = colors[chartGraph.Series.Count % colors.Length],
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 8,
                    IsValueShownAsLabel = true
                  };
                  chartGraph.Series.Add(series);
                  seriesCheckedListBox.Items.Add(name, true);
                }
                foreach (var data in fileData)
                {
                  var series = chartGraph.Series[data.name];
                  int xValue = series.Points.Count + 1;
                  series.Points.AddXY(xValue, GetValueToShow(data));
                }
              }));
              if (fileName != todayFiles.Last() && !_cts.Token.IsCancellationRequested)
              {
                Thread.Sleep(5000);
              }
            }
            logger.Info($"Все файлы обработаны. Всего точек: {allData.Count}");
          }
          catch (OperationCanceledException)
          {
            logger.Info("Обработка отменена");
          }
          catch (Exception ex)
          {
            logger.Error($"Ошибка в потоке обработки: {ex.Message}");
          }
        }, _cts.Token);
      }
      catch (Exception ex)
      {
        logger.Error($"Ошибка: {ex.Message}");
      }

      return allData;
    }
    private List<DataCoordinates> ProcessFillingSingleFile(string filePath)
    {
      var fileData = new List<DataCoordinates>();
      string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();

      foreach (var line in lines)
      {
        var parts = line.Split(';');
        if (parts.Length >= 3)
        {
          fileData.Add(new DataCoordinates
          {
            name = parts[0],
            bid = Convert.ToInt32(parts[1]),
            ask = Convert.ToInt32(parts[2]),
            mid = MidValueCalculation(Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]))
          });
        }
      }
      return fileData;
    }

    private double GetValueToShow(DataCoordinates data)
    {
      if (radioBtn_Ask.Checked)
      {
        return data.ask;
      }
      if (radioBtn_Bid.Checked)
      {
        return data.bid;
      }
      if (radioBtn_Mid.Checked)
      {
        return data.mid;
      }
      return 0;
    }

    private void radioBtn_Bid_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      _cts?.Cancel();
      _cts?.Dispose();
      base.OnFormClosing(e);
    }
  }

  #endregion
}