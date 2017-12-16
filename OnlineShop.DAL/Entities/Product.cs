using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.DAL.Entities
{
	public class Product : BaseEntity
	{
		public Product()
		{
			Orders = new HashSet<Order>();
			Users = new HashSet<User>();
		}

		[MaxLength(length: 16, ErrorMessage = "Длина не должна превышать 16 символов")]
		[MinLength(length: 4, ErrorMessage = "Длина не должна быть меньше 4 символов")]
		public string Name { get; set; }

		[MaxLength(length: 128, ErrorMessage = "Длина не должна превышать 128 символов")]
		public string Description { get; set; }
		
		public int Price { get; set; }

		public int CategoryId { get; set; }

		[ForeignKey("CategoryId")]
		public virtual Category Category { get; set; }

		public virtual ICollection<Order> Orders { get; set; }

		public virtual ICollection<User> Users { get; set; }
	}
}
