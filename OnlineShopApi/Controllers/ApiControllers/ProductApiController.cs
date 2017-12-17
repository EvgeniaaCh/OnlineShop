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
				UnitOfWork.Context.Database.ExecuteSqlCommand($"do $$ begin perform \"dbo\".add_product_to_basket({user.Id}, {product.Id}); end $$");
				return Ok(
					UnitOfWork
					.Context.Database
					.SqlQuery<int>($"select \"dbo\".get_product_count_from_basket({user.Id}, {product.Id});")
					.FirstOrDefault());
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
				UnitOfWork.Context.Database.ExecuteSqlCommand($"do $$ begin perform dbo.delete_one_product_instance_from_basket({user.Id}, {product.Id}); end $$");
				DeleteOneProductInstanceFromBasketResponse response = new DeleteOneProductInstanceFromBasketResponse
				{
					Count = UnitOfWork.Context.Database.SqlQuery<int>($"select \"dbo\".get_product_count_from_basket({user.Id}, {product.Id});").FirstOrDefault()
				};
				return Ok(response);
			}
			catch(System.Exception ex)
			{
				return BadRequest();
			}
		}
	}
}