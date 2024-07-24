using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class FilteredList
    {
        public static IEnumerable<Card> ToFilteredList(this IEnumerable<Card> cards, string artistName, string setName, string rarityName, string cardName, string text, string type)
        {
            return cards.Where(card => (card.Artist != null && card.Artist.FullName != null && card.Artist.FullName.StartsWith(artistName))
                                && (card.SetCodeNavigation != null && card.SetCodeNavigation.Name != null && card.SetCodeNavigation.Name.StartsWith(setName))
                                && card.RarityCodeNavigation.Name.Contains(rarityName)
                                && (card.Name != null && card.Name.StartsWith(cardName))
                                && (card.Text != null) && card.Text.Contains(text)
                                && (card.Type != null && card.Type.StartsWith(type)));
        } 
    }
}
