using System.Text.Json.Serialization;

namespace DataLibrary.Entities
{
    public class USERS
    {
        public int ID_USER { get; set; }
        public required string LOGIN { get; set; }
        public required string USER_PASSWORD { get; set; }
        public required string EMAIL { get; set; }
        public required string FIRSTNAME { get; set; }
        public required string SURNAME { get; set; }
        public int PHONE_NUMBER { get; set; }
        public bool IS_ADMIN { get; set; }
        public string? SALT { get; set; }
        public string? AVATAR { get; set; }
    }
}
