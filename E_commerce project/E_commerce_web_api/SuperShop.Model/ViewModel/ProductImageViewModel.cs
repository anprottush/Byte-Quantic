﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.ViewModel
{
    public class ProductImageViewModel
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public string image_url { get; set; }
        public string description { get; set; }
    }
}
