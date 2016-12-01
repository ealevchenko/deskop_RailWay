using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RailwayCL
{
    public class GruzFrontUtils
    {
        private static GruzFrontUtils gruzFrontUtils;
        private string cbValue = "Id";
        private string cbDisplay = "Name";
        private string cbNonSelected = "ВЫБЕРИТЕ ";
        private string className = "GruzFront";

        private GruzFrontUtils()
        {

        }

        public static GruzFrontUtils GetInstance()
        {
            // для исключения возможности создания двух объектов 
            // при многопоточном приложении
            if (gruzFrontUtils == null)
            {
                lock (typeof(GruzFrontUtils))
                {
                    if (gruzFrontUtils == null)
                        gruzFrontUtils = new GruzFrontUtils();
                }
            }

            return gruzFrontUtils;
        }

        public string CbValue { get { return cbValue; } }

        public string CbDisplay { get { return cbDisplay; } }

        public string CbNonSelected { get { return cbNonSelected; } }

        public string ClassName { get { return className; } set { className = value; } }
    }
}
