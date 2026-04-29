namespace WikiDK.Objects
{
    public class Article
    {
        public ulong Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.MinValue;
        public DateTime Updated { get; set; } = DateTime.MinValue;
        public ulong AuthorId { get; set; }

    }
}
