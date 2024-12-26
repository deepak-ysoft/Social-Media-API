using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZippySocialApi.IServices;
using ZippySocialApi.Models;

namespace ZippySocialApi.Content
{
    public class UserContent : IUser
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        public UserContent(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // User register contant if user is valid
        public async Task<bool> RegisterUser(User user)
        {
            // Handle file upload and user creation logic...
            string fileName = user.Photo.FileName.ToString() != "Default.jpg" ? await UploadUserPhoto(user.Photo, "user image") : "Default.jpg"; //If User Photo field is null then make it "Default.jpg".
            fileName = "https://localhost:7071/uploads/images/users/" + fileName; // File name with url.

            var hasher = new PasswordHasher<User>();
            var userData = new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = hasher.HashPassword(user, user.Password),
                ConPassword = user.ConPassword,
                Phone = user.Phone,
                ImagePath = fileName,
                Gender = user.Gender,
                AboutYou = user.AboutYou,
                DOB = user.DOB,
            };
            _context.Users.Add(userData); // Add in database
            int res = await _context.SaveChangesAsync();  // If save changes return 1 then send user registered successfully (true).
            if (res == 1)
                return true;
            return false;
        }

        // Login the user 
        public async Task<bool> UserLogin(LoginVM login)
        {
            var hasher = new PasswordHasher<User>();
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == login.Email);
            if (user != null)
            {
                var passwordVerificationResult = hasher.VerifyHashedPassword(user, user.Password, login.Password); // To verify password
                if (passwordVerificationResult == PasswordVerificationResult.Success)
                    return true;
            }
            return false;
        }

        // To upload user image when user select image
        public async Task<string> UploadUserPhoto(IFormFile photo, string type)
        {
            if (photo.FileName == null)
            { return null; }
            string shortGuid = Guid.NewGuid().ToString().Substring(0, 8);
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            string originalName = Path.GetFileNameWithoutExtension(photo.FileName);

            // Shorten the original name if it’s longer than 10 characters
            string shortenedName = originalName.Length > 10 ? originalName.Substring(0, 10) : originalName;
            string folder = "";
            if (type == "user image")
                folder = Path.Combine(_env.ContentRootPath, "uploads/images/user");
            else if (type == "user story")
                folder = Path.Combine(_env.ContentRootPath, "uploads/images/userStory");
            string fileName = $"{shortGuid}_{timestamp}_{shortenedName}{Path.GetExtension(photo.FileName)}";
            string filePath = Path.Combine(folder, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream);
            }
            return fileName;
        }

        public async Task<bool> UploadStory(Story st)
        {
            var userStory = await UploadUserPhoto(st.StoryImg, "user story");
            userStory = "https://localhost:7071/uploads/images/userStory/" + userStory;
            var story = new Story()
            {
                Path = userStory,
                FileType = st.FileType,
                UserImage = st.UserImage,
                UserName = st.UserName,
                AboutStory = st.AboutStory,
                DateTime = DateTime.UtcNow,
                UserId = st.UserId,
            };
            await _context.AddAsync(story);
            int res = await _context.SaveChangesAsync();
            if (res == 0)
            { return false; }
            return true;
        }

        public async Task<List<Story>> GetStoryes()
        {
            var data = _context.Story.ToList();
            return data;
        }

        public async Task<bool> DeleteStory(int storyId)
        {
            var story = await _context.Story.FirstOrDefaultAsync(x => x.Id == storyId);

            _context.Story.Remove(story);

            int res = await _context.SaveChangesAsync();
            if (res == 0)
            { return false; }
            return true;
        }
    }
}
