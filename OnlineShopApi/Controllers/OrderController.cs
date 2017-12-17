using OnlineShop.DAL.Entities;
using OnlineShop.DAL.Repositories;
using OnlineShopApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShopApi.Controllers
{
	public class OrderController : BaseMvcController
    {
        public ActionResult List()
		{
			User user = GetCurrentUser();
			List<GetOrderListViewModel> orders = user.Orders.Select(o => new GetOrderListViewModel
			{
				Id = o.Id,
				Count = o.Products.Count,
				CreateDt = o.CreateDt,
				Price = o.Products.Sum(p => p.Price * o.Products.Count(pr => pr.Id == p.Id))
			}).ToList();
			return View(orders);
		}

		public ActionResult Details(int id)
		{
			User user = GetCurrentUser();
            //List<Product> orderProducts = UnitOfWork.OrderRepo.FindById(id).Products.ToList();
            Order order = UnitOfWork.OrderRepo.FindById(id);
			IEnumerable<GetOrderDetailViewModel> details = order.Products.Select(p => new GetOrderDetailViewModel
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
				Count = UnitOfWork.Context.Database.SqlQuery<int>($"select \"dbo\".get_product_count_from_order({order.Id}, {p.Id});").FirstOrDefault()
			});
            ViewData["sum"] = details.Sum(d => d.Count * d.Price);
			return View(details);
		}

		public ActionResult Delete(int id)
		{
			User user = GetCurrentUser();
			Order order = UnitOfWork.OrderRepo.FindById(id);
			try
			{
				UnitOfWork.OrderRepo.Remove(order);
				return RedirectToAction("List");
			}
			catch (Exception e) when (e is DbUpdateException || e is EntityCommandExecutionException)
			{
				ModelState.AddModelError("", "Для запрошенного действия не хватает прав");
				return RedirectToAction("List");
			}
			catch
			{
				ModelState.AddModelError("", "Ошибка в работе системы");
				return RedirectToAction("List");
			}
		}
    }
}