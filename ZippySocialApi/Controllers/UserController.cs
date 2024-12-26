using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZippySocialApi.Content;
using ZippySocialApi.IServices;
using ZippySocialApi.Models;

namespace ZippySocialApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _service;
        private readonly ApplicationDbContext _context;
        public UserController(IUser service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        // To upload story
        [HttpPost("CreateStory")]
        public async Task<IActionResult> CreateStory(Story story)
        {
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            try
            {
                bool res = await _service.UploadStory(story);
                return Ok(new { success = res });
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // To get User Story
        [HttpGet("GetStoryes")]
        public async Task<IActionResult> GetStoryes()
        {
            try
            {
                // Fetch all stories for the user
                var stories = await _context.Story
                    .ToListAsync();

                // Check for expired stories (older than 24 hours)
                var expiredStories = stories
                    .Where(s => s.DateTime.AddHours(24) <= DateTime.UtcNow)
                    .ToList();

                if (expiredStories.Any())
                {
                    // Remove expired stories
                    _context.Story.RemoveRange(expiredStories);
                    await _context.SaveChangesAsync();
                }

                // Fetch valid stories
                var validStories = stories
                    .Where(s => s.DateTime.AddHours(24) > DateTime.UtcNow)
                    .ToList();

                var validStoriesUserIdes = validStories
                                    .GroupBy(s => s.UserId)
                                    .Select(g => g.Key)
                                    .ToList();

                var storiesList = new List<object[]>();

                foreach (var userId in validStoriesUserIdes)
                {
                    // Filter stories for the current UserId
                    var userStories = validStories.Where(x => x.UserId == userId).ToArray();

                    // Add the stories as an array into the list
                    storiesList.Add(userStories);
                }

                return Ok(new { success = true, stories = storiesList });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "An error occurred while get storyes.",
                    error = ex.Message // Include exception details (e.g., ex.StackTrace for more info)
                });
            }
        }

        // To delete User Story
        [HttpDelete("DeleteStory/{storyId}")]
        public async Task<IActionResult> DeleteStory(int storyId)
        {
            var story = await _context.Story.AnyAsync(x => x.Id == storyId);
            if (!story)
            {
                return Ok(new { success = false, message = "Not Found" });
            }
            try
            {
                var res = await _service.DeleteStory(storyId);
                return Ok(new { success = res });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "An error occurred while deleting the story.",
                    error = ex.Message // Include exception details (e.g., ex.StackTrace for more info)
                });
            }
        }
    }
}
