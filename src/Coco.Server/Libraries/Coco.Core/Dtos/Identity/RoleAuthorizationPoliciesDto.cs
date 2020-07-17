﻿using System.Collections.Generic;

namespace Coco.Core.Dtos.Identity
{
    public class RoleAuthorizationPoliciesDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<AuthorizationPolicyDto> AuthorizationPolicies { get; set; }
    }
}