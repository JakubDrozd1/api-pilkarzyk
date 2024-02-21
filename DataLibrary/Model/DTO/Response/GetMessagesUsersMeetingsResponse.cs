namespace DataLibrary.Model.DTO.Response
{
    public class GetMessagesUsersMeetingsResponse
    {
        public string? Answer {  get; set; }
        public DateTime? DateAdd { get; set; }
        public DateTime? DateMeeting { get; set;}
        public string? Place {  get; set; }
        public int? Quantity { get; set; }
        public string? Description {  get; set; }
        public string? Email { get; set; }
        public string? Login {  get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public int? PhoneNumber { get; set; }
        public bool? IsAdmin { get; set; }
        public int? IdUser { get; set; }
        public int? IdMeeting { get; set; }
        public DateTime? WaitingTime { get; set; }
        public string? Avatar { get; set; }
        public int? IdMessage { get; set; }
        public int? IdAuthor { get; set; }
        public int? IdGroup { get; set; }

    }
}
