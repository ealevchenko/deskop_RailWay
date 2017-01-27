using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using EFRailCars.Railcars;
using EFRailCars.Helpers;

namespace RailwayCL
{
    public abstract class DB
    {
        private dbConnection conn;
        //protected RC_VagonsOperations rc_vo = new RC_VagonsOperations();


        public DB()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Railway"].ConnectionString;
            conn = new dbConnection(connectionString);
        }

        public dbConnection Conn { get { return conn; } }
    }
}
