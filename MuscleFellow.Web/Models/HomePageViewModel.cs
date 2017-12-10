using System;
using System.Collections.Generic;
using MuscleFellow.Models.Domain;

namespace MuscleFellow.Web.Models
{
    public class HomePageViewModel
    {
        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}
