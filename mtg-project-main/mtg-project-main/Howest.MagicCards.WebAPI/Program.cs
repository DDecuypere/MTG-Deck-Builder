using Asp.Versioning;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Type = System.Type;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.1", new OpenApiInfo
    {
        Title = "Cards API v1.1",
        Version = "v1.1",
        Description = "API to retrieve all the cards info"
    });
    c.SwaggerDoc("v1.5", new OpenApiInfo
    {
        Title = "Cards API v1.5",
        Version = "v1.5",
        Description = "API to retrieve all the cards info, filter the cards and also get detail of a single card"
    });
});
builder.Services.AddApiVersioning( o =>
{
    o.ReportApiVersions = true;
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 1);
    o.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV";
    o.SubstituteApiVersionInUrl = true;
});
builder.Services.AddDbContext<MtgV1Context>(options => options.UseSqlServer(config.GetConnectionString("MtgV1")));
builder.Services.AddScoped<ICardRepository, CardRepository>();

builder.Services.AddAutoMapper(new Type[] { typeof(Howest.MagicCards.Shared.Mappings.CardsProfile) });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Cards API v1.1");
        c.SwaggerEndpoint("/swagger/v1.5/swagger.json", "Cards API v1.5");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
