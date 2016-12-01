using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{ 
    public class SideUtils
    {
        private static SideUtils sideUtils;
        private string cbNonSelected = "ВЫБЕРИТЕ ГОРЛОВИНУ";
        private object[] cbItems = {"с Четной горловины", "с Нечетной горловины"};

        private SideUtils()
        {

        }

        public static SideUtils GetInstance()
        {
            // для исключения возможности создания двух объектов 
            // при многопоточном приложении
            if (sideUtils == null)
            {
                lock (typeof(SideUtils))
                {
                    if (sideUtils == null)
                        sideUtils = new SideUtils();
                }
            }

            return sideUtils;
        }

        public string CbNonSelected { get { return cbNonSelected; } }

        public object[] CbItems { get { return cbItems; } }
    }
}
