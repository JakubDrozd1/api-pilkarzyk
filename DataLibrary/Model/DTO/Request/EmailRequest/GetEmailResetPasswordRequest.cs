namespace DataLibrary.Model.DTO.Request.EmailRequest
{
    public class GetEmailResetPasswordRequest
    {
        public required string To { get; set; }
        public required int IdResetPassword { get; set; }
    }
}
