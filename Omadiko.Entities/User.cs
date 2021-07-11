﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Omadiko.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HomeAddress { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public Country Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CreditCard { get; set; }
        public string Password { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingPostalCode { get; set; }
        public Country ShippingCountry { get; set; }
    }
}