using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Data;
using Contact.API.Dtos;
using Contact.API.Infrastructure;
using Contact.API.Models;
using Contact.API.Service;
using DnsClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience.Http;

namespace Contact.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*<<<<<<< HEAD

                        services.Configure<Settings>(options =>
                        {
                            options.ConnectionString
                                = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                            options.Database
                                = Configuration.GetSection("MongoConnection:Database").Value;
                        });
                        //services.AddScoped<IContactContext, ContactContext>();
                        services.AddScoped<IContactContext>(sp => sp.GetService<ContactContext>());
                        services.AddScoped<IContactRepository, MongoContactRepository>();
                        services.AddScoped<IContactApplyRequestRepository, MongoContactApplyRequestRepository>();
                        services.AddScoped<IUserService, UserService>();
            =======*/

            #region
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "contact_api";
                    options.Authority = "http://localhost";
                    options.SaveToken = true;

                });
            #endregion

            #region consul and  Consul Service Disvovery

            //将配置文件中的内容进行注入,服务发现配置
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                return new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
            });

            #endregion

            #region polly register

            //注册全局单例ResilientHttpClientFactory
            services.AddSingleton(typeof(ResilientHttpClientFactory), sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilienceHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var retryCount = 5;
                var exceptionCountAllowedBeforeBreaking = 5;
                return new ResilientHttpClientFactory(logger, httpContextAccessor, retryCount, exceptionCountAllowedBeforeBreaking);
            });
            //注册全局实例IHttpClient
            services.AddSingleton<IHttpClient>(sp =>
            {
                return sp.GetRequiredService<ResilientHttpClientFactory>().GetResilienceHttpClient();
            });

            #endregion

            #region Mongo config
            services.Configure<Settings>(
                options =>
                {
                    options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                    options.Database = Configuration.GetSection("MongoDb:Database").Value;
                });

            services.AddTransient<IContactContext, ContactContext>();
            services.AddTransient<IContactRepository, MongoContactRepository>();
            services.AddTransient<IContactApplyRequestRepository, MongoContactApplyRequestRepository>();

            #endregion


            services.AddTransient<IUserService, UserService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
