using System;

namespace DataParser.Models
{
    public class DataModel
    {
        public string Provider { get; set; }
        public DateTime Date { get; set; }
        public string Poster { get; set; }
        public string Message { get; set; }
        public string SearchTerm { get; set; }
    }
}