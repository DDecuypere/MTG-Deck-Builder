using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Extensions;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.Shared.Wrappers;
using Howest.MagicCards.WebAPI.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Howest.MagicCards.WebAPI.Controllers.V1._5
{
    [ApiController]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/cards")]
    public class CardsController15 : ControllerBase

    {
        private const string _key = "allCards";
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _cardsMapper;
        private readonly IMemoryCache _cache;
        public CardsController15(ICardRepository cardRepository, IMapper mapper, IMemoryCache memoryCache)
        {
            _cardRepository = cardRepository;
            _cardsMapper = mapper;
            _cache = memoryCache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardDTO), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<PagedCardsResponse<IEnumerable<CardDTO>>>> GetCards([FromQuery] CardsFilter cardsFilter)
        {
            try
            {
                if (!_cache.TryGetValue(_key, out IEnumerable<Card> cachedResult))
                {
                    cachedResult = await _cardRepository.GetAllCards().ToListAsync();

                    MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    };

                    _cache.Set(_key, cachedResult, cacheOptions);

                    return PagedResponse(cachedResult, cardsFilter);
                }
                return PagedResponse(cachedResult, cardsFilter);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    $"something went wrong: {e.Message}");
            }
        }

        private ActionResult<PagedCardsResponse<IEnumerable<CardDTO>>> PagedResponse(IEnumerable<Card> result, CardsFilter filter)
        {
            return Ok(new PagedCardsResponse<IEnumerable<CardDTO>>(
                    result
                        .ToSortedList(filter.SortType)
                        .ToFilteredList(filter.ArtistName.ToFirstUpperLetter(), filter.SetName.ToFirstUpperLetter(), filter.RarityName.ToFirstUpperLetter(), filter.CardName.ToFirstUpperLetter(), filter.CardText, filter.TypeName.ToFirstUpperLetter())
                        .Skip((filter.PageNumber - 1) * filter.PageSize)
                        .Take(filter.PageSize)
                        .Select(c => _cardsMapper.Map<CardDTO>(c)),
                    filter.PageNumber,
                    filter.PageSize,
                    result.Count()
                    ));
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(CardDTO), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<CardsResponse<CardDTO>>> GetCard(long id)
        {
            try
            {
                if (_cache.TryGetValue(_key, out IEnumerable<Card> cachedResult))
                {
                    return Ok(new CardsResponse<CardDTO>(
                        cachedResult
                        .Select(c => _cardsMapper.Map<CardDTO>(c))
                        .FirstOrDefault(c => c.Id == id)
                        ));
                }
                else
                {
                    var card = await _cardRepository.GetCard(id);
                    if (card == null)
                    {
                        return NotFound("Not able to find that card");
                    }
                    var cardDTO = _cardsMapper.Map<CardDTO>(card);
                    return Ok(new CardsResponse<CardDTO>(
                        cardDTO
                        ));
                }
            } catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    $"something went wrong: {e.Message}");
            }
        }
    }
}
