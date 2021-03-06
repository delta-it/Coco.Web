using Camino.ApiHost.Infrastructure.Extensions;
using Camino.Framework.Infrastructure.Extensions;
using Camino.Infrastructure.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Camino.ApiHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureApiHostServices(Configuration);
            var mvcBuilder = services.AddControllers().AddNewtonsoftJson();
            mvcBuilder.AddModular();
            services.AddAutoMappingModular();
            services.AddGraphQlModular();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureAppBuilder(env);
        }
    }
}
