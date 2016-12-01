using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class Cond
    {
        private int id = -1;
        private string name = "";
        private int id_cond_after = -1;
       

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Id_cond_after
        {
            get { return id_cond_after; }
            set { id_cond_after = value; }
        }
        
    }
}
