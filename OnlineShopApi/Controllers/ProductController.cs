using OnlineShop.DAL.Entities;
using OnlineShop.DAL.Repositories;
using OnlineShopApi.ViewModels.Product;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShopApi.Controllers
{
	[Authorize]
	public class ProductController : BaseMvcController
	{
		public ActionResult List(int id = 1, string product = null, int? category = null)
		{
			IProductRepo repo = UnitOfWork.ProductRepo;
			List<Product> products = null;
			if (product != null && category != null)
				products = repo.Get(p => p.CategoryId == category.Value && p.Name.Contains(product)).ToList();
			else
				if (product != null)
					products = repo.Get(p => p.Name.Contains(product)).ToList();
				else
					if (category != null)
						products = repo.Get(p => p.CategoryId == category.Value).ToList();
					else
						products = repo.Get().ToList();
			IEnumerable<GetProductListViewModel> result = products.Select(p => new GetProductListViewModel
			{
				Id = p.Id,
				Name = p.Name,
				Category = p.Category.Name,
				Price = p.Price,
			});
			ViewData["categories"] = UnitOfWork.CategoryRepo.Get().ToList();
			return View(result.ToPagedList(id, 10));
		}

		[HttpGet]
		public ActionResult Create()
		{
			ViewData["categories"] = UnitOfWork.CategoryRepo.Get().ToList();
			return View();
		}

		[HttpPost]
		public ActionResult Create(CreateProductViewModel viewModel)
		{
			User user = GetCurrentUser();

			try
			{
				Category category = UnitOfWork.CategoryRepo.FindById(viewModel.Category);
				Product product = new Product
				{
					Name = viewModel.Name,
					Price = viewModel.Price,
					Description = viewModel.Description,
					Category = category
				};
				UnitOfWork.ProductRepo.Create(product);
				return RedirectToAction("List", "Product");
			}
			catch (DbEntityValidationException e)
			{
				ViewData["categories"] = UnitOfWork.CategoryRepo.Get().ToList();
				IEnumerable<string> messages = e.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
				foreach (string message in messages)
				{
					ModelState.AddModelError("", message);
				}
				return View(viewModel);
			}
			catch (Exception e) when (e is DbUpdateException || e is EntityCommandExecutionException)
			{
				ModelState.AddModelError("", "Для запрошенного действия не хватает прав");
				ViewData["categories"] = UnitOfWork.CategoryRepo.Get().ToList();
				return View(viewModel);
			}
			catch
			{
				ModelState.AddModelError("", "Ошибка в работе системы");
				ViewData["categories"] = UnitOfWork.CategoryRepo.Get().ToList();
				return View(viewModel);
			}
		}

		[HttpGet]
		public ActionResult Delete(int id)
		{
			try
			{
				IProductRepo repo = UnitOfWork.ProductRepo;
				Product product = repo.FindById(id) ?? throw new System.Exception();
				repo.Remove(product);
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

		[HttpGet]
		public ActionResult Edit(int id)
		{
			ViewData["categories"] = UnitOfWork.CategoryRepo.Get().ToList();
			Product product = UnitOfWork.ProductRepo.FindById(id);
			EditProductViewModel viewModel = new EditProductViewModel
			{
				Id = product.Id,
				Name = product.Name,
				Price = product.Price,
				Category = product.CategoryId,
				Description = product.Description
			};
			return View(viewModel);
		}

		[HttpPost]
		public ActionResult Edit(EditProductViewModel viewModel)
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			try
			{
				Product product = UnitOfWork.ProductRepo.FindById(viewModel.Id);
				product.Name = viewModel.Name;
				product.Description = viewModel.Description;
				product.Price = viewModel.Price;
				UnitOfWork.SaveChanges();
				return RedirectToAction("List");
			}
			catch (DbEntityValidationException e)
			{
				IEnumerable<string> messages = e.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
				foreach (string message in messages)
				{
					ModelState.AddModelError("", message);
				}
				return View(viewModel);
			}
			catch (Exception e) when (e is DbUpdateException || e is EntityCommandExecutionException)
			{
				ModelState.AddModelError("", "Для запрошенного действия не хватает прав");
				ViewData["categories"] = UnitOfWork.CategoryRepo.Get().ToList();
				return View(viewModel);
			}
			catch
			{
				ModelState.AddModelError("", "Ошибка в работе системы");
				ViewData["categories"] = UnitOfWork.CategoryRepo.Get().ToList();
				return View(viewModel);
			}
		}

		[HttpGet]
		public ActionResult Details(int id)
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			Product product = UnitOfWork.ProductRepo.FindById(id);
			GetProductViewModel viewModel = new GetProductViewModel
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				Category = product.Category.Name,
				OrdersCount = product.Orders.Count
			};
			return View(viewModel);
		}
	}
}