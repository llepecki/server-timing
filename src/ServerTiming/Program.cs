using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServerTiming;
using ServerTiming.Fakes;
using System;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<FakeFile>()
    .AddScoped<FakeDatabase>()
    .AddScoped<ServerTimingTracker>()
    .AddScoped<ServerTimingMiddleware>()
    .AddSingleton(TimeProvider.System);

var app = builder.Build();

app.MapGet("/echo", async ([FromQuery] string text, ServerTimingTracker tracker, FakeFile file, FakeDatabase database, CancellationToken cancellationToken) =>
{
    using ServerTimingToken token = tracker.Start("echo");

    var fileResult = await file.Read(cancellationToken);
    var databaseResult = await database.Query(cancellationToken);

    return Results.Ok(new { fileResult, databaseResult, text });
});

app.UseMiddleware<ServerTimingMiddleware>();

await app.RunAsync();
