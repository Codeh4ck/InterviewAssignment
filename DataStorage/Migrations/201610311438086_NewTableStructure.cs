namespace DataStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTableStructure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsDataModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Provider = c.String(),
                        Date = c.DateTime(nullable: false),
                        Poster = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.TwitterDataModels");
        }
        
        public override void Down()
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
            
            DropTable("dbo.NewsDataModels");
        }
    }
}
