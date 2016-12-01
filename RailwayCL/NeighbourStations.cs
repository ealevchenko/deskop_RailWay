using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class NeighbourStations
    {
        private int id;
        private Station stat1;
        private Station stat2;
        private Side sideStat1;
        private Side sideStat2;

        public Side SideStat2
        {
            get { return sideStat2; }
            set { sideStat2 = value; }
        }
        

        public Side SideStat1
        {
            get { return sideStat1; }
            set { sideStat1 = value; }
        }
        

        public Station Stat2
        {
            get { return stat2; }
            set { stat2 = value; }
        }
        

        public Station Stat1
        {
            get { return stat1; }
            set { stat1 = value; }
        }
        

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
    }
}
