using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetGroupInviteWithEmailOrPhoneRequest 
    {
        public required GetGroupInviteRequest GroupInvite {  get; set; }
        public string? EmailOrPhoneNumber { get; set; }
    }
}
