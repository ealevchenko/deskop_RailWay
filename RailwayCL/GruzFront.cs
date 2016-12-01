using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    /// <summary>
    /// Класс данных вагоноопрокид
    /// </summary>
    public class GruzFront : SendingPoint
    {
        //private int id = -1;
        private Station stat = new Station();
        //private string name = "";
        private int vag_amount = 0;

        public GruzFront()
        {
        }

        //public int Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}

        public Station Stat
        {
            get { return stat; }
            set { stat = value; }
        }

        //public string Name
        //{
        //    get { return name; }
        //    set { name = value; }
        //}

        public int Vag_amount
        {
            get { return vag_amount; }
            set { vag_amount = value; }
        }
    }
}
