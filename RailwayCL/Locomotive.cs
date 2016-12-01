using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class Locomotive
    {
        private int id = -1;
        private Station stat = new Station();
        private int num = 0;
        private string num_seria = "";

        public int ID { get { return id; } set { id = value; } }

        public Station Stat { get { return stat; } set { stat = value; } }

        public int Num { get { return num; } set { num = value; } }

        public string Num_seria { get { return num_seria; } set { num_seria = value; } }
    }
}
