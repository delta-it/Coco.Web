﻿using Coco.Framework.SessionManager.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Coco.Framework.Attributes
{
    public class AuthenticationUserAttribute : TypeFilterAttribute
    {
        #region Properties
        public bool IgnoreFilter { get; }
        #endregion

        public AuthenticationUserAttribute(bool isIgnore = false) : base(typeof(AuthenticationFilter))
        {
            this.IgnoreFilter = isIgnore;
            this.Arguments = new object[] { isIgnore };
        }

        public class AuthenticationFilter : IAuthorizationFilter
        {
            #region Fields

            private readonly bool _ignoreFilter;
            private readonly ISessionContext _sessionContext;

            #endregion

            public AuthenticationFilter(ISessionContext sessionContext, bool ignoreFilter = true)
            {
                this._ignoreFilter = ignoreFilter;
                _sessionContext = sessionContext;
            }

            public void OnAuthorization(AuthorizationFilterContext filterContext)
            {
                if (filterContext == null)
                {
                    throw new ArgumentNullException(nameof(filterContext));
                }

                var actionDescriptor = filterContext.ActionDescriptor;
                var filterDescriptors = actionDescriptor.FilterDescriptors;

                //check whether this filter has been overridden for the action
                var actionFilter = filterDescriptors
                    .FirstOrDefault(x => x.Scope == FilterScope.Action)
                    .Filter;

                var authenticationFilter = actionFilter as AuthenticationUserAttribute;
                if (authenticationFilter != null && authenticationFilter.IgnoreFilter && _ignoreFilter)
                {
                    return;
                }

                if(!filterContext.Filters.Any(filter => filter is AuthenticationFilter))
                {
                    return;
                }

                //there is AuthorizeLoggedUserFilter, so check access
                if (_sessionContext.CurrentUser == null)
                {
                    filterContext.Result = new ForbidResult();
                }
            }
        }
    }
}
