namespace BLLLibrary.Model.DTO.Response
{
    public class GetGroupsUsersResponse
    {
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public int? PhoneNumber { get; set; }
        public int? AccountType { get; set; }
    }
}
