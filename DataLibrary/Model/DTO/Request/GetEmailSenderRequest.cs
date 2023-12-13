namespace DataLibrary.Model.DTO.Request
{
    public class GetEmailSenderRequest
    {
        public required string To { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string GroupName { get; set; }
        public required int IdGroup { get; set; }
        public List<string>? BlindCarbonCopy { get; set; }
        public List<string>? CarbonCopy { get; set; }

    }
}
