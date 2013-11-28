namespace CreativeFactory.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRatingTable : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Ratings", "ItemId", "dbo.Items", "Id");
            AddForeignKey("dbo.Ratings", "UserId", "dbo.Users", "Id");
            CreateIndex("dbo.Ratings", "ItemId");
            CreateIndex("dbo.Ratings", "UserId");
            DropColumn("dbo.Ratings", "Vote");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ratings", "Vote", c => c.Int(nullable: false));
            DropIndex("dbo.Ratings", new[] { "UserId" });
            DropIndex("dbo.Ratings", new[] { "ItemId" });
            DropForeignKey("dbo.Ratings", "UserId", "dbo.Users");
            DropForeignKey("dbo.Ratings", "ItemId", "dbo.Items");
        }
    }
}
