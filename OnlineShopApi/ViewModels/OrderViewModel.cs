using System;
using System.Collections.Generic;

namespace OnlineShopApi.ViewModels
{
	public class GetOrderListViewModel
	{
		public int Id { get; set; }

		public float Price { get; set; }

		public int Count { get; set; }

		public DateTimeOffset CreateDt { get; set; }
	}

	public class GetOrderDetailViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public float Price { get; set; }

		public int Count { get; set; }
	}
}