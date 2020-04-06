﻿using Coco.Business;
using Coco.Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Coco.Api.Framework.MappingProfiles;
using Coco.Business.MappingProfiles;
using Coco.Api.Framework.Infrastructure;
using Coco.Api.Framework.Infrastructure.Extensions;
using Api.Identity.Infrastructure.Extensions;
using HotChocolate.AspNetCore;
using Api.Identity.Resolvers.Contracts;
using Api.Identity.Resolvers;

namespace Api.Identity
{
    public class Startup
    {
        private IBootstrapper _bootstrapper;
        readonly string MyAllowSpecificOrigins = "AllowOrigin";
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _bootstrapper = new BusinessStartup(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Config AddCors
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(
                        "http://localhost:3000",
                        "http://localhost:7000",
                        "http://localhost:5000", 
                        "http://localhost:45678")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            InvokeInitialStartup(services);
            services.AddControllers()
                .AddNewtonsoftJson();
        }

        private void InvokeInitialStartup(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FrameworkMappingProfile), typeof(UserMappingProfile));
            FrameworkStartup.AddCustomStores(services);
            _bootstrapper.RegiserTypes(services);

            services.AddTransient<IUserResolver, UserResolver>();

            #region GraphQL DI
            services.AddGraphQlDependency();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Config UseCors
            app.UseHttpsRedirection()
                .UseRouting()
                .UseCors(MyAllowSpecificOrigins)
                .UseWebSockets()
                .UseGraphQL("/api/graphql")
                .UseBasicApiMiddleware();
        }
    }
}
