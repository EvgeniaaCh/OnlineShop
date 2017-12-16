using System;
using OnlineShop.DAL.Repositories;
using OnlineShop.DAL.Repositories.Implements;

namespace OnlineShop.DAL
{
	public class UnitOfWork : IUnitOfWork
	{
		public Context Context { get; set; } 
		private bool disposed = false;

		public UnitOfWork()
		{
			Context = new Context();
		}

		public UnitOfWork(string connection)
		{
			Context = new Context(connection);
		}

		public virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					Context.Dispose();
				}
				disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public ICategoryRepo CategoryRepo => new CategoryRepo(Context);
		public IOrderRepo OrderRepo => new OrderRepo(Context);
		public IProductRepo ProductRepo => new ProductRepo(Context);
		public IRoleRepo RoleRepo => new RoleRepo(Context);
		public IUserRepo UserRepo => new UserRepo(Context);
		public void SaveChanges() => Context.SaveChanges();
	}
}
