using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQLAPI.GraphQLTypes;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

builder.Services.AddDbContext<MtgV1Context>(options => options.UseSqlServer(config.GetConnectionString("MtgV1")));


builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();

builder.Services.AddScoped<RootSchema>();
builder.Services.AddGraphQL()
        .AddSystemTextJson()
        .AddGraphTypes(typeof(RootSchema), ServiceLifetime.Scoped)
        .AddDataLoader();

var app = builder.Build();

app.UseGraphQL<RootSchema>();

app.UseGraphQLPlayground(
    "/ui/playground",
    new PlaygroundOptions()
{
EditorTheme = EditorTheme.Light
});

app.Run();
