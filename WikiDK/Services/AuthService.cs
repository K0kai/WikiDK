using WikiDK.Objects;

namespace WikiDK.Services
{
    public class AuthService : IAuthService
    {
        private UserService _userServiceContext;
        public AuthService(UserService userServiceContext)
        {
            _userServiceContext = userServiceContext;
        }
        public string Login(string username, string password)
        {
            var user = _userServiceContext.GetByName(username).Result;

            //if (user == null)
                throw new NullReferenceException(nameof(user));


        }

        public User Register(string username, string email, string password)
        {
            throw new NotImplementedException();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
