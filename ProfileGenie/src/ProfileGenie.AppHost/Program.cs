var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("profilegenie-db")
    .WithDataVolume();

var apiService = builder.AddProject<Projects.ProfileGenie_ApiService>("apiservice")
    .WithReference(postgres)
    .WithEnvironment("DataProtectionKeysPath", "/home/app/.aspnet/DataProtection-Keys")
    .WithEnvironment("ScreenshotsPath", "/app/screenshots");

var playwrightService = builder.AddProject<Projects.ProfileGenie_PlaywrightService>("playwrightservice")
    .WithEnvironment("ScreenshotsPath", "/app/screenshots");

builder.AddProject<Projects.ProfileGenie_Web>("web")
    .WithReference(apiService)
    .WithReference(playwrightService);

builder.Build().Run();

