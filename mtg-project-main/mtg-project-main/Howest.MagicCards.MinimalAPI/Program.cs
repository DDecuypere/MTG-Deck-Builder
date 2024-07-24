using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.MinimalAPI.Wrappers;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Mappings;
using Type = System.Type;

const string commonPrefix = "/api";

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;


// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDeckRepository, DeckRepository>();
builder.Services.AddAutoMapper(new Type[] { typeof(DeckProfile) });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Minimal api")
    .WithTags("The Minimal Api");

string urlPrefix = config["ApiPrefix"] ?? commonPrefix;

var deckGroup = app.MapGroup($"{urlPrefix}/deck/")
                      .WithTags("Deck"); ;

deckGroup.MapGet("",(IDeckRepository deckRep, IMapper mapper) => 
{
    return Results.Ok(new Deckresponse<IList<CardInDeckReadDTO>>(deckRep.getDeck().Cards
                                                    .Select(card => mapper.Map<CardInDeckReadDTO>(card)).ToList()));
});

deckGroup.MapPost("", async (IDeckRepository deckRep, CardInDeckWriteDTO newCard, IMapper mapper) =>
{
    try
    {
        Deck currentDeck = await deckRep.AddCardDeck(mapper.Map<Deck>(newCard));
        return Results.Created($"{urlPrefix}/deck", new { Deck = currentDeck.Cards });
    } catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
    .Accepts<CardInDeckWriteDTO>("application/json")
    .Produces<Deck>(StatusCodes.Status201Created);

deckGroup.MapDelete("/{cardId:long}", async (IDeckRepository deckRep, long cardId) =>
{
    Deck? currentDeck = await deckRep.RemoveCardDeck(cardId);
    return currentDeck is not null ? Results.Ok(new { Deck = currentDeck.Cards}) : Results.NotFound("Card not found");
})
    .Produces<Deck>()
    .Produces(StatusCodes.Status404NotFound);

deckGroup.MapDelete("/clear", async (IDeckRepository deckRep) =>
{
    Deck? currentDeck = await deckRep.ClearDeck();
    return Results.Ok("Deck cleared");
})
    .Produces(StatusCodes.Status204NoContent   
);

app.Run();

