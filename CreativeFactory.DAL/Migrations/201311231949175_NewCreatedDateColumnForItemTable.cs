namespace CreativeFactory.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewCreatedDateColumnForItemTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "CreatedDate");
        }
    }
}
