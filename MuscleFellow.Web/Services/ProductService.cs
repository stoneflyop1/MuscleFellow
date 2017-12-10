using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MuscleFellow.Data;
using MuscleFellow.Models.Domain;

namespace MuscleFellow.Web.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetPopularProductsAsync(int count);
    }

    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepo;

        public async Task<List<Product>> GetPopularProductsAsync(int count)
        {
            var results = await _productRepo.Table
                    .Take(count).Select(p => p)
                    .ToListAsync();

            return results;
        }
    }
}
