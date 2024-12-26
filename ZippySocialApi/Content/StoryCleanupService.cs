using Microsoft.EntityFrameworkCore;
using ZippySocialApi.Models;

public class StoryCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StoryCleanupService> _logger;
    private readonly IWebHostEnvironment _env;

    public StoryCleanupService(IServiceProvider serviceProvider, IWebHostEnvironment env, ILogger<StoryCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _env = env;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Story cleanup service started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Perform cleanup of expired stories
                await CleanupExpiredStories(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while cleaning up expired stories.");
            }

            // Wait for 1 hour before the next cleanup
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
        _logger.LogInformation("Story cleanup service stopped.");
    }

    private async Task CleanupExpiredStories(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Find all stories that are older than 24 hours
        var expiredStories = await context.Story
            .Where(s => s.DateTime.AddHours(24) <= DateTime.Now)
            .ToListAsync(stoppingToken);
        var expiredStorie = await context.Story
            .Where(s => s.DateTime.AddHours(24) <= DateTime.Now)
            .ToListAsync();

        if (expiredStories.Any())
        {
            context.Story.RemoveRange(expiredStories);
            await context.SaveChangesAsync(stoppingToken);
           foreach(var story in expiredStorie)
            {
                string img = Path.GetFileName(story.Path);
                var filePath = Path.Combine(_env.ContentRootPath, "uploads/images/userStory/", img); // Combine wwwroot/Img Folder and database ImagePath.

                if (System.IO.File.Exists(filePath)) // Check if the image exists in the file system.
                {
                    System.IO.File.Delete(filePath); // Delete image if it not default image or exist 
                }
            }
            _logger.LogInformation($"Deleted {expiredStories.Count} expired stories.");
        }
        else
        {
            _logger.LogInformation("No expired stories found during cleanup.");
        }
    }
}
