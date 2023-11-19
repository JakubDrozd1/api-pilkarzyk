using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    public class User
    {
        public int IdUser { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public int? PhoneNumber { get; set; }
        public string? AccountType {  get; set; }
    }
}
