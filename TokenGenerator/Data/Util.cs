using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenGenerator.Data
{
    public static class Util
    {
        public static long GenerateToken(int CVVNumber, long CardNumber)
        {
            var lastDigits = CardNumber.ToString().Substring(CardNumber.ToString().Length - 4).ToList<char>();

            return long.Parse(string.Join("",lastDigits.Skip(CVVNumber).Concat(lastDigits.Take(CVVNumber)).ToList()));

        }
    }
}
