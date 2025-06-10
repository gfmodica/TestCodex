using AccessibilityChecker.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AccessibilityAnalyzer>();
builder.Services.AddSingleton<AccessibilityDeclarationGenerator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/api/accessibility/analyze", async ([FromBody] string url, AccessibilityAnalyzer analyzer) =>
{
    if (string.IsNullOrWhiteSpace(url))
    {
        return Results.BadRequest("URL missing");
    }
    try
    {
        var result = await analyzer.AnalyzeAsync(url);
        return Results.Ok(result);
    }
    catch (HttpRequestException ex)
    {
        return Results.BadRequest($"Error fetching URL: {ex.Message}");
    }
});

app.MapPost("/api/accessibility/declaration", async ([FromBody] string url, AccessibilityAnalyzer analyzer, AccessibilityDeclarationGenerator generator) =>
{
    if (string.IsNullOrWhiteSpace(url))
    {
        return Results.BadRequest("URL missing");
    }
    try
    {
        var result = await analyzer.AnalyzeAsync(url);
        var pdfBytes = generator.Generate(url, result);
        return Results.File(pdfBytes, "application/pdf", "DichiarazioneAccessibilita.pdf");
    }
    catch (HttpRequestException ex)
    {
        return Results.BadRequest($"Error fetching URL: {ex.Message}");
    }
});

app.Run();
