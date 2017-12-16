using System.Data.Entity;
using OnlineShop.DAL.Entities;

namespace OnlineShop.DAL.Repositories.Implements
{
	internal class CategoryRepo : GenericRepo<Category>, ICategoryRepo
	{
		public CategoryRepo(DbContext context) : base(context)
		{
		}
	}
}
