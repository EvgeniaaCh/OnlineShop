namespace OnlineShop.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedLoginAndPasswordFromRoles : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Roles", "Login");
            DropColumn("dbo.Roles", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Roles", "Password", c => c.String());
            AddColumn("dbo.Roles", "Login", c => c.String());
        }
    }
}
