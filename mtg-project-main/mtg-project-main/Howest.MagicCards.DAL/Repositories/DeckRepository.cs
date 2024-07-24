using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Howest.MagicCards.DAL.Repositories
{
    public class DeckRepository : IDeckRepository
    {
        private readonly string _filePath = "../DeckDataSource.json";
        private Deck _deck;

        public DeckRepository()
        {
            _deck = LoadDeck();
        }

        public async Task<Deck> AddCardDeck(Deck newDeck)
        {
            var totalCardsInDeck = _deck.Cards.Sum(card => card.Quantity);
            if (totalCardsInDeck == 60)
            {
                throw new Exception("Exceeded deck limit, card is not added to deck");
            }
            else
            {
                foreach (var card in newDeck.Cards)
                {
                    var existingCard = _deck.Cards.FirstOrDefault(c => c.CardId == card.CardId);
                    if (existingCard != null)
                    {
                        existingCard.Quantity += card.Quantity;
                    }
                    else
                    {
                        _deck.Cards.Add(card);
                    }
                }
                await SaveDeck(_deck);

                return _deck;
            }
        }

        public async Task<Deck?> ClearDeck()
        {
            _deck.Cards.Clear();
            await SaveDeck(_deck);
            return _deck;
        }

        public Deck getDeck()
        {
            return _deck;
        }

        public async Task<Deck?> RemoveCardDeck(long id)
        {
            var cardToRemove = _deck.Cards.FirstOrDefault(card => card.CardId == id);
            if (cardToRemove != null)
            {
                if (cardToRemove.Quantity > 1)
                {
                    cardToRemove.Quantity -= 1;
                    await SaveDeck(_deck);
                    return _deck;
                }
                _deck.Cards.Remove(cardToRemove);
                await SaveDeck(_deck);
                return _deck;
            }
            return null;
        }

        private Deck LoadDeck()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    var json = File.ReadAllText(_filePath);
                    return JsonConvert.DeserializeObject<Deck>(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading deck: {ex.Message}");
                    return new Deck();
                }
            }
            else
            {
                return new Deck();
            }
        }

        private async Task SaveDeck(Deck deck)
        {
            try
            {
                var json = JsonConvert.SerializeObject(deck);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving deck: {ex.Message}");
            }
        }
    }
}
