using System.Data.Entity;
using OnlineShop.DAL.Entities;

namespace OnlineShop.DAL.Repositories.Implements
{
	internal class UserRepo : GenericRepo<User>, IUserRepo
	{
		public UserRepo(DbContext context) : base(context)
		{
		}
	}
}
