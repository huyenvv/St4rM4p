using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarMap.Models
{
    public class ImportErrorModel
    {
        public int Line { get; set; }
        public string Message { get; set; }
    }
}