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
    private int countMining;
    private int allCountMining;
    private List<DataCoordinates> allData = new List<DataCoordinates>();
    private int sizeCountMining = 0;
    private int _countFiles = 0;
    private List<DataCoordinates> fileData;
    private List<string> todayFiles;

    #endregion

    #region Settings Form

    public MainForm()
    {
      InitializeComponent();
      StartPosition = FormStartPosition.CenterScreen;
    }
    private void MainForm_Load(object sender, EventArgs e)
    {
      Settings();
    }

    #endregion

    #region Buttons Actions

    private void btnAddDataGraph_Click(object sender, EventArgs e)
    {
      try
      {
        if (!radioBtn_Ask.Checked && !radioBtn_Bid.Checked && !radioBtn_Mid.Checked)
        {
          logger.Error($"Не выбран параметр отображаения");
          MessageBox.Show($"Не выбран параметр отображения");
          return;
        }
        List<DataCoordinates> dataCoordinatesList = DataRecording();
        btnAddDataGraph.Enabled = false;
        btnInputAll.Enabled = true;
        btnClearAll.Enabled = true;
      }
      catch (Exception ex)
      {
        logger.Error(ex);
      }
    }

    private void btnInputAll_Click(object sender, EventArgs e)
    {
      for (int i = 0; i < countMining; i++)
      {
        seriesCheckedListBox.SetItemCheckState(i, CheckState.Checked);
      }
    }

    private void btnClearAll_Click(object sender, EventArgs e)
    {
      if (allCountMining != sizeCountMining)
      {
        MessageBox.Show($"Нельзя очистить все элементы, еще не все заполнено");
        return;
      }
      for (int i = 0; i < countMining; i++)
      {
        seriesCheckedListBox.SetItemCheckState(i, CheckState.Unchecked);
      }
    }

    #endregion

    #region RadioButtons Handlers

    private void radioBtn_Ask_CheckedChanged(object sender, EventArgs e)
    {
      if (!btnAddDataGraph.Enabled && radioBtn_Ask.Checked)
      {
        RedrawChartWithCurrentSelection();
      }
      if (radioBtn_Ask.Checked && btnAddDataGraph.Enabled)
      {
        return;
      }
    }

    private void radioBtn_Bid_CheckedChanged(object sender, EventArgs e)
    {
      if (!btnAddDataGraph.Enabled && radioBtn_Bid.Checked)
      {
        RedrawChartWithCurrentSelection();
      }
      if (radioBtn_Bid.Checked && btnAddDataGraph.Enabled)
      {
        return;
      }
    }

    private void radioBtn_Mid_CheckedChanged(object sender, EventArgs e)
    {
      if (!btnAddDataGraph.Enabled && radioBtn_Mid.Checked)
      {
        RedrawChartWithCurrentSelection();
      }
      if (radioBtn_Mid.Checked && btnAddDataGraph.Enabled)
      {
        return;
      }
    }

    #endregion

    #region Methods

    //Расчет значения mid
    private double MidValueCalculation(int bid, int ask)
    {
      return (bid + ask) / 2;
    }

    private void Settings()
    {
      btnClearAll.Enabled = false;
      btnInputAll.Enabled = false;
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
      if (Convert.ToInt32(_quantityPool) < minCompletionPortThreads)
      {
        logger.Info($"Количество выбранных потоков ({_quantityPool}) не может быть меньше минимального количества доступных потоков ({minCompletionPortThreads})");
        _quantityPool = minCompletionPortThreads.ToString();
        logger.Info($"Количество выбранных потоков изменено = {_quantityPool}");
      }
      try
      {
        todayFiles = GetTodayAllQuoteFiles();
        if (todayFiles != null)
        {
          foreach (var file in todayFiles)
          {
            _countFiles++;
            logger.Info($"Найден {_countFiles} файл: {file}");
            string[] lines = File.ReadAllLines(file).Skip(1).ToArray();
            sizeCountMining += lines.Length;
          }
        }
        logger.Info($"Всего точек должно быть {sizeCountMining}");
      }
      catch (Exception ex)
      {
        logger.Error($"{ex.Message}");
      }
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

    //отрисовка точек на графике
    private List<DataCoordinates> DataRecording()
    {
      _cts?.Cancel();
      _cts = new CancellationTokenSource();
      try
      {
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
              logger.Info($"\n----------------------------------------------------------\nОбработка файла: {Path.GetFileName(fileName)}");
              fileData = ProcessFillingSingleFile(fileName);
              allData.AddRange(fileData);
              chartGraph.Invoke((MethodInvoker)(() =>
              {
                var existingSeries = chartGraph.Series.Select(s => s.Name).ToList();
                var newNames = fileData.Select(d => d.name).Distinct()
                                    .Where(name => !existingSeries.Contains(name));
                seriesCheckedListBox.ItemCheck += (s, e) =>
                {
                  if (e.Index >= 0 && e.Index < chartGraph.Series.Count)
                  {
                    chartGraph.Series[e.Index].Enabled = (e.NewValue == CheckState.Checked);
                  }
                };
                foreach (var name in newNames)
                {
                  var series = new Series(name)
                  {
                    ChartType = SeriesChartType.Line,
                    Color = colors[chartGraph.Series.Count % colors.Length],
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 8,
                    Enabled = true,
                    IsValueShownAsLabel = false,
                  };
                  chartGraph.Series.Add(series);
                  seriesCheckedListBox.Items.Add(name, true);
                }
                chartGraph.ChartAreas[0].CursorX.IsUserEnabled = true;
                chartGraph.ChartAreas[0].CursorY.IsUserEnabled = true;
                foreach (var data in fileData)
                {
                  var series = chartGraph.Series[data.name];
                  int xValue = series.Points.Count;
                  double yValue = GetValueToShow(data);
                  DataPoint point = new DataPoint(xValue, yValue)
                  {
                    ToolTip = $"Значение в точке = {yValue}",
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 8,
                    MarkerColor = series.Color
                  };
                  series.Points.Add(point);
                  logger.Info($"Отрисована точка {xValue + 1} файла {fileName} со значением {yValue}");
                }
              }));
              logger.Info($"Обработано {allData.Count} точек из {sizeCountMining}");
              allCountMining = allData.Count;
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

    //заполнение данных для одного файла
    private List<DataCoordinates> ProcessFillingSingleFile(string filePath)
    {
      logger.Info($"Заполнение данных из файла");
      fileData = new List<DataCoordinates>();
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
            mid = MidValueCalculation(Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2])),
          });
          logger.Info($"Заполнено значение {fileData.Count} точки из {lines.Count()}");
        }
      }
      if (fileData.Count == lines.Count())
      {
        logger.Info($"Все точки из файла {filePath} считаны успешно");
        countMining = fileData.Count();
      }
      return fileData;
    }

    //для выбора значений
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

    //Для отображения нужного параметра в потоке
    private void RedrawChartWithCurrentSelection()
    {
      chartGraph.Invoke((MethodInvoker)(() =>
      {
        foreach (var series in chartGraph.Series)
        {
          series.Points.Clear();
        }
        foreach (var data in allData)
        {
          var series = chartGraph.Series[data.name];
          int xValue = series.Points.Count;
          double yValue = GetValueToShow(data);
          DataPoint point = new DataPoint(xValue, yValue)
          {
            ToolTip = $"Значение в точке = {yValue}",
            MarkerStyle = MarkerStyle.Circle,
            MarkerSize = 8,
            MarkerColor = series.Color
          };
          series.Points.Add(point);
        }
      }));
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