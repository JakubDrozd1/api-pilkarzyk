namespace DataLibrary.Model.DTO.Response
{
    public class GetGroupsUsersResponse
    {
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public int? PhoneNumber { get; set; }
        public bool? IsAdmin { get; set; }
        public int? AccountType { get; set; }
        public int? IdGroup { get; set; }
        public int? IdUser { get; set; }
        public int? IdGroupUser { get; set; }
        public string? Avatar { get; set; }
    }
}
