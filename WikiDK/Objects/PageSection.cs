using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace WikiDK.Objects
{
    public class PageSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [MaxLength(100)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;
        [Column("content")]
        public string Content { get; set; } = string.Empty;
        [MaxLength(255)]
        [Column("slug")]
        public string Slug { get; set; } = string.Empty;
        [Column("order")]
        public int Order { get; set; }
        [Column("visible")]
        public bool IsVisible { get; set; } = true;

        public void GenerateSlug()
        {
            var titleNoDupeSpaces = Regex.Replace(Title, @"\s+", " ");
            var titleMinus = Regex.Replace(titleNoDupeSpaces, @"\s+", "-");
            var slug = $"{titleMinus}-{Id}";
            Slug = slug.ToLower();
        }
    }
}
