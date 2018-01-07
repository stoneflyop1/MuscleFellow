using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuscleFellow.Models.Domain;
using MuscleFellow.Web.Services;

namespace MuscleFellow.Web.Pages.Brands
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Brand Brand { get; set; }

        [BindProperty]
        public IEnumerable<Product> Products { get; set; }

        private readonly IBrandService _brandService;

        public IndexModel(IBrandService brandService)
        {
            _brandService = brandService;
        }

        //public void OnGet(int id)
        //{
        //    if (!HttpContext.Session.Keys.Contains("UserSession"))
        //    {
        //        HttpContext.Session.Set("UserSession",
        //          System.Text.Encoding.UTF8.GetBytes("SessionCreation"));
        //    }
        //    Brand = _brandService.Get(id);
        //    Products = _brandService.GetProducts(id, "", 50, 0);
        //}

        public async Task OnGetAsync(int id)
        {
            if (!HttpContext.Session.Keys.Contains("UserSession"))
            {
                HttpContext.Session.Set("UserSession",
                  System.Text.Encoding.UTF8.GetBytes("SessionCreation"));
            }
            Brand = await _brandService.GetAsync(id);
            Products = await _brandService.GetProductsAsync(id, "", 50, 0);
        }
    }
}