namespace DataLibrary.Model.DTO.Request.Pagination
{
    public class Pagination : ISortable, IPagination
    {
        public int Page { get; set; }
        public int OnPage { get; set; }
        public string? SortColumn { get; set; }
        public string? SortMode { get; set; }
    }
}
