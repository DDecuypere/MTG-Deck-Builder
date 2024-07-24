using AutoMapper;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class Deck
    {
        [Parameter]
        public IList<CardInDeckReadDTO>? CardsInDeck { get; set; }

        [Parameter]
        public EventCallback<CardInDeckReadDTO> OnDeleteClick { get; set; }

        [Parameter]
        public EventCallback<CardInDeckWriteDTO> OnAddClick { get; set; }

        [Inject]
        public IMapper mapper { get; set; }

        private async Task HandleAddClick(CardInDeckReadDTO card)
        {
            var cardInDeckWriteDTO = mapper.Map<CardInDeckWriteDTO>(card);
            await OnAddClick.InvokeAsync(cardInDeckWriteDTO);
        }
    }
}
