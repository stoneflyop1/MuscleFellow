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
        Task<List<Product>> GetAllAsync();

        Task<List<Product>> GetPopularProductsAsync(int count);

        Task<IEnumerable<Product>> GetProductsAsync(string keyword, int page, int pageSize);

        Task<List<ProductImage>> GetProductImagesAsync(Guid productID);

        Task<Product> GetAsync(Guid id);

        Task<Guid> AddAsync(Product product);
    }

	#pragma warning disable 1591

    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<ProductImage> _imgRepo;

        public ProductService(IRepository<Product> productRepo, IRepository<ProductImage> imgRepo)
        {
            _productRepo = productRepo;
            _imgRepo = imgRepo;
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _productRepo.ToListAsync();
        }

        public async Task<List<Product>> GetPopularProductsAsync(int count)
        {
            var results = await _productRepo.Table
                    .Take(count).Select(p => p)
                    .ToListAsync();

            return results;
        }

        public async Task<List<ProductImage>> GetProductImagesAsync(Guid productID)
        {
            var results = await _imgRepo.Table.Where
                (i => i.ProductID == productID)
                .Select(i => new { ProductImage = i, })
                .ToListAsync();

            return results.Select(i => i.ProductImage).ToList();
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

        public Task<Product> GetAsync(Guid id)
        {
            return _productRepo.Table.Where(p => p.ProductID == id).SingleOrDefaultAsync();
        }

        public async Task<Guid> AddAsync(Product product)
        {
            if (null == product) return Guid.Empty;
            await _productRepo.InsertAsync(product);
            return product.ProductID;
        }
    }
}
