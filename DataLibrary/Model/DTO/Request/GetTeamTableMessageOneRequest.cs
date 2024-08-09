using DataLibrary.Entities;
using DataLibrary.Model.DTO.Response;

namespace DataLibrary.Model.DTO.Request
{
    public class GetTeamTableMessageOneRequest
    {
        public int IdMeeting { get; set; }
        public int? IdTeam { get; set; }
        public int? IdUser { get; set; }
        public int? IdGuest { get; set; }
        public int IdAuthor { get; set; }
    }
}
