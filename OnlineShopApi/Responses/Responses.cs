using Newtonsoft.Json;

namespace OnlineShopApi.Responses
{
	public class ResponseBase
	{
		[JsonProperty("count")]
		public int Count { get; set; }
	}

	public class AddProductToBasketResponse : ResponseBase {}

	public class DeleteOneProductInstanceFromBasketResponse : ResponseBase {}

}