using DataStorage.Models;

namespace DataStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Database.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataStorage.Database.DatabaseContext context)
        {
            context.NewsData.AddOrUpdate(t => t.Poster, new NewsDataModel[]
            {
                new NewsDataModel()
                {
                    Date = DateTime.Now,
                    Poster = "RandomPoster",
                    Message = "This is a migration tweet for #felixbaumgartner",
                    Provider = "Twitter",
                    SearchTerm = "#felixbaumgartner"
                },
                new NewsDataModel()
                {
                    Date = DateTime.Now.Subtract(TimeSpan.FromDays(4)),
                    Poster = "another_poster",
                    Message = "This is a migration tweet for #felixbaumgartner",
                    Provider = "Twitter",
                    SearchTerm = "#felixbaumgartner"
                },
                new NewsDataModel()
                {
                    Date = DateTime.Now.Subtract(TimeSpan.FromHours(12)),
                    Poster = "random_poster_2",
                    Message = "This is a migration tweet for #felixbaumgartner",
                    Provider = "Twitter",
                    SearchTerm = "#felixbaumgartner"
                },
                new NewsDataModel()
                {
                    Date = DateTime.Now.Subtract(TimeSpan.FromMinutes(10)),
                    Poster = "AnotherPoster",
                    Message = "This is a migration tweet for #felixbaumgartner",
                    Provider = "Twitter",
                    SearchTerm = "#felixbaumgartner"
                },
            });

        }
    }
}
