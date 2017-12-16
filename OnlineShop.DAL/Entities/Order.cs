using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.DAL.Entities
{
	public class Order : BaseEntity
	{
		public Order()
		{
			Products = new HashSet<Product>();
		}

		public DateTimeOffset CreateDt { get; set; }

		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public virtual ICollection<Product> Products { get; set; }
	}
}
