﻿using Microsoft.AspNetCore.Builder;

namespace Module.Web.FarmManagement.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
