﻿using System.Collections.Generic;
using System.Linq;
using StarMap.Models;
using StarMap.Utilities;

namespace StarMap.Controllers
{
    public class CategoryApiController : BaseApiController
    {
        private readonly StarMapEntities _db = new StarMapEntities();

        // GET api/CategoryApi/GetAll
        public List<CategoryModel> GetAll()
        {
            var data = _db.Category.ToList();
            return data.Count > 0 ? data.Select(m => m.ToCategoryModel()).ToList() : new List<CategoryModel>();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }
    }
}