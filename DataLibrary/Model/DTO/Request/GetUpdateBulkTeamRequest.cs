namespace DataLibrary.Model.DTO.Request
{
    public class GetUpdateBulkTeamRequest
    {
        public required int IdMeeting { get; set; }
        public required GetUpdateTeamRequest[] Teams { get; set; }
    }
}
