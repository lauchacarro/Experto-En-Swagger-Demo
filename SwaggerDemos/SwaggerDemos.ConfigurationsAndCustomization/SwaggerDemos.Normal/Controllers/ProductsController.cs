using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SwaggerDemos.ConfigurationsAndCustomization.Entity;

using System.Collections.Generic;

namespace SwaggerDemos.Normal.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Create(Product routeEntity)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(Product routeEntity)
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }

        [HttpDelete]

        public IActionResult DeleteAll()
        {
            return Ok();
        }
    }
}
