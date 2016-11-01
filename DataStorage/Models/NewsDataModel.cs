using System;
using System.ComponentModel.DataAnnotations;

namespace DataStorage.Models
{
    public class NewsDataModel
    {
        [Key]
        public string Id { get; private set; }
        public string Provider { get; set; }
        public DateTime Date { get; set; }
        public string Poster { get; set; }
        public string Message { get; set; }
        public string SearchTerm { get; set; }

        public NewsDataModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}