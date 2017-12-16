using System.Data.Entity;
using OnlineShop.DAL.Entities;

namespace OnlineShop.DAL.Repositories.Implements
{
	internal class RoleRepo : GenericRepo<Role>, IRoleRepo
	{
		public RoleRepo(DbContext context) : base(context)
		{
		}
	}
}
