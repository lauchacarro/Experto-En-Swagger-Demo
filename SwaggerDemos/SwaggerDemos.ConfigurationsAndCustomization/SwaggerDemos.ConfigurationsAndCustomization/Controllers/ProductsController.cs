using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SwaggerDemos.ConfigurationsAndCustomization.Entity;

using System;
using System.Collections.Generic;

namespace SwaggerDemos.ConfigurationsAndCustomization.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(ErrorModel))]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>The list of all products</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok();
        }
        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="product">Product to create</param>
        /// <returns>The product created</returns>
        [HttpPost(Name = "CreateProduct")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult Create(Product product)
        {
            return Ok(product);
        }

        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="product">Product to update</param>
        /// <returns>The product updated</returns>
        [HttpPut(Name = "UpdateProduct")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult Update(Product product)
        {
            return Ok(product);
        }

        /// <summary>
        /// Get a product
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <returns>The product</returns>
        [HttpGet("{id}")]

        [ApiExplorerSettings(IgnoreApi = true)]

        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <returns>Emply</returns>
        [HttpDelete("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult Delete(int id)
        {
            return Ok();
        }

        /// <summary>
        /// Delete all products
        /// </summary>
        /// <returns>Emply</returns>
        [HttpDelete(Name = "DeleteAllProducts")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]

        [Obsolete]
        public IActionResult DeleteAll()
        {
            return Ok();
        }
    }
}
