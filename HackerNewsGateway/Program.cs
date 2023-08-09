using AspNetCoreRateLimit;
using HackerNewsGateway;
using HackerNewsGateway.Model;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient(Constants.HackerNewsHttpClientName, c =>
{
    c.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
    c.DefaultRequestHeaders.Accept.Clear();
    c.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.Converters.Add(new DateTimeConverter());
});

// Configure rate limiting options
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Limit = 100, // Maximum number of requests allowed
            Period = "1m" // Time period for the limit
        }
    };
});

// Add rate limiting services
builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddOptions();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

var app = builder.Build();

// Use rate limiting middleware
app.UseIpRateLimiting();

// Configure the HTTP request pipeline.
app.MapGet("/best-stories/{n}", async (IHttpClientFactory httpClientFactory, int n) =>
{
    if (n <= 0)
        return Results.BadRequest("Parameter must be greater than 0.");

    using (var httpClient = httpClientFactory.CreateClient(Constants.HackerNewsHttpClientName))
    {
        var bestStoryIds = await httpClient.GetFromJsonAsync<List<int>>("beststories.json");
        if (bestStoryIds == null || !bestStoryIds.Any())
            return Results.NotFound("Best stories list is empty.");

        var bestStoriesItems = new List<HackerNewsItem>();
        foreach (var storyId in bestStoryIds)
        {
            var storyApiUrl = $"item/{storyId}.json";
            var story = await httpClient.GetFromJsonAsync<HackerNewsItem>(storyApiUrl);
            if (story != null)
                bestStoriesItems.Add(story);
        }

        var selectedBestStories = bestStoriesItems
            .OrderByDescending(b => b.Score)
            .Take(n)
            .Select(p =>
                new HackerNewsStory
                {
                    Title = p.Title,
                    Uri = p.Url,
                    PostedBy = p.By,
                    Time = DateTimeOffset.FromUnixTimeSeconds(p.Time).DateTime,
                    Score = p.Score,
                    CommentCount = p.Kids.Length
                });

        return Results.Ok(selectedBestStories);
    }
});

app.Run();