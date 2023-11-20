namespace WebApi.Model.DTO.Request
{
    public class RankingRequest
    {
        public DateTime DateMeeting { get; set; }
        public int IdUser { get; set; }
        public int IdGroup { get; set; }
        public int Point { get; set; }
    }
}
