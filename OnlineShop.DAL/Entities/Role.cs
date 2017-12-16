using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DAL.Entities
{
	public class Role : BaseEntity
	{
		public Role()
		{
			Users = new HashSet<User>();
		}

		[MaxLength(length: 16, ErrorMessage = "Длина не должна превышать 16 символов")]
		[MinLength(length: 4, ErrorMessage = "Длина не должна быть меньше 4 символов")]
		public string Name { get; set; }

		public string Connection { get; set; }

		public virtual ICollection<User> Users { get; set; }
	}
}
