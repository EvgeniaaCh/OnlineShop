using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DAL.Entities
{
	public class Category : BaseEntity
	{
		public Category()
		{
			Products = new HashSet<Product>();
		}
		
		[MaxLength(length: 16, ErrorMessage = "Длина не должна превышать 16 символов")]
		[MinLength(length: 4, ErrorMessage = "Длина не должна быть меньше 4 символов")]
		public string Name { get; set; }

		public virtual ICollection<Product> Products { get; set; }
	}
}
