using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class Way
    {
        private int id = -1;
        private Station stat;
        private string num = "";
        private string name = "";
        private int vag_amount = 0;
        private int capacity = 0;
        private string numName = "";
        private Cond bind_cond = new Cond();
      

        public Way()
        {
        }

        public Way(int id, Station stat, string num, string name)
        {
            this.id = id;
            this.stat = stat;
            this.num = num;
            this.name = name;
        }

        public int ID { get { return id; } set { id = value; } }

        public Station Stat { get { return stat; } set { stat = value; } }

        public string Num { get { return num; } set { num = value; } }

        public string Name { get { return name; } set { name = value; } }

        public int Vag_amount
        {
            get { return vag_amount; }
            set { vag_amount = value; }
        }

        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public string NumName { get { return numName; } set { numName = value; } }

        public Cond Bind_cond
        {
            get { return bind_cond; }
            set { bind_cond = value; }
        }
    }
}
