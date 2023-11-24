namespace DataLibrary.Model.DTO.Response
{
    public class GetRankingsUsersGroupsResponse
    {
        public DateTime? DateMeeting { get; set; }
        public int? Point {  get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public int? PhoneNumber { get; set; }
        public bool? IsAdmin { get; set; }

    }
}
