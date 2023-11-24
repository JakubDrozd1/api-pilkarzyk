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

    }
}
