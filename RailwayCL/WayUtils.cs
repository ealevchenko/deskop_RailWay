using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RailwayCL
{
    public class WayUtils
    {
        private static WayUtils wayUtils;
        private string cbValue = "Id";
        private string cbDisplay = "NumName";

        private WayUtils()
        {

        }

        public static WayUtils GetInstance()
        {
            // для исключения возможности создания двух объектов 
            // при многопоточном приложении
            if (wayUtils == null)
            {
                lock (typeof(WayUtils))
                {
                    if (wayUtils == null)
                        wayUtils = new WayUtils();
                }
            }

            return wayUtils;
        }

        public string CbValue { get { return cbValue; } }

        public string CbDisplay { get { return cbDisplay; } }
    }
}
