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
        public void Write(string CSVFilePath)
        {
            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("");
            //foreach ()
            //{

            //}
            File.WriteAllText(CSVFilePath, csvBuilder.ToString());
        }
    }
}
