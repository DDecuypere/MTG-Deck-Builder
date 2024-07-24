using AutoMapper;
using Azure;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.MinimalAPI.Wrappers;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.Shared.ViewModels;
using Howest.MagicCards.WebAPI.Wrappers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class Home
    {
        private string _message = string.Empty;
        private int? _allPages = null;
        private int _currentPage = 1;

        private IEnumerable<CardDTO>? _cards = null;
        private IList<CardInDeckReadDTO> _cardsInDeck { get; set; } = new List<CardInDeckReadDTO>();

        private FilterViewModel _filterViewModel;

        private readonly JsonSerializerOptions _jsonOptions;
        private HttpClient _deckClient;
        private HttpClient _cardsClient;

        #region Services
        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }
        [Inject]
        public ProtectedSessionStorage session { get; set; }
        [Inject]
        public IMapper mapper { get; set; }

        #endregion



        public Home()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

        }

        protected override async Task OnInitializedAsync()
        {
            _filterViewModel = new FilterViewModel();

            _cardsClient = HttpClientFactory.CreateClient("CardsApi");
            _deckClient = HttpClientFactory.CreateClient("DeckApi");

            await ShowAllCards();
            await GetDeck();
        }

        private async Task GetDeck()
        {
            HttpResponseMessage response = await _deckClient.GetAsync($"deck");
            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Deckresponse<IList<CardInDeckReadDTO>>? result = 
                    JsonSerializer.Deserialize<Deckresponse<IList<CardInDeckReadDTO>>>(apiResponse, _jsonOptions);

                if (result == null)
                {
                    _cardsInDeck = new List<CardInDeckReadDTO>();
                }
                _cardsInDeck = result.Deck;
            }
            else
            {
                _message = "Not able to get the deck";
                _cardsInDeck = null;
            }
        }


        private async Task ShowAllCards()
        {
            CheckSorting();

            string queryString = ParamsString();

            if (!string.IsNullOrWhiteSpace(queryString))
            {
                queryString = "?" + queryString; 
            }

            HttpResponseMessage response = await _cardsClient.GetAsync($"v1.5/cards{queryString}");
            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                PagedCardsResponse<IEnumerable<CardDTO>>? result =
                    JsonSerializer.Deserialize<PagedCardsResponse<IEnumerable<CardDTO>>>(apiResponse, _jsonOptions);
                _cards = result?.Data;
                _allPages = result?.TotalPages;
            }
            else
            {
                _cards = new List<CardDTO>();
                _message = "Something went wrong getting the cards";
            }

        }

        private async Task AddCardToDeck(CardInDeckWriteDTO cardFromList)
        {
            HttpContent content =
                new StringContent(JsonSerializer.Serialize(cardFromList), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _deckClient.PostAsync("deck", content);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                await GetDeck();
            }
            else
            {
                _message = "Deck has a limit of 60 cards";
            }

        }

        private async Task DeleteCardFromDeck(CardInDeckReadDTO card)
        {

            HttpResponseMessage response = await _deckClient.DeleteAsync($"deck/{card.CardId}");

            if (response.IsSuccessStatusCode)
            {
                await GetDeck();
            }
            else
            {
                _message = "something  went wrong deleting card";
            }
        }

        private async Task ClearDeck()
        {
            HttpResponseMessage response = await _deckClient.DeleteAsync($"deck/clear");

            if (response.IsSuccessStatusCode)
            {
                await GetDeck();
            }
            else
            {
                _message = "something  went wrong clearing deck";
            }
        }

        private string ParamsString()
        {
            string queryString = string.Empty;

            _filterViewModel.GetType().GetProperties().ToList().ForEach(prop =>
            {
                var value = prop.GetValue(_filterViewModel);
                if (value != null)
                {
                    string stringValue = value.ToString();
                    if (!string.IsNullOrWhiteSpace(stringValue))
                    {
                        queryString += $"{prop.Name}={stringValue}&";
                    }
                }
            }
            );

            return queryString;
        }

        private void CheckSorting()
        {
            if (_filterViewModel.IsDescending == true)
            {
                _filterViewModel.SortType = "desc";
            }
            else
            {
                _filterViewModel.SortType = "asc";
            }
        }

        public async Task NextPage()
        {
            if (_currentPage < _allPages)
            {
                _currentPage++;
                _filterViewModel.PageNumber = _currentPage;
                await ShowAllCards();
            }
        }

        public async Task PreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                _filterViewModel.PageNumber = _currentPage;
                await ShowAllCards();
            }
        }
    }
}
