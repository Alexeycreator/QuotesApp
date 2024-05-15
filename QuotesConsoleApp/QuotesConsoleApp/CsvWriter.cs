using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesConsoleApp
{
    class CsvWriter
    {
        public void Write(string CSVFilePath, List<QuotesModel> quotes)
        {
            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Name;Bid;Ask");
            foreach (var item in quotes)
            {
                csvBuilder.AppendLine($"{item.Name};{item.Bid};{item.Ask}");
            }
            File.WriteAllText(CSVFilePath, csvBuilder.ToString());
        }
    }
}
