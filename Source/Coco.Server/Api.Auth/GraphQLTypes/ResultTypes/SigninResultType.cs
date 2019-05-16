﻿using Api.Auth.Models;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class SigninResultType: ObjectGraphType<SigninResultModel>
    {
        public SigninResultType()
        {
            Field(x => x.Token, type: typeof(BooleanGraphType));
        }
    }
}