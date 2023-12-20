
using System.Text.Json.Serialization;
using DataLibrary.Helper;

namespace DataLibrary.Model.DTO.Request
{
    public class GetUpdateUserRequest
    {
        public string? LOGIN { get; set; }
        public string? USER_PASSWORD { get; set; }
        public string? EMAIL { get; set; }
        public string? FIRSTNAME { get; set; }
        public string? SURNAME { get; set; }
        public int? PHONE_NUMBER { get; set; }
        [JsonConverter(typeof(JsonToByteArrayConverter))]
        public byte[]? AVATAR { get; set; }
        public required string[] Column {  get; set; }
    }
}
