using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class SortedList
    {
        public static IEnumerable<Card> ToSortedList(this IEnumerable<Card> cards, string sortedType)
        {
            if (sortedType == "desc")
            {
                return cards.OrderByDescending(c => c.Name);
            }
            else
            {
                return cards.OrderBy(c => c.Name);
            }
        }
    }
}
