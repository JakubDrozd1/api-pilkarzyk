namespace DataLibrary.Model.DTO.Response
{
    public class GetArrayUsersResponse
    {
        public required int IdUser { get; set; }
        public string? Avatar { get; set; }
        public string? Login { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
    }
}
