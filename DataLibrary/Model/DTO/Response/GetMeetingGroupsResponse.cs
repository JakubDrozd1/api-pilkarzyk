namespace DataLibrary.Model.DTO.Response
{
    public class GetMeetingGroupsResponse
    {
        public string? Name { get; set; }
        public DateTime? DateMeeting { get; set; }
        public string? Place { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }

    }
}
