using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RailwayCL
{
    public class ShopUtils
    {
        private static ShopUtils shopUtils;
        private string cbValue = "Id";
        private string cbDisplay = "Name";
        private string cbNonSelected = "ВЫБЕРИТЕ ";
        private string className = "Shop";

        private ShopUtils()
        {

        }

        public static ShopUtils GetInstance()
        {
            // для исключения возможности создания двух объектов 
            // при многопоточном приложении
            if (shopUtils == null)
            {
                lock (typeof(ShopUtils))
                {
                    if (shopUtils == null)
                        shopUtils = new ShopUtils();
                }
            }

            return shopUtils;
        }

        public string CbValue { get { return cbValue; } }

        public string CbDisplay { get { return cbDisplay; } }

        public string CbNonSelected { get { return cbNonSelected; } }

        public string ClassName { get { return className; } set { className = value; } }
    }
}
