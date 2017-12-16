using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using OnlineShopApi.Models;
using Owin;

[assembly: OwinStartup(typeof(OnlineShopApi.App_Start.Startup))]

namespace OnlineShopApi.App_Start
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.CreatePerOwinContext(ApplicationContext.Create);
			app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Account/Login"),
			});
		}
	}
}
