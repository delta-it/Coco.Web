﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Navigation.GraphQL.Queries;
using Module.Api.Navigation.GraphQL.Resolvers;
using Module.Api.Navigation.GraphQL.Resolvers.Contracts;

namespace Module.Api.Navigation.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddTransient<IShortcutResolver, ShortcutResolver>();

            services.AddGraphQLServer()
                .AddType<ShortcutQueries>();
            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureGraphQlServices();
            return services;
        }
    }
}