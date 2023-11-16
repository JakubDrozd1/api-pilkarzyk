using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    internal class Message
    {
        public int IdMessage { get; set; }
        public int IdMeeting { get; set; }
        public int IdUser { get; set; }
        public string? Answer {  get; set; }
    }
}
