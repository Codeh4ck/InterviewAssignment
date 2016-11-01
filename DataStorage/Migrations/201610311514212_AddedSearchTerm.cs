namespace DataStorage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSearchTerm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsDataModels", "SearchTerm", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NewsDataModels", "SearchTerm");
        }
    }
}
