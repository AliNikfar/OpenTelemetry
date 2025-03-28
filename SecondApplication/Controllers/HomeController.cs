using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace SecondApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;

        public HomeController(IHttpClientFactory client)
        {
            _client = client.CreateClient("ApiFirst");
        }

        [HttpGet("ApiFirst")]
        public async Task<IActionResult> GetAll()
        {
            var result =await _client.GetStringAsync("/GetAll");
            return Ok(result);
        }
    }
}
