using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuscleFellow.Models.Domain;
using MuscleFellow.Web.Services;

namespace MuscleFellow.Web.Api
{
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string keyword, [FromQuery] int page, [FromQuery] int pagesize)
        {
            if (0 == pagesize)
            {
                pagesize = 10;
                //return BadRequest();
            }

            IEnumerable<Product> products = await _productService.GetProductsAsync(keyword, page, pagesize);
            //foreach (Product p in products)
            //    p.ThumbnailImage = _settings.HostName + p.ThumbnailImage;
            JsonResult result = new JsonResult(products);
            return result;
        }
    }
}