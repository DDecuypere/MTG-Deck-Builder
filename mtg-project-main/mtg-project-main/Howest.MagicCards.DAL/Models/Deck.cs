using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Models
{
    public partial class Deck
    {
        public ICollection<CardInDeck> Cards { get; set; } = new List<CardInDeck>();
    }
}
