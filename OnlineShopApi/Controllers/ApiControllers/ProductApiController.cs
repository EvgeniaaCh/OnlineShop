using OnlineShop.DAL.Entities;
using OnlineShop.DAL.Repositories;
using OnlineShopApi.Requests;
using OnlineShopApi.Responses;
using System.Linq;
using System.Web.Http;

namespace OnlineShopApi.Controllers.ApiControllers
{
	[RoutePrefix("product")]
	[Authorize]
	public class ProductApiController : BaseApiController
	{

		[HttpPost]
		[Route("add_product_to_basket")]
		[Authorize]
		public IHttpActionResult AddProductToBasket([FromBody]AddProductToBasketRequest request)
		{
			User user = GetCurrentUser();
			if (user == null) return Unauthorized();
			IProductRepo repo = UnitOfWork.ProductRepo;
			Product product = repo.FindById(request.Id);
			try
			{
				UnitOfWork.Context.Database.ExecuteSqlCommand($"do $$ begin perform add_product_to_basket({user.Id}, {product.Id}); end $$");
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost]
		[Route("delete_product_from_basket")]
		public IHttpActionResult DeleteProductFromBasket([FromBody]DeleteProductFromBasketRequest request)
		{
			User user = GetCurrentUser();
			if (user == null) return Unauthorized();
			Product basketProduct = user.Products.FirstOrDefault(p => p.Id == request.Id);
			if (basketProduct == null) return BadRequest();
			try
			{
				user.Products.Remove(basketProduct);
				UnitOfWork.SaveChanges();
				DeleteProductFromBasketResponse response = new DeleteProductFromBasketResponse
				{
					Count = 0,
					TotalCount = user.Products.Count
				};
				return Ok(response);
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost]
		[Route("delete_one_product_instance_from_basket")]
		public IHttpActionResult DeleteOneProductInstanceFromBasket([FromBody]DeleteOneProductInstanceFromBasketRequest request)
		{
			User user = GetCurrentUser();
			if (user == null) return Unauthorized();
			Product product = user.Products.FirstOrDefault(p => p.Id == request.Id);
			if (product == null) return BadRequest();
			try
			{
				user.Products.Remove(product);
				UnitOfWork.SaveChanges();
				DeleteOneProductInstanceFromBasketResponse response = new DeleteOneProductInstanceFromBasketResponse
				{
					Count = user.Products.Count(p => p.Id == request.Id),
					TotalCount = user.Products.Count
				};
				return Ok(response);
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost]
		[Route("clear_basket")]
		public IHttpActionResult ClearBasket()
		{
			User user = GetCurrentUser();
			if (user == null) return Unauthorized();
			try
			{
				user.Products.Clear();
				UnitOfWork.SaveChanges();
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}