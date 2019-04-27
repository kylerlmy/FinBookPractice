using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User.API.Data;
using User.API.Dtos;
using User.API.Filters;


namespace User.API
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
            services.AddDbContext<UserContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });

            #region --服务发现--
            //将配置文件中的内容进行注入
            services.Configure<Dtos.ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));

            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;

                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));

            #endregion --服务发现--


            #region  --身份认证--
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "user_api";
                    options.Authority = "http://localhost";

                });

            #endregion -- 身份认证--


            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            })
           .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            try
            {
                services.AddCap(options =>
                {

                    options.UseEntityFramework<UserContext>()
                      .UseRabbitMQ("192.168.1.110")
                      .UseDashboard();

                    //Register to Consul
                    options.UseDiscovery(d =>
                        {
                            d.DiscoveryServerHostName = "localhost";
                            d.DiscoveryServerPort = 8500;
                            d.CurrentNodeHostName = "localhost";
                            d.CurrentNodePort = 5800;
                            d.NodeId = 1;
                            d.NodeName = "CAP No.1 Node";
                        });
                });

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime applicationLifetime,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region --服务发现注册--
            //启动的时候注册服务
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                RegisterService(app, serviceOptions, consul);
            });

            //停止的时候移除服务
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                DeRegisterService(app, serviceOptions, consul);
            });

            #endregion --服务发现注册--

            // app.UseCap();//2.3版本之后不再需要注册
            //身份认证管道配置
            app.UseAuthentication();

            app.UseMvc();

            //执行数据库迁移
            UserContextSeed.SeedAsync(app, loggerFactory).Wait();
        }

        #region --服务发现注册--
        private void RegisterService(IApplicationBuilder app,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            //http://michaco.net/blog/ServiceDiscoveryAndHealthChecksInAspNetCoreWithConsul

            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";
                //健康检查
                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(address, "HealthCheck").OriginalString
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = address.Port
                };

                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            }
        }

        private void DeRegisterService(IApplicationBuilder app,
         IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            //http://michaco.net/blog/ServiceDiscoveryAndHealthChecksInAspNetCoreWithConsul

            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";
                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            }

        }

        #endregion --服务发现注册--

    }
}
