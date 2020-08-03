﻿using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Web.AuthorizationManagement.Models
{
    public class AuthorizationPolicyRolesViewModel : BaseViewModel
    {
        public AuthorizationPolicyRolesViewModel()
        {
            AuthorizationPolicyRoles = new List<RoleViewModel>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte RoleId { get; set; }

        public IEnumerable<RoleViewModel> AuthorizationPolicyRoles { get; set; }
    }
}
