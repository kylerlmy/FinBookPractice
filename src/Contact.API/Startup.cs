using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Data;
using Contact.API.Dtos;
using Contact.API.Infrastructure;
using Contact.API.IntegrationEvents.EventHanding;
using Contact.API.Models;
using Contact.API.Service;
using DnsClient;
using DotNetCore.CAP;
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
                    //options.SaveToken = true;

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

            services.AddHttpContextAccessor();//不添加，以下代码会抛出异常
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

            services.AddScoped<IContactContext, ContactContext>();
            services.AddScoped<IContactRepository, MongoContactRepository>();
            services.AddScoped<IContactApplyRequestRepository, MongoContactApplyRequestRepository>();

            services.Configure<Settings>(
                options =>
                {
                    options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                    options.Database = Configuration.GetSection("MongoDb:Database").Value;
                });


            #endregion


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserProfileChangedEventHandler>();



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCap(x =>
            {

                x.UseMySql("server=192.168.1.110;port=3306;database=beta_contact;userid=kyle;password=Netkyle");
                x.UseRabbitMQ("192.168.1.110");
                x.UseDashboard();

                //Register to Consul
                x.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 5801;
                    d.NodeId = 2;
                    d.NodeName = "CAP No.2 Node";
                });



            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
