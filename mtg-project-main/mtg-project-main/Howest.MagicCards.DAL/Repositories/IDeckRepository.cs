using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IDeckRepository
    {
        Deck getDeck();
        Task<Deck> AddCardDeck(Deck newDeck);
        Task<Deck?> RemoveCardDeck(long id);
        Task<Deck?> ClearDeck();
    }
}
