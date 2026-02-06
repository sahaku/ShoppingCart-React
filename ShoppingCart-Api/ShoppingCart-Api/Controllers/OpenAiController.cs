using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart_Api.Models;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using Microsoft.EntityFrameworkCore;
using ShoppingCart_Api.Api;


namespace ShoppingCart_Api.Controllers
{
    [Route("api/aiservice")]
    [ApiController]
    public class OpenAiController : ControllerBase
    {
        //private readonly HttpClient _http;
        private readonly ShoppingContext _context;
        //private readonly string baseUrl;
        private readonly IConfiguration _configuration;
        private readonly ApiService _apiService;

        public OpenAiController(IHttpClientFactory factory, ShoppingContext context, IConfiguration configuration, ApiService apiService)
        {
            //_http = factory.CreateClient();
            _context = context;
            _configuration = configuration;
            //baseUrl = configuration["EndPointServices:BaseUrl"];
            _apiService = apiService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] AiSearchRequest req)
        {
            var ollamaRequest = new
            {
                model = "llama3.1",
                prompt = $@"
                    You are an AI that converts natural language into product search filters.
                    Return ONLY valid JSON.
                    User query: ""{req.Query}""
                    Respond in this JSON format:
                    {{
                      ""category"": string or null,
                      ""maxPrice"": number or null,
                      ""programmingFriendly"": boolean,
                      ""keywords"": [string]
                    }}"
            };
            var baseUrl = $"{_configuration["EndPointServices:BaseUrl"]}/generate";
            //var response = await _http.PostAsJsonAsync("", ollamaRequest);
            var response = await _apiService.InvokeApi(baseUrl, ollamaRequest);
            var result = await response.Content.ReadAsStringAsync();
           

            // Split NDJSON lines
            var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            string accumulated = "";

            foreach (var line in lines)
            {
                var obj = JsonDocument.Parse(line).RootElement;

                if (obj.TryGetProperty("response", out var chunk))
                {
                    accumulated += chunk.GetString();
                }

                if (obj.TryGetProperty("done", out var doneProp) && doneProp.GetBoolean())
                {
                    break;
                }
            }

            // Now accumulated contains the full JSON string from the model
            AiFilters filters = null;

            try
            {
                filters = JsonSerializer.Deserialize<AiFilters>(accumulated);
            }
            catch
            {
                return BadRequest(new { error = "AI returned invalid JSON", raw = accumulated });
            }

            //var json = JsonDocument.Parse(result);
            //var text = json.RootElement.GetProperty("response").GetString();
            //var filters=JsonSerializer.Deserialize<AiFilters>(text!=null?text:string.Empty);
            //AiFilters filters = null;

            //if (!string.IsNullOrWhiteSpace(accumulated))
            //{
            //    try
            //    {
            //        filters = JsonSerializer.Deserialize<AiFilters>(accumulated);
            //    }
            //    catch
            //    {
            //        return BadRequest(new { error = "AI returned invalid JSON", raw = accumulated });
            //    }
            //}

            //if (filters == null)
            //{
            //    return BadRequest(new { error = "AI returned null or invalid filters", raw = text });
            //}


            var query = _context.Products.AsQueryable();

            if (filters != null)
            {
                if (!String.IsNullOrEmpty(filters?.Category))
                {
                    query = query.Where(x => x.Category == filters.Category);
                }
                if (filters.MaxPrice.HasValue)
                {
                    double maxPrice = (Double)filters.MaxPrice.Value;
                    query = query.Where(x => x.Price.HasValue && x.Price.Value <= maxPrice);
                }
                if (filters.Keywords?.Any() == true)
                {
                    var keywords = filters.Keywords;

                    query = query.Where(p =>
                        keywords.Any(kw =>
                            (p.ProductName != null && p.ProductName.Contains(kw)) ||
                            (p.Description != null && p.Description.Contains(kw))
                        )
                    );
                }

            }

            var products = await query.ToListAsync();
            return Ok(products);
        }
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] AiRequest request)
        {
            var baseUrl = $"{_configuration["EndPointServices:BaseUrl"]}/generate";
            var reply = await _apiService.InvokeApi(baseUrl, request.Prompt);
            return Ok(new { description = reply });
        }

        [HttpPost("generate-description")]
        public async Task<IActionResult> GenerateDescription([FromBody] Product product)
        {
            var prompt = $@"
                Write a creative product description for the following product:
                Name: {product.ProductName}
                Category: {product.Category}
                Features: {product.Description}
                The description should be engaging and highlight the key features of the product.
            ";
            var aiRequest = new AiRequest { Prompt = prompt };
            var baseUrl = $"{_configuration["EndPointServices:BaseUrl"]}/generate";
            var response = await _apiService.InvokeApi(baseUrl, aiRequest);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(result);
            var description = json.RootElement.GetProperty("response").GetString();
            return Ok(new { description = description });
        }

        [HttpPost("smart-search")]
        public async Task<IActionResult> SmartSearch([FromBody] SearchRequest request)
        {
            var prompt = $@"
                Given the user search query: ""{request.Query}"",
                suggest relevant product categories and keywords to improve search results.
                Respond in this JSON format:
                {{
                  ""categories"": [string],
                  ""keywords"": [string]
                }}
            ";
            var aiRequest = new AiRequest { Prompt = prompt };
            var baseUrl = $"{_configuration["EndPointServices:BaseUrl"]}/generate";
            var response = await _apiService.InvokeApi(baseUrl, aiRequest);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(result);
            var text = json.RootElement.GetProperty("response").GetString();
            return Ok(new { suggestions = text });
        }

        [HttpPost("cart-suggestions")]
        public async Task<IActionResult> CartSuggestions([FromBody] CartRequest request)
        {
            var itemsDescription = string.Join(", ", request.Items.Select(i => i.ProductName));
            var prompt = $@"
                The user has the following items in their shopping cart: {itemsDescription}.
                Suggest 3 additional products that complement these items.
                Respond in this JSON format:
                {{
                  ""suggestedProducts"": [string]
                }}
            ";
            var aiRequest = new AiRequest { Prompt = prompt };
            var baseUrl = $"{_configuration["EndPointServices:BaseUrl"]}/generate";
            var response = await _apiService.InvokeApi(baseUrl, aiRequest);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(result);
            var text = json.RootElement.GetProperty("response").GetString();
            return Ok(new { suggestions = text });
        }

    }
    public class AiRequest
    {
        public string Prompt { get; set; }
    }
    public class SearchRequest
    {
        public string Query { get; set; }
    }
    public class CartRequest
    {
        public List<Product> Items { get; set; }
    }
    public class AiSearchRequest
    {
        public string Query { get; set; }
    }
    public class AiFilters
    {
        public string Category { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool ProgrammingFriendly { get; set; }
        public List<string> Keywords { get; set; }
    }
}
