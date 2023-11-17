using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    public class Meeting
    {
        public int IdMeeting {  get; set; }
        public DateTime DateMeeting { get; set; }
        public string? Place {  get; set; }
        public string? Quantity { get; set; }
        public string? Description { get; set; }
        public int IdUser { get; set; }
        public int IdGroup { get; set; }
    }
}
