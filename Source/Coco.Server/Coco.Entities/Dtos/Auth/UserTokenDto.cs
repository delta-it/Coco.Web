﻿namespace Coco.Entities.Dtos.Auth
{
    public class UserTokenDto
    {
        public virtual string LoginProvider { get; set; }
        public virtual string Name { get; set; }
        public virtual long UserId { get; set; }
        public virtual string Value { get; set; }
    }
}