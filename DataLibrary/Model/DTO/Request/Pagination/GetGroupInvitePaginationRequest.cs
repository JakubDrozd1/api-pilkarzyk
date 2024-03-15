using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataLibrary.Model.DTO.Request.Pagination
{
    public class GetGroupInvitePaginationRequest : ISortable, IPagination
    {
        [Required]
        [DefaultValue(0)]
        public int Page { get; set; }
        [Required]
        [DefaultValue(10)]
        public int OnPage { get; set; }
        public string? SortColumn { get; set; }
        public string? SortMode { get; set; }
        public int? IdUser { get; set; }
        public int? IdGroup { get; set;}
        public string? Email { get; set; }
    }
}
