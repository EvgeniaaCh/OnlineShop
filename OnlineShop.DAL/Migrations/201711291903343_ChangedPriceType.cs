namespace OnlineShop.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ChangedPriceType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Image", c => c.String());
            AlterColumn("dbo.Products", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Price", c => c.Single(nullable: false));
            AlterColumn("dbo.Products", "Image", c => c.Binary());
        }
    }
}
