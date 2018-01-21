# 微软开源跨平台移动开发实践

阅读《微软开源跨平台移动开发实践》的源代码，参考了[李争老师的源码库](https://github.com/micli/MuscleFellow)。其中MuscleFellow.Web中的Assets文件夹内容全部来自于李老师的源码库。

## 使用的技术和框架

- asp.net core 2.0 razor pages (Microsoft.AspNetCore.All)，[MVC与Razor Pages的对比](https://stackify.com/asp-net-razor-pages-vs-mvc/)，以及[CSRF/XSRF处理](http://www.talkingdotnet.com/handle-ajax-requests-in-asp-net-core-razor-pages/)
- 依赖注入使用autofac (Autofac, Autofac.Extensions.DependencyInjection)适配.net core的依赖注入
- 数据库使用sqlite(Microsoft.EntityFrameworkCore.Sqlite)，ORM使用泛型的Repository模式
- 移动App使用[Xamarin Forms](https://github.com/xamarin/Xamarin.Forms)，其中Android选择了支持Android7.0以上版本，低版本需要重新安装Xamarin.Android的Nuget包

## 如何使用

[使用 `dotnet publish`发布dotnetcore程序](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore2x)

```sh
cd MuscleFellow.Web
dotnet publish -c Release # it'll build and publish to bin/Release/netcoreapp2.0/publish
cd bin/Release/netcoreapp2.0/publish
dotnet MuscleFellow.Web.dll # start the website
```

## 问题

1. 虽然Razor Pages本身已经自动做了[跨站攻击防御](https://docs.microsoft.com/en-us/aspnet/core/mvc/razor-pages/index?tabs=visual-studio#xsrf)，使用Ajax请求时，还是需要添加`__RequestVerificationToken`。