using Howest.MagicCards.Shared.Mappings;
using Howest.MagicCards.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("DeckApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7079/api/");
});

builder.Services.AddHttpClient("CardsApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7195/api/");
});

builder.Services.AddAutoMapper(new Type[] { typeof(DeckProfile) });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
