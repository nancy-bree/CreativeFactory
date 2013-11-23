namespace CreativeFactory.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "Title", c => c.String(maxLength: 128));
            AlterColumn("dbo.Articles", "Description", c => c.String(maxLength: 256));
            AlterColumn("dbo.Users", "UserName", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Items", "Title", c => c.String(maxLength: 128));
            AlterColumn("dbo.Tags", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tags", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Items", "Title", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.Articles", "Description", c => c.String());
            AlterColumn("dbo.Articles", "Title", c => c.String());
        }
    }
}
