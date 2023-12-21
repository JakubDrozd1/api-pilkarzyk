using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataLibrary.Model.DTO.Request
{
    public class GetMeetingsGroupsPaginationRequest: ISortable, IPagination
    {
        [Required]
        [DefaultValue(0)]
        public int Page { get; set; }
        [Required]
        [DefaultValue(10)]
        public int OnPage { get; set; }
        public string? SortColumn { get; set; }
        public string? SortMode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? IdGroup { get; set; }
        public int? IdUser { get; set; }
        public string? Answer { get; set; }
    }
}
