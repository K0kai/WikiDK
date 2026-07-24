using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using WikiDK.Controllers;
using WikiDK.Objects;
using WikiDK.Repositories;

namespace WikiDK.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<dynamic> GetUserAndRoles(int id)
        {
            var user = await GetById(id) ?? throw new Exception("User is null");
            var roles = await _context.UsersRoles.Where(ur => ur.UserId == id).ToListAsync();
            dynamic response = new ExpandoObject();
            response.User = user;
            response.Roles = roles;
            return response;
        }
        public async Task<User?> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User?> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User?> GetByName(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
        }
        public async Task<User> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> Update(int id, UpdateInfoRequest UIR)
        {
            var user = await GetById(id) ?? throw new Exception("User does not exist");
            user.Email = string.IsNullOrWhiteSpace(UIR.Email) ? user.Email : UIR.Email;
            user.Name = string.IsNullOrWhiteSpace(UIR.Name) ? user.Name : UIR.Name;
            await _context.SaveChangesAsync();
            return user;
        }


    }
}
