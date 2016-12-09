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
        public string owner { get; set; }
        public string TypeVag { get; set; }
        public string gruz { get; set; }
        public string godn { get; set; }
        public string cond { get; set; }
        public string Station { get; set; }
        public string NumWay { get; set; }
        public DateTime dt_amkr { get; set; }
        public int Hour_Amkr { get; set; }
        public DateTime dt_on_stat { get; set; }
        public int Hour_on_stat { get; set; }
    }
}
