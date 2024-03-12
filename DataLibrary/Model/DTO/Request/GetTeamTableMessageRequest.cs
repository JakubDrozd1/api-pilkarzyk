using DataLibrary.Entities;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.Model.DTO.Request
{
    public class GetTeamTableMessageRequest
    {
        public TEAMS[]? Teams { get; set; }
        public Dictionary<int, List<GetMessagesUsersMeetingsResponse>>? UpdatedTeams { get; set; }
    }
}
