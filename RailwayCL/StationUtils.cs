using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RailwayCL
{
    public class StationUtils
    {
        private static StationUtils stationUtils;
        private string cbValue = "id";
        private string cbDisplay = "name";
        private string cbNonSelected = "ВЫБЕРИТЕ СТАНЦИЮ";
        private string className = "Station";

        private StationUtils()
        {

        }

        public static StationUtils GetInstance()
        {
            // для исключения возможности создания двух объектов 
            // при многопоточном приложении
            if (stationUtils == null)
            {
                lock (typeof(StationUtils))
                {
                    if (stationUtils == null)
                        stationUtils = new StationUtils();
                }
            }

            return stationUtils;
        }

        public string CbValue { get { return cbValue; } }

        public string CbDisplay { get { return cbDisplay; } }

        public string CbNonSelected { get { return cbNonSelected; } }

        public string ClassName { get { return className; } set { className = value; } }
    }
}
