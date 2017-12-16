using OnlineShop.DAL.Entities;
using OnlineShopApi.ViewModels.Product;
using System;
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

		[HttpGet]
		public ActionResult Delete(int id)
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			Product product = UnitOfWork.ProductRepo.FindById(id);
			user.Products.Remove(product);
			return RedirectToAction("Basket");
		}

		[HttpGet]
		public ActionResult Buy()
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			try
			{
				Order order = new Order()
				{
					CreateDt = DateTimeOffset.Now,
					User = user,
					Products = user.Products
				};
				UnitOfWork.OrderRepo.Create(order);
				return RedirectToAction("List", "Product");
			}
			catch
			{
				return RedirectToAction("Basket", "Home");
			}
		}
	}
}
