using Logic;
using System;

namespace TesterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainScraperFlow flow = new();

            flow.TestDownloadFile().Wait();
        }
    }
}
