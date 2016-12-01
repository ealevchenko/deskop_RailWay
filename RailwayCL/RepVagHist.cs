using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class RepVagHist
    {
        public string PointType { get; set; }
        public string Station_name { get; set; }
        public string Point { get; set; }
        public int HoursAtPoint { get; set; }
        public int MinAtPoint { get; set; }
        public string DtOnStat { get; set; }
        public string DtFromStat { get; set; }
    }
}
