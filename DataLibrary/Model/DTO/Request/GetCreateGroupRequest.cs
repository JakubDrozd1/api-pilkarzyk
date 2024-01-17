using DataLibrary.Entities;

namespace DataLibrary.Model.DTO.Request
{
    public class GetCreateGroupRequest
    {
        public required GetGroupRequest GroupRequest { get; set; }
        public required USERS User { get; set; }
    }
}
