using System.ComponentModel.DataAnnotations;

namespace DataLibrary.Model.DTO.Request
{
    public class GetUsersByLoginAndPasswordRequest
    {
        
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? PasswordNew { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
