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
using System.Threading.Tasks;

namespace QuotesWinFormsApp
{
  public partial class MainForm : Form
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();
    private static string dateGetQuetos = DateTime.Now.ToShortDateString();
    private static DateTime timeChart = DateTime.Now;
    private readonly string filePathCSV = $@"C:\Users\Алексей\Desktop\Учеба\github\QuotesApp\QuotesConsoleApp\QuotesConsoleApp\QuotesFiles\Quotes{dateGetQuetos}.csv";
    private bool working = true;
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
        ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
        //Для определения минимального количества потоков
        ThreadPool.GetMinThreads(out minWorkerThreads, out minCompletionPortThreads);
        //Для значения количества потоков
        string quantityPool = ConfigurationManager.AppSettings["quantity"];
        logger.Info($"Максимальное количество потоков: {maxWorkerThreads}, " +
            $"максимальное количество доступных потоков: {maxCompletionPortThreads}, " +
            $"выбранное количество потоков: {quantityPool}.");
        logger.Info($"Минимальное количество потоков: {minWorkerThreads}, " +
            $"Минимальное количество доступных потоков: {minCompletionPortThreads}, " +
            $"выбранное количество потоков: {quantityPool}.");
        List<DataCoordinates> dataCoordinatesList = DataRecording();
        ShowDataChart(Convert.ToInt32(quantityPool), dataCoordinatesList);
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
    private void SetupSeries(List<string> names)
    {
      chartGraph.Series.Clear();
      for (int i = 0; i < names.Count; i++)
      {
        Series dataSeries = new Series()
        {
          Name = names[i],
          ChartType = SeriesChartType.Line,
          MarkerStyle = MarkerStyle.Circle,
          Color = colors[i],
          MarkerSize = 10,
          MarkerColor = colors[i],
          IsValueShownAsLabel = true,
          SmartLabelStyle = {
            Enabled = true,
            MovingDirection = LabelAlignmentStyles.Top,
          }
        };
        chartGraph.Series.Add(dataSeries);
      }
    }

    private void ShowDataChart(int defaultPool, List<DataCoordinates> dataCoordinates)
    {
      _cts?.Cancel();
      _cts = new CancellationTokenSource();
      logger.Info("Сбор данных для отрисовки графика.");
      var nameChartGraph = new List<string>();
      var askChartGraph = new List<int>();
      var bidChartGraph = new List<int>();
      var midChartGraph = new List<int>();
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
      SetupSeries(nameChartGraph);
      List<int> valuesToDraw;
      string seriesName;
      if (radioBtn_Ask.Checked)
      {
        valuesToDraw = askChartGraph;
        seriesName = "ask";
      }
      else if (radioBtn_Bid.Checked)
      {
        valuesToDraw = bidChartGraph;
        seriesName = "bid";
      }
      else if (radioBtn_Mid.Checked)
      {
        valuesToDraw = midChartGraph;
        seriesName = "mid";
      }
      else
      {
        logger.Error("Не выбраны данные для отображения!");
        return;
      }
      ThreadPool.QueueUserWorkItem(_ =>
          UpdateSeriesOnThread(nameChartGraph, valuesToDraw, seriesName, defaultPool, _cts.Token));
    }

    private void UpdateSeriesOnThread(List<string> namesChart, List<int> values, string seriesName, int defPool, CancellationToken cancellationToken)
    {
      logger.Info($"Отрисовка данных {seriesName} в фоновом потоке.");
      var groupedData = values
          .Select((x, index) => new { Index = index, Value = x })
          .GroupBy(x => x.Index / defPool)
          .ToList();
      int count = 0;
      foreach (var group in groupedData)
      {
        if (cancellationToken.IsCancellationRequested)
        {
          logger.Info("Отрисовка отменена.");
          return;
        }
        if (group.Key == 0)
        {
          chartGraph.Invoke((MethodInvoker)(() =>
              chartGraph.Series[0].Points.Clear()));
        }
        foreach (var item in group)
        {
          if (cancellationToken.IsCancellationRequested)
          {
            return;
          }
          chartGraph.Invoke((MethodInvoker)(() =>
          {
            chartGraph.Series[0].Points.AddXY(item.Index + 1, item.Value);
            var point = chartGraph.Series[0].Points.Last();
            point.Label = namesChart[item.Index];
            point.Color = colors[item.Index % colors.Length];
            point.MarkerColor = colors[item.Index % colors.Length];
            point.ToolTip = $"X: {item.Index + 1}, Y: {item.Value}\n{namesChart[item.Index]}";
          }));
          count++;
          logger.Info($"Добавлена точка: X={item.Index + 1}, Y={item.Value}, Label={namesChart[item.Index]}");
        }
        chartGraph.Invoke((MethodInvoker)(() => chartGraph.Update()));
        Thread.Sleep(5000);
      }
      logger.Info("Отрисовка завершена.");
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      _cts?.Cancel();
      _cts?.Dispose();
      base.OnFormClosing(e);
    }
  }
}