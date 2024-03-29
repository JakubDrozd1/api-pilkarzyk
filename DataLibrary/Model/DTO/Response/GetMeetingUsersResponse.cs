﻿
namespace DataLibrary.Model.DTO.Response
{
    public class GetMeetingUsersResponse
    {
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
        public int? IdUser { get; set; }
        public int? IdMeeting { get; set; }
        public string? Answer {  get; set; }
        public DateTime? WaitingTime { get; set; }

    }
}
