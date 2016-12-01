using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    /// <summary>
    /// Класс данных отправки
    /// </summary>
    public class SendingPoint
    {
        private int id = -1;
        private string name = "";
        private string pointName = "";

        public SendingPoint()
        {
        }

        public SendingPoint(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int ID { get { return id; } set { id = value; } }

        public string Name { get { return name; } set { name = value; } }

        public string PointName { get { return pointName; } set { pointName = value; } }
    }
}
