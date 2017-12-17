namespace OnlineShop.DAL.Migrations
{
	using System.Data.Entity.Migrations;
	using System.IO;
	using System.Reflection;
	using System.Web;

	public partial class Initializer : DbMigration
	{
		public override void Up()
		{

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;    

			string sql = File.ReadAllText(path + @"\Resources\SQL.sql");
			Sql(sql);	
		}

		public override void Down()
		{
		}
	}
}
