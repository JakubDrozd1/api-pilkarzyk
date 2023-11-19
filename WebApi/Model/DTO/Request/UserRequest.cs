namespace WebApi.Model.DTO.Request
{
    public class UserRequest
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? PhoneNumber { get; set; }
        public string? AccountType { get; set; }
    }
}
