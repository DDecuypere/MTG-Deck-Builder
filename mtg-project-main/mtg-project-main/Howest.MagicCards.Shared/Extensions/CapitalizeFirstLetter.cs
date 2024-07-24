using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class CapitalizeFirstLetter
    {
        public static string ToFirstUpperLetter(this string stringToUpper)
        {
            if (string.IsNullOrEmpty(stringToUpper))
                return stringToUpper;

            return char.ToUpper(stringToUpper[0]) + stringToUpper.Substring(1);
        }
    }
}
