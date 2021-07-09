using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TokenValidator.Models
{
    public class CustomerData
    {
        [Key]
        public int CardID { get; set; }
        public int CustomerID { get; set; }

        public long Token { get; set; }
        public int CVV { get; set; }
        internal DateTime RegistationDate { get; set; }
        internal long CardNumber { get; set; }
    }
}
