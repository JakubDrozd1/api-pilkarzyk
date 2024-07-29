namespace DataLibrary.Model.DTO.Response
{
    public class GetAddWithClicksResponse
    {

        public required int IdAd { get; set; }


        public required string Content { get; set; }


        public required string Url { get; set; }


        public required string Color { get; set; }

        public int ClickCount { get; set; }
    }
}
