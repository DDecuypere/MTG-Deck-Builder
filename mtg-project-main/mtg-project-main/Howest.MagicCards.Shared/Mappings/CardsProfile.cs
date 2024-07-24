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
    public class CardsProfile : Profile
    {
        public CardsProfile() 
        {
            CreateMap<Card, CardDTO>()
                .ForMember(dto => dto.Rarity,
                            opt => opt.MapFrom(card => card.RarityCodeNavigation.Name))
                .ForMember(dto => dto.Artist,
                            opt => opt.MapFrom(card => card.Artist.FullName))
                .ForMember(dto => dto.Set,
                            opt => opt.MapFrom(card => card.SetCodeNavigation.Name));
        }
    }
}
