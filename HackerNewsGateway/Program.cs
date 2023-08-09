using HackerNewsGateway.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/stories/best/{n}", async (IHttpClientFactory httpClientFactory, int n) =>
{
    if (n <= 0)
    {
        return Results.BadRequest("Invalid value for n.");
    }

    var httpClient = httpClientFactory.CreateClient();
    var hackerNewsApiUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";

    var bestStoryIds = await httpClient.GetFromJsonAsync<List<int>>(hackerNewsApiUrl);
    if (bestStoryIds == null || bestStoryIds.Count == 0)
    {
        return Results.NotFound("No best stories found.");
    }

    var bestStories = new List<HackerNewsItem>();
    foreach (var storyId in bestStoryIds)
    {
        var storyApiUrl = $"https://hacker-news.firebaseio.com/v0/item/{storyId}.json";
        var story = await httpClient.GetFromJsonAsync<HackerNewsItem>(storyApiUrl);
        if (story != null)
        {
            bestStories.Add(story);
        }
    }

    var selectedBestStories = bestStories.OrderByDescending(b => b.Score).Take(n);

    return Results.Ok(selectedBestStories);
});


app.Run();