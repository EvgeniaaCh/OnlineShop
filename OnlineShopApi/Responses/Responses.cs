using Newtonsoft.Json;

namespace OnlineShopApi.Responses
{
	public class ResponseBase
	{
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("totalCount")]
		public int TotalCount { get; set; }
	}

	public class AddProductToBasketResponse : ResponseBase {}

	public class DeleteProductFromBasketResponse : ResponseBase {}

	public class DeleteOneProductInstanceFromBasketResponse : ResponseBase {}

}