using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetGroupInviteWithEmailOrPhoneRequest : GetGroupInviteRequest
    {

        [JsonPropertyName("EmailOrPhoneNumber")]
        public string? EMAIL_OR_PHONE_NUMBER { get; set; }
    }
}
