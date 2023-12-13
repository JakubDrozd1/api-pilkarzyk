namespace DataLibrary.Entities
{
    public class EMAIL_SENDER
    {
        public required string DISPLAY_NAME { get; set; }
        public required string EMAIL { get; set; }
        public required string EMAIL_PASSWORD { get; set; }
        public required string HOST { get; set; }
        public required string SALT { get; set; }
        public required int PORT { get; set; }
        public required bool SSL { get; set; }
        public required bool TLS { get; set; }
    }
}
