using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace RailwayCL
{
    public class CondDB : DB
    {
        //private static CondDB condDB;

        //private CondDB() { }

        //public static CondDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (condDB == null)
        //    {
        //        lock (typeof(CondDB))
        //        {
        //            if (condDB == null)
        //                condDB = new CondDB();
        //        }
        //    }

        //    return condDB;
        //}

        public Cond getCondById(int id)
        {
            string query = string.Format("select name, id_cond_after from VAG_CONDITIONS2 where id_cond=@id");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id", id);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];

            Cond cond = new Cond();
            if (table.Rows.Count > 0)
            {
                cond.Id = id;
                if (table.Rows[0]["Name"] != DBNull.Value) cond.Name = table.Rows[0]["Name"].ToString().Trim();
                if (table.Rows[0]["id_cond_after"] != DBNull.Value) cond.Id_cond_after = Int32.Parse(table.Rows[0]["id_cond_after"].ToString());
            }

            return cond;
        }
    }
}
