using ProfileGenie.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapRazorComponents<ProfileGenie.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
