﻿using Coco.Management.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Coco.Management.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureManagementAppBuilder(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        public static IApplicationBuilder CheckDatabaseInstalled(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CheckDatabaseInstalledMiddleware>();
        }
    }
}
