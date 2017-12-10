using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MuscleFellow.Data;
using MuscleFellow.Models.Domain;

namespace MuscleFellow.Web.Services
{
    public interface IBrandService
    {
        Task<List<Brand>> GetAllAsync();
    }

    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepo;

        public BrandService(IRepository<Brand> brandRepo)
        {
            _brandRepo = brandRepo;
        }

        public Task<List<Brand>> GetAllAsync()
        {
            return _brandRepo.ToListAsync();
        }
    }
}
