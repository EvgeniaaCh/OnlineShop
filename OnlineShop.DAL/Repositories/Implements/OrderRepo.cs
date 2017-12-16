using System.Data.Entity;
using OnlineShop.DAL.Entities;

namespace OnlineShop.DAL.Repositories.Implements
{
	internal class OrderRepo : GenericRepo<Order>, IOrderRepo
	{
		public OrderRepo(DbContext context) : base(context)
		{
		}
	}
}
