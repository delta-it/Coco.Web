﻿using System;

namespace Camino.Shared.Requests.Authorization
{
    public class AuthorizationPolicyRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
    }
}
