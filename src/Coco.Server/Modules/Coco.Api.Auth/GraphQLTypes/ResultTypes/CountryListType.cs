﻿using Coco.Api.Auth.Resolvers.Contracts;
using HotChocolate.Types;

namespace  Coco.Api.Auth.GraphQLTypes.ResultTypes
{
    public class CountryListType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("CountryList");
            descriptor
                .Field("countrySelections")
                .Type<CountryResultType>()
                .Resolver(ctx => ctx.Service<ICountryResolver>().GetAll());
        }
    }
}