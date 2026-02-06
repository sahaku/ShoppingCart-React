using static System.Net.WebRequestMethods;

namespace ShoppingCart_Api.Api
{
    public class ApiService
    {
        private readonly HttpClient _http;
        public ApiService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient();
        }

        public async Task<HttpResponseMessage> InvokeApi(string url, Object request)
        {
            return await _http.PostAsJsonAsync(url, request);
          
        }
    }
}
