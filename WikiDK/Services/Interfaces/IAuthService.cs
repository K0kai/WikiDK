using WikiDK.Objects;

namespace WikiDK.Services.Interfaces
{
    public interface IAuthService
    {
        public string Login(string username, string password);
        public User Register(string username, string email, string password);

    }
}
