using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OnlineShop.DAL.Entities;
using OnlineShopApi.Models;
using OnlineShopApi.ViewModels.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopApi.Controllers
{
	[AllowAnonymous]
	public class AccountController : BaseMvcController
	{
		private ApplicationUserManager UserManager
			=> HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

		private IAuthenticationManager AuthenticationManager
			=> HttpContext.GetOwinContext().Authentication;

		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				IdentityUser user = UserManager.Find(viewModel.Email, viewModel.Password);
				if (user == null)
				{
					ModelState.AddModelError("", "Неверный логин или пароль.");
				}
				else
				{
					ClaimsIdentity claim = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
					AuthenticationManager.SignOut();
					AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claim);
					return RedirectToAction("List", "Product");
				}
			}
			return View(viewModel);
		}

		[HttpGet]
		public ActionResult Registration()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Registration(RegistrationViewModel viewModel)
		{
			IdentityUser identityUser = new IdentityUser()
			{
				Email = viewModel.Email,
				UserName = viewModel.Email
			};
			IdentityResult result = UserManager.Create(identityUser, viewModel.Password);
			if (result.Succeeded)
			{
				User user = Mapper.Map(viewModel, typeof(RegistrationViewModel), typeof(User)) as User;
				user.Guid = identityUser.Id;
				Role role = UnitOfWork.RoleRepo.Get(r => r.Name == "user").FirstOrDefault();
				user.Role = role;
				user.CreateDt = DateTime.Now;
				UnitOfWork.UserRepo.Create(user);
				return RedirectToAction("List", "Product");
			}
			foreach (string error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
			return View(viewModel);
		}

		public ActionResult Logout()
		{
			AuthenticationManager.SignOut();
			return RedirectToAction("Login");
		}
	}
}