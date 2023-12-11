namespace DataLibrary.Model.DTO.Request
{
    public class GetUserGroupRequest
    {
        public required int IdUser { get; set; }
        public required int IdGroup { get; set; }
        public int? AccountType { get; set; }
    }
}
