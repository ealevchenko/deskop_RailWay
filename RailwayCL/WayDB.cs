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
    public class WayDB : DB
    {
        //private static WayDB wayDB;

        //private WayDB() { }

        //public static WayDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (wayDB == null)
        //    {
        //        lock (typeof(WayDB))
        //        {
        //            if (wayDB == null)
        //                wayDB = new WayDB();
        //        }
        //    }

        //    return wayDB;
        //}

        public List<Way> getWays(Station stat, bool rospusk)
        {
            List<Way> list = new List<Way>();
            string str = "";
            if (rospusk) str = "and w.for_rospusk<>0 ";

            string query = string.Format("select w.id_way, w.num, w.name, w.vag_capacity, "+
                "w.bind_id_cond, count(v.id_oper) as vag_amount, vc.name as cond_name, vc.id_cond_after "+
                "from WAYS w left join VAGON_OPERATIONS v on w.id_way=v.id_way and v.is_present=1 " +
                "left join VAG_CONDITIONS2 vc on w.bind_id_cond=vc.id_cond "+
                "where w.id_stat=@id_stat "+str+
                "group by w.id_way, w.num, w.name, w.vag_capacity, w.[order], w.bind_id_cond, vc.name, vc.id_cond_after "+
                "order by w.[order]");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_stat", stat.ID);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];

            foreach (DataRow row in table.Rows)
            {
                Way way = new Way();
                way.ID = Int32.Parse(row["id_way"].ToString());
                way.Stat = stat;
                if (row["num"] != DBNull.Value) way.Num = row["num"].ToString().Trim();
                if (row["name"] != DBNull.Value) way.Name = row["name"].ToString().Trim();
                if (row["vag_capacity"] != DBNull.Value) way.Capacity = Int32.Parse(row["vag_capacity"].ToString());
                way.Vag_amount = Int32.Parse(row["vag_amount"].ToString());
                way.NumName = way.Num + " - " + way.Name;
                if (row["bind_id_cond"] != DBNull.Value) way.Bind_cond.Id = Int32.Parse(row["bind_id_cond"].ToString());
                if (row["cond_name"] != DBNull.Value) way.Bind_cond.Name = row["cond_name"].ToString().Trim();
                if (row["id_cond_after"] != DBNull.Value) way.Bind_cond.Id_cond_after = Int32.Parse(row["id_cond_after"].ToString());
                list.Add(way);
            }

            return list;
        }

        public Way getWayByIdOper(int id_oper)
        {
            string query = string.Format("select id_way from VAGON_OPERATIONS where id_oper=@id_oper");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_oper", id_oper);
            int id_way = Int32.Parse(Conn.executeSelectQuery(query, sqlParameters).Tables[0].Rows[0]["id_way"].ToString());
            return new Way() { ID = id_way };
        }
    }
}
