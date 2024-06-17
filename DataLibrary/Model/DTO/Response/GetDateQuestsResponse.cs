using DataLibrary.Entities;

namespace DataLibrary.Model.DTO.Response
{
    public class GetDateQuestsResponse : DATE_QUESTS
    {
        public List <USERS_DATE_QUESTS>? USERS_DATE_QUESTS {  get; set; }
    }
}
