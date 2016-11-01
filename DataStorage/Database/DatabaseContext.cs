using System.Data.Entity;
using DataStorage.Models;

namespace DataStorage.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : 
            base("DatabaseContext") { }

        public DbSet<NewsDataModel> NewsData { get; set; }
    }
}