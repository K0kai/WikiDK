using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(60)]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; } = string.Empty;
        [Column("password_hash")]
        [JsonIgnore]
        public byte[] PasswordHash { get; set; } = [];
        [Column("password_salt")]
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; } = [];
        [Column("role")]
        public UserRole Role { get; set; } = UserRole.User;
        [Column("user_icon")]
        public string UserIcon { get; set; } = ServerDefaults.DefaultUserIcon;
    }
    public enum UserRole
    {
        User,
        Admin,
        Editor,
        Owner
    }
}
