using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    /// <summary>
    /// Класс поезд (состав)
    /// </summary>
    public class Train
    {
        private int num = 0;
        private SendingPoint sendingPoint = new SendingPoint();
        //private Station stationFrom;
        //private GruzFront gruzFront = new GruzFront();
        //private Shop shop = new Shop();
        private Station stationTo;
        private DateTime dateFromStat;
        private int vag_amount;
        private int st_lock_locom1 = -1;
        private int st_lock_locom2 = -1;
        //public int id_ora_23_temp { get; set; } //TODO:
        //public int id_oracle { get; set; }     //TODO:
        public int Num { get { return num; } set { num = value; } }

        //public Station StationFrom { get { return stationFrom; } set { stationFrom = value; } }

        //public GruzFront GruzFront { get { return gruzFront; } set { gruzFront = value; } }

        //public Shop Shop { get { return shop; } set { shop = value; } }
        public SendingPoint SendingPoint { get { return sendingPoint; } set { sendingPoint = value; } }

        public Station StationTo { get { return stationTo; } set { stationTo = value; } }

        public DateTime DateFromStat { get { return dateFromStat; } set { dateFromStat = value; } }

        public int Vag_amount { get { return vag_amount; } set { vag_amount = value; } }

        public int St_lock_locom1
        {
            get { return st_lock_locom1; }
            set { st_lock_locom1 = value; }
        }

        public int St_lock_locom2
        {
            get { return st_lock_locom2; }
            set { st_lock_locom2 = value; }
        }

    }
}
