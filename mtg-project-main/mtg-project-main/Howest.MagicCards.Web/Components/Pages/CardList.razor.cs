using AutoMapper;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class CardList
    {
        [Parameter]
        public IEnumerable<CardDTO>? Cards { get; set; }

        [Parameter]
        public EventCallback<CardInDeckWriteDTO> OnCardClick { get; set; }

        [Inject]
        public IMapper mapper { get; set; }

        private async Task HandleCardClick(CardDTO card)
        {
            var cardInDeckWriteDTO = mapper.Map<CardInDeckWriteDTO>(card);
            await OnCardClick.InvokeAsync(cardInDeckWriteDTO);
        }
    }
}
