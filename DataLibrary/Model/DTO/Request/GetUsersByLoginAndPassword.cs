using System.ComponentModel.DataAnnotations;

namespace DataLibrary.Model.DTO.Request
{
    public class GetUsersByLoginAndPassword
    {
        [Required]
        public required string Login { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
