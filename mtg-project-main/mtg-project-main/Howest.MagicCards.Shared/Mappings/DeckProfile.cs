using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Mappings
{
    public class DeckProfile : Profile
    {
        public DeckProfile() 
        {
            CreateMap<CardInDeckWriteDTO, Deck>()
                .ForMember(deck => deck.Cards,
                            opt => opt.MapFrom(dto => new List<CardInDeck> { new CardInDeck { CardId = (long?)dto.CardId, CardName = dto.CardName } })
                );
            CreateMap<CardDTO, CardInDeckWriteDTO>()
                .ForMember(deckDTO => deckDTO.CardId,
                            opt => opt.MapFrom(cardDTO => cardDTO.Id)
                )
                .ForMember(deckDTO => deckDTO.CardName,
                            opt => opt.MapFrom(cardDTO => cardDTO.Name)
                );
            CreateMap<CardInDeckReadDTO, CardInDeckWriteDTO>();
            CreateMap<CardInDeck, CardInDeckReadDTO>();
        }
    }
}
