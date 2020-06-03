﻿using Api.Auth.GraphQLTypes.InputTypes;
using Api.Auth.GraphQLTypes.ResultTypes;
using Api.Auth.Resolvers.Contracts;
using Coco.Framework.GraphQLTypes.ResultTypes;
using Coco.Framework.Infrastructure.Middlewares;
using HotChocolate.Types;

namespace Api.Auth.GraphQLTypes
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            //descriptor.Field<IUserResolver>(x => x.SignoutAsync(default))
            //    .Type<CommonResultType>()
            //    .Directive<AuthenticationDirectiveType>()
            //    .Resolver(ctx => ctx.Service<IUserResolver>().SignoutAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.GetLoggedUserAsync(default))
                .Type<LoggedInResultType>()
                .Directive<InitializeSessionDirectiveType>()
                .Resolver(ctx => ctx.Service<IUserResolver>().GetLoggedUserAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.FindFullUserInfoAsync(default))
                .Type<FullUserInfoResultType>()
                .Directive<InitializeSessionDirectiveType>()
                .Argument("criterias", a => a.Type<FindUserInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().FindFullUserInfoAsync(ctx));

            descriptor.Field<IUserResolver>(x => x.ActiveAsync(default))
                .Type<CommonResultType>()
                .Argument("criterias", a => a.Type<ActiveUserInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ActiveAsync(ctx));
        }
    }
}
