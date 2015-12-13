using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StarMap.Controllers
{
    public class BaseApiController : ApiController
    {
        public int PageSize = 20;
        public int DistanceAround = 3;
    }
}