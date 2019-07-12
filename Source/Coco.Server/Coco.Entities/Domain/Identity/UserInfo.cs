﻿using Coco.Entities.Base;
using Coco.Entities.Domain.Dbo;
using System;
using System.ComponentModel.DataAnnotations;

namespace Coco.Entities.Domain.Identity
{
    public class UserInfo : BaseEntity
    {
        public long Id { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        public byte? GenderId { get; set; }
        public short? CountryId { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
        public virtual User User { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Country Country { get; set; }
    }
}
