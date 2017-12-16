namespace OnlineShop.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDateType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "CreateDt", c => c.DateTime());
            AlterColumn("dbo.Users", "BirthDt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "BirthDt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.Users", "CreateDt", c => c.DateTimeOffset(precision: 7));
        }
    }
}
