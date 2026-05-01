using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
            var user = _userServiceContext.GetByName(username).Result ?? throw new Exception($"User with username '{username}' not found");

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Password is incorrect");

            var claims = new List<System.Security.Claims.Claim>
            {
                new(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(System.Security.Claims.ClaimTypes.Name, user.Name),
                new(System.Security.Claims.ClaimTypes.Role, user.Role.ToString())
            };

            var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY")!)); // Replace with your secret key
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public User Register(string username, string email, string password)
        {
            var userNameExists = _userServiceContext.GetByName(username).Result != null;
            var emailExists = _userServiceContext.GetByEmail(email).Result != null;

            if (userNameExists)
                throw new Exception($"An user has already been registered with the username '{username}'");
            if (emailExists)
                throw new Exception($"An user has already been registered with the email '{email}'");
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password cannot be empty");

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Name = username,
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = UserRole.User
            };

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
