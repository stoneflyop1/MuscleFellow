using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MuscleFellow.Data;
using MuscleFellow.Models.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MuscleFellow.Web.Services
{
    public interface IBrandService
    {
        Task<List<Brand>> GetAllAsync();

        Task<Brand> GetAsync(int brandID);

        Task<IEnumerable<Product>> GetProductsAsync(int brandID, string filter, int pageSize, int pageCount);
    }

    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepo;
        private readonly IRepository<Product> _productRepo;

        public BrandService(IRepository<Brand> brandRepo, IRepository<Product> productRepo)
        {
            _brandRepo = brandRepo;
            _productRepo = productRepo;
        }

        public Task<List<Brand>> GetAllAsync()
        {
            return _brandRepo.ToListAsync();
        }

        public async Task<Brand> GetAsync(int brandID)
        {
            return await _brandRepo.Table.Where(b => b.BrandID == brandID).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int brandID, string filter, int pageSize, int pageCount)
        {
            var results = await _productRepo.Table.Where
                (p => p.BrandID == brandID && (String.IsNullOrEmpty(filter) ||
                p.ProductName.Contains(filter) || p.Description.Contains(filter)))
                .Select(p => new { Product = p, })
                .Skip(pageSize * pageCount)
                .Take(pageSize)
                .ToListAsync();

            return results.Select(p => p.Product);
        }
    }
}
