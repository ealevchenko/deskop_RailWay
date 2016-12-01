using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;

namespace RailwayCL
{
    public class LocomotiveDB : DB
    {
        //private static LocomotiveDB locomotiveDB;

        //private LocomotiveDB() { }

        //public static LocomotiveDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (locomotiveDB == null)
        //    {
        //        lock (typeof(LocomotiveDB))
        //        {
        //            if (locomotiveDB == null)
        //                locomotiveDB = new LocomotiveDB();
        //        }
        //    }

        //    return locomotiveDB;
        //}

        public List<Locomotive> getLocomotives()
        {
            List<Locomotive> list = new List<Locomotive>();

            string query = string.Format("select id_vag, num, locom_seria, id_stat from VAGONS where is_locom=1 order by num");
            DataTable table = Conn.executeSelectQuery(query, new SqlParameter[0]).Tables[0];

            foreach (DataRow row in table.Rows)
            {
                Locomotive locom = new Locomotive();
                locom.ID = Int32.Parse(row["id_vag"].ToString());
                locom.Num_seria = row["num"].ToString().Trim() + " " + row["locom_seria"].ToString().Trim();
                if (row["id_stat"] != DBNull.Value) locom.Stat.ID = Int32.Parse(row["id_stat"].ToString());
                list.Add(locom);
            }

            return list;
        }

        public List<Locomotive> getLocomotives(Station stat)
        {
            List<Locomotive> list = new List<Locomotive>();

            string query = string.Format("select id_vag, num, locom_seria from VAGONS where id_stat=@id_stat and is_locom=1 order by num");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_stat", stat.ID);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];

            foreach (DataRow row in table.Rows)
            {
                Locomotive locom = new Locomotive();
                locom.ID = Int32.Parse(row["id_vag"].ToString());
                locom.Num_seria = row["num"].ToString().Trim() + " " + row["locom_seria"].ToString().Trim() ;
                locom.Stat = stat;
                list.Add(locom);
            }

            return list;
        }
    }
}
