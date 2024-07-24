using GraphQL;
using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace GraphQLAPI.GraphQLTypes;
public class ArtistType: ObjectGraphType<Artist>
{
    public ArtistType(ICardRepository cardRepo) 
    {
        Name = "Artist";
        Field(artist => artist.Id, type: typeof(IdGraphType));
        Field(artist => artist.FullName, type: typeof(StringGraphType));
        Field<ListGraphType<CardType>>
            (
                "Cards",
                Description = "Get all related cards",
                arguments: new QueryArguments
                {
                    new QueryArgument<IntGraphType> { Name = "show", DefaultValue = 25 },
                },
                resolve: context =>
                {
                    int limit = context.GetArgument<int>("show");

                    return cardRepo.GetAllCardsByArtistId((int)context.Source.Id).Take(limit).ToList();
                }
            );
    }
}
