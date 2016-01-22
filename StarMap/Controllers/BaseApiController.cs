using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StarMap.Utilities;

namespace StarMap.Controllers
{
    public class BaseApiController : ApiController
    {
        public int PageSize
        {
            get
            {
                return Convert.ToInt32("PageSizeApi".AppSetting());
            }
        }
        public int RadiusSearch
        {
            get
            {
                return Convert.ToInt32("RadiusSearch".AppSetting());
            }
        }
    }
}