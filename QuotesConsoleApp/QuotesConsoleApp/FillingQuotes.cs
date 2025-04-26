using NLog;
using System;
using System.Collections.Generic;

namespace QuotesConsoleApp
{
  class FillingQuotes
  {
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    private static string dateGetQuotes = DateTime.Now.ToShortDateString();
    private static string timeGetQoutes = DateTime.Now.ToString("HH-mm-ss");
    private readonly string CSVFilePath = $@"C:\Users\Алексей\Desktop\Учеба\github\QuotesApp\QuotesConsoleApp\QuotesConsoleApp\QuotesFiles\Quotes{dateGetQuotes}_{timeGetQoutes}.csv";
    private CsvWriter csvWriter = new CsvWriter();
    private Random random = new Random();

    public string GetQuotes()
    {
      try
      {
        List<QuotesModel> quotes = new List<QuotesModel>();
        string[] names = { "MSFT" };
        int[] bids = new int[30];
        int[] asks = new int[30];
        //Заполнение полей
        _logger.Info("Приступаем к заполнению полей");
        for (int i = 0; i != 30; i++)
        {
          int valueBids = random.Next(1, 100);
          _logger.Info("Получены рандомные значения для bid");
          int downLimit = (int)(valueBids / 2);
          int upperLimit = (int)(valueBids * 2) > 100 ? 100 : (int)(valueBids * 2);
          int valueAsks = random.Next(downLimit, upperLimit);
          _logger.Info("Получены рандомные значения для ask");
          bids[i] = valueBids;
          asks[i] = valueAsks;
          if (i <= 10)
          {
            foreach (var item in names)
            {
              quotes.Add(new QuotesModel
              {
                Name = $"MSFT_{i + 1}",
                Bid = bids[i],
                Ask = asks[i],
              });
            }
            _logger.Info($"Заполнены значения {i} элемента фирмы MSFT");
          }
          else if (i > 10 && i <= 20)
          {
            foreach (var item in names)
            {
              quotes.Add(new QuotesModel
              {
                Name = $"APPL_{i + 1}",
                Bid = bids[i],
                Ask = asks[i],
              });
            }
            _logger.Info($"Заполнены значения {i} элемента фирмы APPL");
          }
          else if (i > 20 && i <= 30)
          {
            foreach (var item in names)
            {
              quotes.Add(new QuotesModel
              {
                Name = $"VIX_{i + 1}",
                Bid = bids[i],
                Ask = asks[i],
              });
            }
            _logger.Info($"Заполнены значения {i} элемента фирмы VIX");
          }
        }
        _logger.Info($"Идет запись в файл, количество данных: {30}");
        csvWriter.Write(CSVFilePath, quotes);
        _logger.Info($"Файлы успешно записаны! {30} из {30}");
      }
      catch (Exception ex)
      {
        _logger.Error(ex);
      }
      return null;
    }
  }
}
