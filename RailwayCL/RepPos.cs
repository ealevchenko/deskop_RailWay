using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class RepPos
    {
        private int isLoaded = -1;
        private string detailsName = "";
        private int amount = 0;
        private int wayOrStatId = -1;
        private string wayOrStatName = "";

        public int IsLoaded
        {
            get { return isLoaded; }
            set { isLoaded = value; }
        }

        public string DetailsName
        {
            get { return detailsName; }
            set { detailsName = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public int WayOrStatId
        {
            get { return wayOrStatId; }
            set { wayOrStatId = value; }
        }

        public string WayOrStatName 
        {
            get { return wayOrStatName; }
            set { wayOrStatName = value; }
        }
    }
}
