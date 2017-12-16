namespace OnlineShop.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 16),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(maxLength: 128),
                        Price = c.Single(nullable: false),
                        Count = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateDt = c.DateTimeOffset(nullable: false, precision: 7),
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
                        Guid = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        FirstName = c.String(maxLength: 16),
                        LastName = c.String(maxLength: 16),
                        CreateDt = c.DateTimeOffset(precision: 7),
                        BirthDt = c.DateTimeOffset(precision: 7),
                        Gender = c.Boolean(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.Email, unique: true, name: "email")
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 16),
                        Connection = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderProducts",
                c => new
                    {
                        Order_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true);
            
            CreateTable(
                "dbo.UserProducts",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.UserProducts", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.OrderProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.OrderProducts", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            //DropIndex("dbo.UserProducts", new[] { "Product_Id" });
            //DropIndex("dbo.UserProducts", new[] { "User_Id" });
            //DropIndex("dbo.OrderProducts", new[] { "Product_Id" });
            //DropIndex("dbo.OrderProducts", new[] { "Order_Id" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Users", "email");
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropTable("dbo.UserProducts");
            DropTable("dbo.OrderProducts");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
