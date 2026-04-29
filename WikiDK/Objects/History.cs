namespace WikiDK.Objects
{
    public class History
    {
        public ulong Id { get; set; }
        public ulong ArticleId { get; set; }
        public ulong EditorId { get; set; }
        public string PreviousContent { get; set; } = string.Empty;
        public DateTime EditDate { get; set; } = DateTime.MinValue;
    }
}
