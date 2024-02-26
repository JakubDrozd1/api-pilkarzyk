namespace DataLibrary.Model.DTO.Response
{
    public class GetChatMessagesResponse
    {
        public DateTime? DateSend { get; set; }
        public string? ChatMessage { get; set; }
        public int? IdUser { get; set; }
        public string? Login { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public string? Avatar { get; set; }

    }
}
