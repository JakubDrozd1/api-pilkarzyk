namespace DataLibrary.Model.DTO.Request.EmailRequest
{
    public class GetEmailInvitationGroupRequest
    {
        public required string To { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string GroupName { get; set; }
        public required int IdGroupInvite { get; set; }
    }
}
