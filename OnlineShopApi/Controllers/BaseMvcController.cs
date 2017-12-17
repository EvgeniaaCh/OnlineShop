using Microsoft.AspNet.Identity.EntityFramework;
using OnlineShop.DAL;
using OnlineShop.DAL.Entities;
using OnlineShopApi.Models;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShopApi.Controllers
{
	public abstract class BaseMvcController : Controller
	{
		protected IUnitOfWork UnitOfWork { get; private set; } = new UnitOfWork();

		protected User GetCurrentUser()
		{
			string userName = HttpContext.User.Identity.Name;
			IdentityUser identityUser = new ApplicationContext().Users.FirstOrDefault(u => u.UserName == userName);
			User user = UnitOfWork.UserRepo.Get().FirstOrDefault(u => u.Guid == identityUser.Id);
			try
			{
				UnitOfWork = new UnitOfWork(user.Role.Connection);
				user = UnitOfWork.UserRepo.FindById(user.Id);
			}
			catch { }
			
			return user;
		}
	}
}