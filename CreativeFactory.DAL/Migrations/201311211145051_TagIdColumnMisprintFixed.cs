namespace CreativeFactory.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagIdColumnMisprintFixed : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ArticleTag", name: "TagIg", newName: "TagId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.ArticleTag", name: "TagId", newName: "TagIg");
        }
    }
}
