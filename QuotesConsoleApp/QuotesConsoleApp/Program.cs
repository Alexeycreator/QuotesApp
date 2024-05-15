using NLog;
using System;

namespace QuotesConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            FillingQuotes fillingQuotes = new FillingQuotes();
            fillingQuotes.GetQuotes();
            Console.WriteLine("Приложение закончило работу!");
            Console.ReadKey();
        }
    }
}
