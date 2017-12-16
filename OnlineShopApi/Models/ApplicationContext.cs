using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineShopApi.Models
{
	public class ApplicationContext : IdentityDbContext<IdentityUser>
	{
		public ApplicationContext() : base("identity") { }

		public static ApplicationContext Create()
		{
			return new ApplicationContext();
		}
	}
}