﻿using Camino.Shared.Requests.Media;
using System;
using System.Collections.Generic;

namespace Camino.Shared.Requests.Farms
{
    public class FarmModifyRequest
    {
        public FarmModifyRequest()
        {
            Pictures = new List<PictureRequest>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UpdatedById { get; set; }
        public long CreatedById { get; set; }
        public long FarmTypeId { get; set; }
        public string Address { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public IEnumerable<PictureRequest> Pictures { get; set; }
    }
}
