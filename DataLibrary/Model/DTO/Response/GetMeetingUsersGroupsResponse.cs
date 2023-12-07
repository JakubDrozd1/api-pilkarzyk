namespace DataLibrary.Model.DTO.Response
{
    public class GetMeetingUsersGroupsResponse
    {
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public int? PhoneNumber { get; set; }
        public DateTime? DateMeeting { get; set; }
        public string? Place { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public bool? IsAdmin { get; set; }

    }
}
