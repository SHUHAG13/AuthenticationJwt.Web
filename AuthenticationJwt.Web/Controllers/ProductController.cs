using AuthenticationJwt.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationJwt.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly MyAppDbContext _context;

        public ProductController(MyAppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _context.Prodects.ToList();
                if (products.Count == 0)
                {
                    return NotFound("No products found.");
                }
                return Ok(products);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
