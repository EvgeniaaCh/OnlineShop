namespace OnlineShop.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Consolidation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Image", c => c.String());
        }
    }
}
