﻿namespace Coco.Entities.Model.Account
{
    public class UserFullModel : UserModel
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string GenderLabel { get; set; }
    }
}