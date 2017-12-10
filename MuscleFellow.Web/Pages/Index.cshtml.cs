using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuscleFellow.Models.Domain;
using MuscleFellow.Web.Services;

namespace MuscleFellow.Web.Pages
{
    public class HomePageViewModel : PageModel
    {
        private readonly IBrandService _brandService;
        private readonly IProductService _productService;

        private readonly int _maxProductCount = 20;

        public HomePageViewModel(IBrandService brandService, IProductService productService)
        {
            _brandService = brandService;
            _productService = productService;
        }

        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }

        public async Task OnGetAsync()
        {
            if (!HttpContext.Session.Keys.Contains("UserSession"))
            {
                HttpContext.Session.Set("UserSession",
                  System.Text.Encoding.UTF8.GetBytes("SessionCreation"));
            }
            Brands = await _brandService.GetAllAsync();
            Products = (await _productService.GetPopularProductsAsync(_maxProductCount));
        }
    }
}
