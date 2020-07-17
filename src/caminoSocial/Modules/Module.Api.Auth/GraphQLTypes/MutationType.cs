﻿using Module.Api.Auth.GraphQLTypes.InputTypes;
using Module.Api.Auth.GraphQLTypes.ResultTypes;
using Module.Api.Auth.Resolvers.Contracts;
using Camino.Framework.GraphQLTypes.DirectiveTypes;
using Camino.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace  Module.Api.Auth.GraphQLTypes
{

    public class MutationType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IUserResolver>(x => x.UpdateUserInfoItemAsync(default))
                .Type<ItemUpdatedResultType>()
                .Argument("criterias", a => a.Type<UpdatePerItemInputType>())
                .Directive<AuthenticationDirectiveType>();

            descriptor.Field<IUserResolver>(x => x.UpdateIdentifierAsync(default))
                .Type<UserIdentifierUpdateResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<UserIdentifierUpdateInputType>());

            descriptor.Field<IUserResolver>(x => x.UpdatePasswordAsync(default))
                .Type<UserTokenResultType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<NonNullType<UserPasswordUpdateInputType>>());

            // Public mutation
            descriptor.Field<IUserResolver>(x => x.SignupAsync(default))
                .Type<CommonResultType>()
                .Argument("criterias", a => a.Type<SignupInputType>());

            descriptor.Field<IUserResolver>(x => x.SigninAsync(default))
                .Type<UserTokenResultType>()
                .Argument("criterias", a => a.Type<SigninInputType>());

            descriptor.Field<IUserResolver>(x => x.ForgotPasswordAsync(default))
                .Type<CommonResultType>()
                .Argument("criterias", a => a.Type<NonNullType<ForgotPasswordInputType>>());

            descriptor.Field<IUserResolver>(x => x.ResetPasswordAsync(default))
                .Type<CommonResultType>()
                .Argument("criterias", a => a.Type<NonNullType<ResetPasswordInputType>>());
        }
    }
}