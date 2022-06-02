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
        /// ��������Դ�Ĳ�����
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
           .AddNewtonsoftJson(options => // Newtonsoft.Json ����
           {
               // ��ʽ�� json ʱ��
               options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
               // json ������
               options.SerializerSettings.ContractResolver = new ApiJsonContractResolver();
           });
        
            services.AddHealthChecks();
            // ��������
            services.AddCors(options =>
            {
                // ��������Դ
                options.AddPolicy(name: CorsAllowAll, builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SqlSugar.Api",
                    Version = "v1",
                    Description = @"ʾ�����:ʹ����.NET5 Swagger Serilog DMS  SqlSugar.IOC SqlSugarCore AutoMapper JWT  ��ӭ��������",
                    Contact = new OpenApiContact { Email = "2515855304@qq.com", Name = "���ɳ�", }
                });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
                var xmlPath = Path.Combine(basePath, "SqlSugar.Api.xml");
                var xmlPath_entity = Path.Combine(basePath, "Sqlsugar.Domain.xml");//�ֲ�ʵ����ʾע��
                c.IncludeXmlComments(xmlPath, true);
                c.IncludeXmlComments(xmlPath_entity, true);
                c.DocInclusionPredicate((docName, description) => true);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ���·�����Bearer {token} ���ɣ�ע������֮���пո�",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });
                //��֤��ʽ���˷�ʽΪȫ�����
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
                c.OrderActionsBy(o => o.RelativePath); // ��action�����ƽ�����������ж�����Ϳ��Կ���Ч���ˡ�
            });
            var Secret = Appsettings.app(new string[] { "JWT", "Secret" });
            var Audience = Appsettings.app(new string[] { "JWT", "Audience" });
            var SecurityKey = Appsettings.app(new string[] { "JWT", "SecurityKey" });
            var Issuer = Appsettings.app(new string[] { "JWT", "Issuer" });
            //������Կ
            var symmetricKeyAsBase64 = Secret;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            //���jwt��֤��
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//�Ƿ���֤Issuer
                        ValidateAudience = true,//�Ƿ���֤Audience
                        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                        ClockSkew = TimeSpan.FromSeconds(30),
                        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                        ValidAudience = Audience,//Audience
                        ValidIssuer = Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))//�õ�SecurityKey
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
            // ��������ѡ��Դ���ԣ�����
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
        /// �ӿ�ע��
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
