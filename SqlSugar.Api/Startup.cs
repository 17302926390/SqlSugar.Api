using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SqlSugar.Api.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using Sqlsugar.Infrastructure.Seed;
using Autofac;
using DMS.Extensions.ServiceExtensions;
using Sqlsugar.Infrastructure.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SqlSugar.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 允许所有源的策略名
        /// </summary>
        public const string CorsAllowAll = "CorsAllowAll";
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {             
                options.Conventions.Add(new RouteTokenTransformerConvention(
                         new SlugifyParameterTransformer()));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
           .AddNewtonsoftJson(options => // Newtonsoft.Json 配置
           {
               // 格式化 json 时间
               options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
               // json 解析器
               options.SerializerSettings.ContractResolver = new ApiJsonContractResolver();
           });
        
            services.AddHealthChecks();
            // 跨域配置
            services.AddCors(options =>
            {
                // 允许所有源
                options.AddPolicy(name: CorsAllowAll, builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SqlSugar.Api",
                    Version = "v1",
                    Description = @"示例框架:使用了.NET5 Swagger Serilog DMS  SqlSugar.IOC SqlSugarCore AutoMapper JWT  欢迎大家提意见",
                    Contact = new OpenApiContact { Email = "2515855304@qq.com", Name = "王成成", }
                });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "SqlSugar.Api.xml");
                var xmlPath_entity = Path.Combine(basePath, "Sqlsugar.Domain.xml");//分层实体显示注释
                c.IncludeXmlComments(xmlPath, true);
                c.IncludeXmlComments(xmlPath_entity, true);
                c.DocInclusionPredicate((docName, description) => true);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                //认证方式，此方式为全局添加
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference()
                    {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                    }
                    }, Array.Empty<string>() }
                    });
                c.OrderActionsBy(o => o.RelativePath); // 对action的名称进行排序，如果有多个，就可以看见效果了。
            });
            var Secret = Appsettings.app(new string[] { "JWT", "Secret" });
            var Audience = Appsettings.app(new string[] { "JWT", "Audience" });
            var SecurityKey = Appsettings.app(new string[] { "JWT", "SecurityKey" });
            var Issuer = Appsettings.app(new string[] { "JWT", "Issuer" });
            //生成密钥
            var symmetricKeyAsBase64 = Secret;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            //添加jwt验证：
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ClockSkew = TimeSpan.FromSeconds(30),
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = Audience,//Audience
                        ValidIssuer = Issuer,//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))//拿到SecurityKey
                    };
                });
            services.AddScoped<DbContext>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="app"></param>
       /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SqlSugar.Api v1"));
            }
            // 根据配置选择源策略（跨域）
            app.UseCors(CorsAllowAll);
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCustomExceptionMiddleware();
            app.UseHealthChecks("/health");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        /// <summary>
        /// 接口注入
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister(AppContext.BaseDirectory, new List<string>()
            {
                "SqlSugar.Service.dll","Sqlsugar.Repositories.dll","Sqlsugar.Infrastructure.dll"
            }));
        }
    }
}
