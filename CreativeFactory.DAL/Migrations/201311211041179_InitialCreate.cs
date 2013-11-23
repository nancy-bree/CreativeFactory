namespace CreativeFactory.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(nullable: false),
                        Order = c.Int(nullable: false),
                        ArticleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Vote = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleTag",
                c => new
                    {
                        ArticleId = c.Int(nullable: false),
                        TagIg = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ArticleId, t.TagIg })
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagIg, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.TagIg);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ArticleTag", new[] { "TagIg" });
            DropIndex("dbo.ArticleTag", new[] { "ArticleId" });
            DropIndex("dbo.Items", new[] { "ArticleId" });
            DropIndex("dbo.Articles", new[] { "UserId" });
            DropForeignKey("dbo.ArticleTag", "TagIg", "dbo.Tags");
            DropForeignKey("dbo.ArticleTag", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Items", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Articles", "UserId", "dbo.Users");
            DropTable("dbo.ArticleTag");
            DropTable("dbo.Ratings");
            DropTable("dbo.Tags");
            DropTable("dbo.Items");
            DropTable("dbo.Users");
            DropTable("dbo.Articles");
        }
    }
}
