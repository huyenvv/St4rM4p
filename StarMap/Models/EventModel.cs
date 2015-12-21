﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarMap.Models
{
    public class EventModel : GeneralModel
    {
        public bool? IsHot { get; set; }

        public DateTime? PublicDate { get; set; }
    }
}