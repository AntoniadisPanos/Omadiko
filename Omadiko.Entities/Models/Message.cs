﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

   


namespace Omadiko.Entities
{
   public class Message
    {
        
        public int MessageId { get; set; }
       
        public string UserName { get; set; }
        
        public string Text { get; set; }
        public DateTime When { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        
    }
}
