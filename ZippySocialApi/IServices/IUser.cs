using ZippySocialApi.Models;

namespace ZippySocialApi.IServices
{
    public interface IUser
    {
        public Task<bool> UserLogin(LoginVM login);
        public Task<bool> RegisterUser(User user);
        public Task<string> UploadUserPhoto(IFormFile photo,string st);
        public Task<bool> UploadStory(Story st);
        public Task<List<Story>> GetStoryes();
        public Task<bool> DeleteStory(int storyId);
    }
}
