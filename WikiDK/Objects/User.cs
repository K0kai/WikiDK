namespace WikiDK.Objects
{
    public class User
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = [];
        public byte[] PasswordSalt { get; set;} = [];
        public UserRole Role { get; set; } = UserRole.User;
    }
    public enum UserRole
    {
        User,
        Admin,
        Editor
    }
}
