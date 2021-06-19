﻿using Camino.Framework.Models;
using Camino.Shared.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.FarmManagement.Models
{
    public class FarmTypeModel : BaseModel
    {
        public FarmTypeModel()
        {
            SelectFarmTypes = new List<SelectListItem>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public FarmTypeStatus StatusId { get; set; }
        public IEnumerable<SelectListItem> SelectFarmTypes { get; set; }
    }
}
