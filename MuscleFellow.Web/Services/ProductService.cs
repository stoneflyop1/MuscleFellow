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

        Task<IEnumerable<Product>> GetProductsAsync(string keyword, int page, int pageSize);
    }

    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepo;

        public ProductService(IRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<List<Product>> GetPopularProductsAsync(int count)
        {
            var results = await _productRepo.Table
                    .Take(count).Select(p => p)
                    .ToListAsync();

            return results;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string keyword, int page, int pageSize)
        {
            var results = await _productRepo.Table.Where
                    (p => (String.IsNullOrEmpty(keyword) ||
                     p.ProductName.Contains(keyword) || p.Description.Contains(keyword)))
                    .Select(p => new { Product = p, })
                    .Skip(pageSize * page)
                    .Take(pageSize)
                    .ToListAsync();

            return results.Select(p => p.Product);
        }
    }
}
