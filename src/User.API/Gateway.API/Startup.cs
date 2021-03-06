﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using IdentityServer4.AccessTokenValidation;
namespace Gateway.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationProviderKey = "finbook";
            //配置对网关的请求交由Identity处理请求的认证与授权
            services.AddAuthentication()
            .AddIdentityServerAuthentication(authenticationProviderKey, options =>
             {
                 options.Authority = "http://localhost:51740";
                 options.ApiName = "gateway_api";
                 options.SupportedTokens = SupportedTokens.Both;
                 options.ApiSecret = "secret";
                 options.RequireHttpsMetadata = false;//设置使用http；默认为true，即强制使用https
             });
            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOcelot();
        }
    }
}
