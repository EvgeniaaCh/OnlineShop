using OnlineShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OnlineShop.DAL
{
	public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : BaseEntity
	{
		private DbContext _context;

		public GenericRepo(DbContext context)
		{
			_context = context;
		}

		public IEnumerable<TEntity> Get() => _context.Set<TEntity>();


		public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
			=> _context.Set<TEntity>().Where(predicate).ToList();


		public IEnumerable<TEntity> Get(int page) => _context.Set<TEntity>().OrderBy(t => t.Id).Skip((page - 1) * 10).Take(10);


		public TEntity FindById(int id) => _context.Set<TEntity>().FirstOrDefault(t => t.Id == id);

		public void Create(TEntity item)
		{
			_context.Set<TEntity>().Add(item);
			_context.SaveChanges();
		}

		public void Remove(TEntity item)
		{
			_context.Set<TEntity>().Remove(item);
			_context.SaveChanges();
		}

		public int Count() => _context.Set<TEntity>().Count();
	}
}
