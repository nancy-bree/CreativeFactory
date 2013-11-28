namespace CreativeFactory.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRatingTable2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ratings", "ItemId", "dbo.Items");
            DropIndex("dbo.Ratings", new[] { "ItemId" });
            AddForeignKey("dbo.Ratings", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            CreateIndex("dbo.Ratings", "ItemId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Ratings", new[] { "ItemId" });
            DropForeignKey("dbo.Ratings", "ItemId", "dbo.Items");
            CreateIndex("dbo.Ratings", "ItemId");
            AddForeignKey("dbo.Ratings", "ItemId", "dbo.Items", "Id");
        }
    }
}
