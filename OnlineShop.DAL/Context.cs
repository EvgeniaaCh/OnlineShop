using OnlineShop.DAL.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace OnlineShop.DAL
{
	public class Context : DbContext
	{
		public Context() : base("admin")
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Migrations.Configuration>());
		}

		public Context(string connection) : base(connection)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Migrations.Configuration>());
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Configurations.Add(new EntityTypeConfiguration<Category>());
			modelBuilder.Configurations.Add(new EntityTypeConfiguration<Role>());
			modelBuilder.Configurations.Add(new EntityTypeConfiguration<User>());
			modelBuilder.Configurations.Add(new EntityTypeConfiguration<Product>());
			modelBuilder.Configurations.Add(new EntityTypeConfiguration<Order>());
		}
	}
}
