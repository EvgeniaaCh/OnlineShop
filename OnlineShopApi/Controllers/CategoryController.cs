using OnlineShop.DAL.Entities;
using OnlineShop.DAL.Repositories;
using OnlineShopApi.ViewModels.Categories;
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
	public class CategoryController : BaseMvcController
    {
		[HttpGet]
		public ActionResult List(string name = null)
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			ICategoryRepo repo = UnitOfWork.CategoryRepo;
			return View(repo.Get().Select(c => new GetCategoryListViewModel
			{
				Id = c.Id,
				Name = c.Name
			}));
		}

		[HttpGet]
		public ActionResult Create()
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			return View();
		}

		[HttpPost]
		public ActionResult Create(CreateCategoryViewModel viewModel)
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			try
			{
				UnitOfWork.CategoryRepo.Create(new Category { Name = viewModel.Name });
				return RedirectToAction("List");
			}
			catch(DbEntityValidationException e)
			{
				IEnumerable<string> messages = e.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
				foreach (string message in messages)
				{
					ModelState.AddModelError("", message);
				}
				return View(viewModel);
			}
		}

		[HttpGet]
		public ActionResult Edit(int id)
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			Category category = UnitOfWork.CategoryRepo.FindById(id);
			EditCategoryViewModel viewModel = new EditCategoryViewModel
			{
				Id = category.Id,
				Name = category.Name
			};
			return View(viewModel);
		}

		[HttpPost]
		public ActionResult Edit(EditCategoryViewModel viewModel)
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			Category category = UnitOfWork.CategoryRepo.FindById(viewModel.Id);
			try
			{
				category.Name = viewModel.Name;
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
		}

		[HttpGet]
		public ActionResult Delete(int id)
		{
			User user = GetCurrentUser();
			if (user == null) return RedirectToAction("Login", "Account");
			Category category = UnitOfWork.CategoryRepo.FindById(id);
			try
			{
				UnitOfWork.CategoryRepo.Remove(category);
				return RedirectToAction("List");
			}
			catch (Exception e) when (e is DbUpdateException || e is EntityCommandExecutionException)
			{
				ModelState.AddModelError("", "Для запрошенного действия не хватает прав");
				return RedirectToAction("List", "Product");
			}
			catch
			{
				ModelState.AddModelError("", "Ошибка в работе системы");
				return RedirectToAction("List", "Product");
			}
		}
	}
}