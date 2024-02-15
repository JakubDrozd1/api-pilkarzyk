namespace DataLibrary.Model.DTO.Response
{
    public class GetGroupInviteResponse
    {
        public string? Name { get; set; }
        public int IdGroupInvite { get; set; }
        public int IdGroup { get; set; }
        public int IdUser { get; set; }
        public int IdAuthor { get; set; }
        public DateTime DateAdd { get; set; }
        public string? FirstnameAuthor {  get; set; }
        public string? SurnameAuthor { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
    }
}
