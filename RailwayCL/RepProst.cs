using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class RepProst
    {
        public int NumVag { get; set; }
        public string Owner { get; set; }
        public string TypeVag { get; set; }
        public string Gruz { get; set; }
        public string Godn { get; set; }
        public string Cond { get; set; }
        public string Station { get; set; }
        public string NumWay { get; set; }
        public DateTime Dt_Amkr { get; set; }
        public int Hour_Amkr { get; set; }
        public DateTime Dt_on_stat { get; set; }
        public int Hour_on_stat { get; set; }
    }
}
