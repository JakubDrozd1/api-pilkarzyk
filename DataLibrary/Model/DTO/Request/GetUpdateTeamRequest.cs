namespace DataLibrary.Model.DTO.Request
{
    public class GetUpdateTeamRequest
    {
        public required int IdTeam { get; set; }
        public required string Name { get; set; }
        public required string Color { get; set; }
    }
}
