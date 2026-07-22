namespace WikiDK.Helpers
{
    public class SlugHelper
    {
        public static string Slugify(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            // Convert to lower case
            text = text.ToLowerInvariant();
            // Remove invalid characters
            text = System.Text.RegularExpressions.Regex.Replace(text, @"[^a-z0-9\s-]", "");
            // Replace multiple spaces with a single space
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();
            // Replace spaces with hyphens
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s", "-");
            return text;
        }
    }
}
