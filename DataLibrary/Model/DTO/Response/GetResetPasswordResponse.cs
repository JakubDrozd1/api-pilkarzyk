namespace DataLibrary.Model.DTO.Response
{
    public class GetResetPasswordResponse
    {
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public int? IdResetPassword { get; set; }
        public int? IdUser { get; set; }
        public DateTime DateAdd { get; set; }
    }
}
