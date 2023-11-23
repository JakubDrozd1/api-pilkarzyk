using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataLibrary.Model.DTO
{
    internal interface IPagination
    {
        [Required]
        [DefaultValue(0)]
        public int Page { get; set; }

        [Required]
        [DefaultValue(10)]
        public int OnPage { get; set; }
    }
}
