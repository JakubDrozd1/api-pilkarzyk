namespace WebApi.Model.DTO.Request
{
    public class UserRequest
    {
        public int IdUser { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public int? PhoneNumber { get; set; }
        public string? AccountType { get; set; }
    }
}
