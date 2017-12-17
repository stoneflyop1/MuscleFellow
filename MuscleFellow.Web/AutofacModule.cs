using Autofac;
using Microsoft.EntityFrameworkCore;
using MuscleFellow.Data;
using MuscleFellow.Models;
using MuscleFellow.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuscleFellow.Web
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //http://www.cnblogs.com/JacZhu/p/5869969.html
            builder.Register(c => c.Resolve<MuscleFellowDbContext>()).As<IDbContext>().InstancePerLifetimeScope();
            //builder.RegisterType<MuscleFellowDbContext>().As<IDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<BrandService>().As<IBrandService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<CartItemService>().As<ICartItemService>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            //base.Load(builder);
        }
    }
}
