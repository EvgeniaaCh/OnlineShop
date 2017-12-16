using OnlineShop.DAL.Entities;
using OnlineShopApi.ViewModels.Product;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShopApi.Controllers
{
	[Authorize]
	public class HomeController : BaseMvcController
	{

		public ActionResult Index()
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			ViewBag.Title = "Home Page";
			return View();
		}

		[HttpGet]
		public ActionResult Basket()
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			ViewBag.ProductCount = user.Products.Count;
			return View(user.Products.Distinct().Select(p => new GetBasketViewModel
			{
				Id = p.Id,
				Name = p.Name,
				Count = UnitOfWork.Context.Database.SqlQuery<int>($"select dbo.get_product_count_from_basket({user.Id}, {p.Id});").FirstOrDefault(),
				PricePerUnit = p.Price
			}));
		}

		[HttpPost]
		public ActionResult Delete()
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");

			return RedirectToAction("Basket");
		}

		[HttpPost]
		public ActionResult Buy()
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			try
			{
				Order order = new Order();
				UnitOfWork.OrderRepo.Create(order);
				foreach (Product product in user.Products)
				{
					order.Products.Add(product);
				}
				UnitOfWork.SaveChanges();
				return RedirectToAction("List", "Product");
			}
			catch
			{
				return RedirectToAction("Basket", "Home");
			}
		}
	}
}
