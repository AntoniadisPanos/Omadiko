﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Omadiko.Entities
{
    public class Cart
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }



        public bool Add { get; set; }
        public bool Remove { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Purchase { get; set; }

    }
}