namespace OnlineShop.DAL.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(maxLength: 16));
            DropColumn("dbo.Products", "Count");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Count", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "Name", c => c.String());
        }
    }
}
