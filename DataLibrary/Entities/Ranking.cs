using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities
{
    internal class Ranking
    {
        public int IdRanking { get; set; }
        public DateTime DateMeeting { get; set; }
        public int IdUser { get; set; }
        public int IdGroup { get; set; }
        public int Point {  get; set; }
    }
}
