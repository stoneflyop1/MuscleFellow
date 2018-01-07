# 微软开源跨平台移动开发实践

阅读《微软开源跨平台移动开发实践》的源代码，参考了[李争老师的源码库](https://github.com/micli/MuscleFellow)。其中MuscleFellow.Web中的Assets文件夹内容全部来自于李老师的源码库。

## 使用的技术和框架

- asp.net core 2.0 razor pages (Microsoft.AspNetCore.All)，[MVC与Razor Pages的对比](https://stackify.com/asp-net-razor-pages-vs-mvc/)
- 依赖注入使用autofac (Autofac, Autofac.Extensions.DependencyInjection)适配.net core的依赖注入
- 数据库使用sqlite(Microsoft.EntityFrameworkCore.Sqlite)，ORM使用泛型的Repository模式