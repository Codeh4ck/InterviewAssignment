namespace DataStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleTweets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TwitterDataModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Poster = c.String(),
                        Tweet = c.String(),
                        Provider = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TwitterDataModels");
        }
    }
}
