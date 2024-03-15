using Microsoft.AspNetCore.Mvc;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetUserGroupRequest
    {
        [FromQuery(Name = "IdUser")]
        public required int IDUSER { get; set; }

        [FromQuery(Name = "IdGroup")]
        public required int IDGROUP { get; set; }

        [FromQuery(Name = "AccountType")]
        public int? ACCOUNT_TYPE { get; set; }
    }
}
