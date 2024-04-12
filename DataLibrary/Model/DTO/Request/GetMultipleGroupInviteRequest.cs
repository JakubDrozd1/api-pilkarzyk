namespace DataLibrary.Model.DTO.Request
{
    public class GetMultipleGroupInviteRequest
    {
        public required int IdGroup { get; set; }

        public required int IdAuthor { get; set; }

        public int[]? PhoneNumbers { get; set; }
    }
}
