using FirstApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly PersonRepo _repo;

        public HomeController(PersonRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result= await _repo.GetAll();
            return Ok(result);
        }
    }
}
