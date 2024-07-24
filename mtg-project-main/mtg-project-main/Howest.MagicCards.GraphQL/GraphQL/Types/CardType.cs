using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace GraphQLAPI.GraphQLTypes;
public class CardType : ObjectGraphType<Card>
{
    public CardType(IArtistRepository artistRepo)
    {
        Name = "Card";

        Field(card => card.Id, type: typeof(IdGraphType));
        Field(card => card.Name, type: typeof(StringGraphType));
        Field(card => card.Power, type: typeof(StringGraphType));
        Field(card => card.Toughness, type: typeof(StringGraphType));
        Field(card => card.Type, type: typeof(StringGraphType));
        Field(card => card.ManaCost, type: typeof(StringGraphType));
        Field<ArtistType>(
            "Artist",
            Description= "Related artists",
            resolve: context => artistRepo.GetArtist((int)context.Source.Id)
            );
    }
}
