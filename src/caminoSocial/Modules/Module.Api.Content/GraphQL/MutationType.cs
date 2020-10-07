﻿using Module.Api.Content.GraphQL.InputTypes;
using Module.Api.Content.GraphQL.Resolvers.Contracts;
using Camino.Framework.GraphQL.DirectiveTypes;
using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;
using Camino.Core.Modular.Contracts;
using Module.Api.Content.GraphQL.ResultTypes;
using Camino.Framework.GraphQL.InputTypes;

namespace Module.Api.Content.GraphQL
{
    public class MutationType : BaseMutationType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IArticleResolver>(x => x.CreateArticleAsync(default))
               .Type<ArticleResultType>()
               .Directive<AuthenticationDirectiveType>()
               .Argument("criterias", a => a.Type<ArticleInputType>());

            descriptor.Field<IArticleCategoryResolver>(x => x.GetArticleCategories(default))
                .Type<ListType<SelectOptionType>>()
                .Argument("criterias", a => a.Type<SelectFilterInputType>());
        }
    }
}
