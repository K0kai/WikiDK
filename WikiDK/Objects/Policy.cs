namespace WikiDK.Objects
{
    public class Policy
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset UpdatedAt { get; private set; } = DateTime.UtcNow;
    }
}
