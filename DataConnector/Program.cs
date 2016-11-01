using System;
using System.Threading.Tasks;
using DataConnector.BaseComponents;
using DataConnector.Connectors;

namespace DataConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            string Hashtag = "";

            if (args.Length == 1)            
                Hashtag = args[0];
            else
            {
                while (string.IsNullOrEmpty(Hashtag))
                {
                    Console.WriteLine("Enter a hashtag to search for: ");
                    Hashtag = Console.ReadLine();                   
                }
            }

            if (!Hashtag.StartsWith("#"))            
                Hashtag = $"#{Hashtag}";

            IDataConnector Connector = DataConnectorFactory.CreateDataConnector("Twitter");

            Task<int> GetResult = Connector.RetrieveData(Hashtag);
            Task.WaitAll(GetResult);

            int DataCount = GetResult.Result;

            Console.WriteLine($"Number of Tweets received: {DataCount}.");

            Console.ReadKey();
        }        
    }
}
