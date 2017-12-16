using System.Data.Entity;
using OnlineShop.DAL.Entities;

namespace OnlineShop.DAL.Repositories.Implements
{
	internal class ProductRepo : GenericRepo<Product>, IProductRepo
	{
		public ProductRepo(DbContext context) : base(context)
		{
		}
	}
}
