using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenValidator.Models;

namespace TokenValidator.Data
{
    public class Util
    {
        public static bool ValidateToken(CustomerData cardRequest, CustomerData cardDB)
        {
            if ((Math.Abs((cardDB.RegistationDate - DateTime.Now).TotalMinutes) > 30) || (cardRequest.CustomerID != cardDB.CustomerID) || (cardDB.Token == cardRequest.Token))
                return false;
            return true;
        }
    }
}
