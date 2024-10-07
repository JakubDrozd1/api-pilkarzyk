using DataLibrary.Entities;

namespace DataLibrary.Model.DTO.Response
{
    public class GetDateQuestsResponse
    {
        public DateTime DateMeeting { get; set; }
        public required int IdDateQuest { get; set; }
        public bool UserVoted { get; set; }
        public required List<GetArrayUsersResponse> Users { get; set; }
    }
}
