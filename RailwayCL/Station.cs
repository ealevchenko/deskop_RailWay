using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    /// <summary>
    /// Класс данных отправки "Станция"
    /// </summary>
    public class Station : SendingPoint
    {
        private Side outer_side;

        public Station()
        {
        }

        public Station(int id, string name)
        {
            base.ID = id;
            base.Name = name;
        }

        public Side Outer_side { get { return outer_side; } set { outer_side = value; } }
    }
}
