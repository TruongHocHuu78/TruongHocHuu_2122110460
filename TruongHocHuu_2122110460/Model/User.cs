using CloudinaryDotNet.Actions;

namespace TruongHocHuu_2122110460.Model
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role {  get; set; }
    }
}
