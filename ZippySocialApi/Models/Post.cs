using System.ComponentModel.DataAnnotations.Schema;

namespace ZippySocialApi.Models
{
    public class Post
    {
        public int id { get; set; }
        public IFormFile storyImg { get; set; }
        public string? postPath { get; set; }
        public string aboutPost { get; set; }
        public string postType { get; set; }
        public string userImage { get; set; }
        public string userName { get; set; }
        public DateTime dateTime { get; set; }
        [ForeignKey(nameof(User))] // Foreign key for User
        public int userId
        {
            get; set;
        }
    }
}
