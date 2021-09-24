using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SwaggerDemos.Auth.Entity;

namespace SwaggerDemos.Auth.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Product product)
        {
            return Ok(product);
        }


        [HttpPut]
        [Authorize]
        public IActionResult Update(Product product)
        {
            return Ok(product);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteAll()
        {
            return NoContent();
        }
    }
}
