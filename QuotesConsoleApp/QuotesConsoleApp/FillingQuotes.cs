using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesConsoleApp
{
    class FillingQuotes
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string CSVFilePath = $@"C:\Users\Алексей\Desktop\Учеба\github\QuotesApp\QuotesConsoleApp\QuotesConsoleApp\QuotesFiles\Quotes{dateGetQuotes}.csv";
        private static string dateGetQuotes = DateTime.Now.ToShortDateString();
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
                    int valueAsks = random.Next(1, 100);
                    _logger.Info("Получены рандомные значения для ask");
                    bids[i] = valueBids;
                    asks[i] = valueAsks;
                    if (i <= 10)
                    {
                        foreach (var item in names)
                        {
                            quotes.Add(new QuotesModel
                            {
                                Name = "MSFT",
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
                                Name = "APPL",
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
                                Name = "VIX",
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
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }
    }
}
