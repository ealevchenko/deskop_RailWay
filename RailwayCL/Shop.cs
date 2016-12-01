using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class Shop : SendingPoint
    {
        private Station stat = new Station();
        private int vag_amount = 0;

        public Shop()
        {
        }

        public Station Stat
        {
            get { return stat; }
            set { stat = value; }
        }

        public int Vag_amount
        {
            get { return vag_amount; }
            set { vag_amount = value; }
        }
    }
}
