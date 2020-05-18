﻿using Coco.Entities.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Coco.Entities.Domain.Auth
{
    public class Role
    {
        public Role()
        {
            this.UserRoles = new HashSet<UserRole>();
        }

        public byte Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public long CreatedById { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }
        [Required]
        public long UpdatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleAuthorizationPolicy> RoleAuthorizationPolicies { get; set; }
    }
}
