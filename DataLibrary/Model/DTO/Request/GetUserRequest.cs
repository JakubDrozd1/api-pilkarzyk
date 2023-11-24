namespace WebApi.Model.DTO.Request
{
    public class GetUserRequest
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string Firstname { get; set; }
        public required string Surname { get; set; }
        public int PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
    }
}
