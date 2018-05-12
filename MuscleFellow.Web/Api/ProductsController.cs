using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuscleFellow.Models.Domain;
using MuscleFellow.Web.Services;

namespace MuscleFellow.Web.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// Get the specified products.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="keyword">Keyword.</param>
        /// <param name="page">Page.</param>
        /// <param name="pagesize">Pagesize.</param>
        [AllowAnonymous]
        [HttpGet]
		public async Task<IEnumerable<Product>> Get([FromQuery] string keyword, [FromQuery] int page, [FromQuery] int pagesize)
        {
            if (0 == pagesize)
            {
                pagesize = 10;
            }

            IEnumerable<Product> products = await _productService.GetProductsAsync(keyword, page, pagesize);
            return products;
        }
    }
}