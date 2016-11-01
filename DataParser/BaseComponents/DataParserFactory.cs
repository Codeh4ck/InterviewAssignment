using System;
using DataParser.DataParsers;

namespace DataParser.BaseComponents
{
    public class DataParserFactory
    {
        public static IDataParser CreateParser(string providerName, string searchTerm)
        {
            switch (providerName.ToLower())
            {
                case "twitter":
                    return new TwitterParser(searchTerm);
                default:
                    throw new IndexOutOfRangeException("The parser for the provider given is not yet implemented.");
            }
        }
    }
}