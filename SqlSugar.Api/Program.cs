using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sqlsugar.Infrastructure.Helpers;
namespace SqlSugar.Api
{
    /// <summary>
    /// ���
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // ���������ڽ��������������� Microsoft.Extensions.DependencyInjection.IServiceScope��
            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args).UseServiceProviderFactory(new AutofacServiceProviderFactory())
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      //���÷���·��
                      var ApplicationUrl = Appsettings.app("ApplicationUrl");
                      webBuilder.UseUrls(ApplicationUrl);
                      webBuilder.UseStartup<Startup>();
                  })//���Serilog��־
                .UseSerilog((hostingContext, loggerConfiguration) =>
                //��appsettings.json�ж�ȡ����
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext()
            );
        }
    }
}
