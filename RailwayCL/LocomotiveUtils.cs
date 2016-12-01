using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RailwayCL
{
    public class LocomotiveUtils
    {
        private static LocomotiveUtils locomotiveUtils;
        private string cbValue = "Id";
        private string cbDisplay = "Num_seria";
        private string cbNonSelected = "ВЫБЕРИТЕ ";

        private LocomotiveUtils()
        {

        }

        public static LocomotiveUtils GetInstance()
        {
            // для исключения возможности создания двух объектов 
            // при многопоточном приложении
            if (locomotiveUtils == null)
            {
                lock (typeof(LocomotiveUtils))
                {
                    if (locomotiveUtils == null)
                        locomotiveUtils = new LocomotiveUtils();
                }
            }

            return locomotiveUtils;
        }

        public string CbValue { get { return cbValue; } }

        public string CbDisplay { get { return cbDisplay; } }

        public string CbNonSelected { get { return cbNonSelected; } }
    }
}
