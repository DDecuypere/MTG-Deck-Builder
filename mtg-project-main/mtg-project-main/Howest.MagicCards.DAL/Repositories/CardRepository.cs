using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly MtgV1Context _db;
        public CardRepository(MtgV1Context db)
        {
            _db = db;
        }

        public IQueryable<Card> GetAllCards()
        {
            IQueryable<Card> cards = _db.Cards
                                        .Include(card => card.SetCodeNavigation)
                                        .Include(card => card.RarityCodeNavigation)
                                        .Include(card => card.Artist)
                                        .Select(card => card);
            return cards;
        }

        public IQueryable<Card> GetAllCardsByArtistId(long id)
        {
            return _db.Cards
                .Where(card => card.ArtistId == id)
                .Select(card => card);
        }

        public async Task<Card?> GetCard(long id)
        {
            return await _db.Cards
                            .SingleOrDefaultAsync(card => card.Id == id);
        }
    }
}
