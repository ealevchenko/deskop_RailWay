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
    public class GruzFrontDB : DB
    {
        //private static GruzFrontDB gruzFrontDB;

        //private GruzFrontDB() { }

        //public static GruzFrontDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (gruzFrontDB == null)
        //    {
        //        lock (typeof(GruzFrontDB))
        //        {
        //            if (gruzFrontDB == null)
        //                gruzFrontDB = new GruzFrontDB();
        //        }
        //    }

        //    return gruzFrontDB;
        //}

        public List<GruzFront> getGruzFronts(Station stat)
        {
            List<GruzFront> list = new List<GruzFront>();

            string query = string.Format("select g.id_gruz_front, g.name, count(vo.id_oper) as vag_amount "+
            "from GRUZ_FRONTS g left join VAGON_OPERATIONS vo "+
            "on g.id_gruz_front=vo.st_gruz_front and vo.is_hist=0 and vo.is_present=0 "+
            "where g.id_stat=@id_stat "+
            "group by id_gruz_front, name "+
            "order by name");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_stat", stat.ID);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];

            foreach (DataRow row in table.Rows)
            {
                GruzFront gf = new GruzFront { ID = Int32.Parse(row["id_gruz_front"].ToString()), Name = row["name"].ToString().Trim(),
                    Stat=stat, Vag_amount=Int32.Parse(row["vag_amount"].ToString())};

                list.Add(gf);
            }

            return list;
        }
    }
}
