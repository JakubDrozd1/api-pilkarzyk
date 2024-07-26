namespace DataLibrary.Model.DTO.Response
{
    public class GetGuestsMeetingsResponse
    {
        public int ID_GUEST { get; set; }
        public string? NAME { get; set; }
        public int IDMEETING { get; set; }
        public int? IDTEAM { get; set; }
        public string? TeamColor { get; set; }
        public bool? Canceled { get; set; }
    }
}
