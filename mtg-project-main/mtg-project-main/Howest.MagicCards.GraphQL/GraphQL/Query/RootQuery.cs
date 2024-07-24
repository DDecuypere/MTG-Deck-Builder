using GraphQL;
using GraphQL.Types;
using Howest.MagicCards.DAL.Repositories;
using System.Net.NetworkInformation;

namespace GraphQLAPI.GraphQLTypes;

    public class RootQuery : ObjectGraphType
    {
        public RootQuery(ICardRepository cardRepo, IArtistRepository artistRepo) 
        {
            Name = "Query";

        #region Cards
        Field<ListGraphType<CardType>>(
            "Cards",
            Description = "Get all the cards",

            resolve: context => cardRepo.GetAllCards().ToList()
            );
        #endregion

        #region Artists
        Field<ListGraphType<ArtistType>>(
            "Artists",
            Description = "Get all the artists",
            arguments: new QueryArguments
            {
                new QueryArgument<IntGraphType> {Name = "show"}
            },
            resolve: context =>
            {
                int limit = context.GetArgument<int>("show");

                return artistRepo.GetAllArtists().Take(limit).ToList();
            }
        );

        Field<ArtistType>(
            "Artist",
            Description = "Get the artist",
            arguments: new QueryArguments
            {
                new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "artistId"}
            },
            resolve: context =>
            {
                int artistId = context.GetArgument<int>("artistId");

                return artistRepo.GetArtist(artistId);
            }
        );
        #endregion
    }
}
