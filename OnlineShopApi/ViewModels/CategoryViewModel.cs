namespace OnlineShopApi.ViewModels.Categories
{
	public class GetCategoryListViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}

	public class CreateCategoryViewModel
	{
		public string Name { get; set; }
	}

	public class EditCategoryViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}