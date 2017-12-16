using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.DAL.Entities
{
	public class User : BaseEntity
	{
		public User()
		{
			Products = new HashSet<Product>();
			Orders = new HashSet<Order>();
		}

		public string Guid { get; set; }

		[Index(name: "email", IsUnique = true)]
		[EmailAddress]
		public string Email { get; set; }
		
		public string Password { get; set;}

		[MaxLength(length: 16, ErrorMessage = "Длина не должна превышать 16 символов")]
		[MinLength(length: 2, ErrorMessage = "Длина не должна быть меньше 2 символов")]
		public string FirstName { get; set; }

		[MaxLength(length: 16, ErrorMessage = "Длина не должна превышать 16 символов")]
		[MinLength(length: 2, ErrorMessage = "Длина не должна быть меньше 2 символов")]
		public string LastName { get; set; }

		public DateTime? CreateDt { get; set; }

		public DateTime? BirthDt { get; set; }

		public bool Gender { get; set; }

		public int RoleId { get; set; }

		[ForeignKey("RoleId")]
		public virtual Role Role { get; set; }

		public virtual ICollection<Order> Orders { get; set; }

		public virtual ICollection<Product> Products { get; set; }
	}
}
