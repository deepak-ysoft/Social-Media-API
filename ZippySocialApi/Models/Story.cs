using System.ComponentModel.DataAnnotations.Schema;

namespace ZippySocialApi.Models
{
    public class Story
    {
        public int Id { get; set; }
        [NotMapped]
        public IFormFile StoryImg { get; set; }
        public string? Path { get; set; }
        public string AboutStory { get; set; }
        public string FileType { get; set; }
        public string UserImage { get; set; }
        public string UserName { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey(nameof(User))] // Foreign key for User
        public int UserId { get; set; }
    }
}
