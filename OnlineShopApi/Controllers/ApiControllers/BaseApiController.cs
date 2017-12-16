using Microsoft.AspNet.Identity.EntityFramework;
using OnlineShop.DAL;
using OnlineShop.DAL.Entities;
using OnlineShopApi.Models;
using System.Linq;
using System.Web.Http;

namespace OnlineShopApi.Controllers.ApiControllers
{
	public abstract class BaseApiController : ApiController
	{
		protected IUnitOfWork UnitOfWork { get; private set; } = new UnitOfWork();

		protected User GetCurrentUser()
		{
			string name = User.Identity.Name;
			ApplicationContext context = new ApplicationContext();
			IdentityUser identityUser = context.Users.FirstOrDefault(u => u.UserName == name);
			context.Dispose();
			User user = UnitOfWork.UserRepo.Get(u => u.Guid == identityUser.Id).FirstOrDefault();
			int id = user.Id;
			UnitOfWork = new UnitOfWork(user.Role.Connection);
			user = UnitOfWork.UserRepo.FindById(id);
			return user;
		}
	}
}