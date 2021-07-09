using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TokenGenerator.Models
{
    public class CustomerCard
    {
        [Key]

        public int CardID { get; set; }
        public int CustomerID { get; set; }
        public long CardNumber { get; set; }
        public int CVV { get; set; }
     
        public DateTime RegistationDate { get; set; }
       
        public long Token { get; set; }
    }
}
