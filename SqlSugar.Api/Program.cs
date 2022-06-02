using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sqlsugar.Infrastructure.Helpers;
namespace SqlSugar.Api
{
    /// <summary>
    /// 入口
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // 创建可用于解析作用域服务的新 Microsoft.Extensions.DependencyInjection.IServiceScope。
            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args).UseServiceProviderFactory(new AutofacServiceProviderFactory())
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      //设置访问路径
                      var ApplicationUrl = Appsettings.app("ApplicationUrl");
                      webBuilder.UseUrls(ApplicationUrl);
                      webBuilder.UseStartup<Startup>();
                  })//添加Serilog日志
                .UseSerilog((hostingContext, loggerConfiguration) =>
                //从appsettings.json中读取配置
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext()
            );
        }
    }
}
