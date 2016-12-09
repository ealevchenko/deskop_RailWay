using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

namespace RailwayCL
{
    public class VagManeuverDB : VagOperationsDB
    {
        //private static VagManeuverDB vagOperationsDB;

        //private VagManeuverDB() { }

        //public static VagManeuverDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (vagOperationsDB == null)
        //    {
        //        lock (typeof(VagManeuverDB))
        //        {
        //            if (vagOperationsDB == null)
        //                vagOperationsDB = new VagManeuverDB();
        //        }
        //    }

        //    return vagOperationsDB;
        //}

        private DataTable getVagonsTable(Way way, Side side)
        {
            string query = "[RailCars].[GetManeuverWagons]";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@idway", way.ID);
            sqlParameters[1] = new SqlParameter("@side", way.Stat.Outer_side == side ? 1 : 0);
            return Conn.executeProc(query, sqlParameters).Tables[0];
        }
        //private DataTable getVagonsTable(Way way, Side side)
        //{
        //    string str = "";
        //    if (way.Stat.Outer_side == side) str = "desc";

        //    string query = string.Format("select vo.*, o.abr as owner_, c.name as country, vc.name as cond,  " +
        //    "g.name as gruz, g2.name as gruz_amkr, v.num, v.rod, v.st_otpr, s.name as shop, t.name as tupik, gd.name as gdstait, nc.name as nazn_country, vc2.name as cond2, vc2.id_cond_after, " +
        //    "p.date_mail, p.n_mail, p.[text], p.nm_stan, p.nm_sobstv " +
        //    "from VAGON_OPERATIONS vo " +
        //    "inner join VAGONS v on vo.id_vagon=v.id_vag " +
        //    "left join OWNERS o on v.id_owner=o.id_owner " +
        //    "left join OWNERS_COUNTRIES c on o.id_country=c.id_own_country " +
        //    "left join VAG_CONDITIONS vc on vo.id_cond=vc.id_cond " +
        //    "left join GRUZS g on vo.id_gruz=g.id_gruz " +
        //    "left join GRUZS g2 on vo.id_gruz_amkr=g2.id_gruz " +
        //    "left join SHOPS s on vo.id_shop_gruz_for=s.id_shop " +
        //    "left join TUPIKI t on vo.id_tupik = t.id_tupik " +
        //    "left join GDSTAIT gd on vo.id_gdstait = gd.id_gdstait " +
        //    "left join NAZN_COUNTRIES nc on vo.id_nazn_country = nc.id_country " +
        //    "left join VAG_CONDITIONS2 vc2 on vo.id_cond2=vc2.id_cond " +
        //    "left join v_p_vozvrat_ip p on p.id = (select top 1 id from v_P_VOZVRAT_IP where n_vag=v.num order by DATE_MAIL desc) " +
        //    "where vo.id_way = @id_way and vo.is_present = 1 " +
        //    "order by vo.num_vag_on_way " + str);
        //    SqlParameter[] sqlParameters = new SqlParameter[1]; 
        //    sqlParameters[0] = new SqlParameter("@id_way", way.ID);

        //    return Conn.executeSelectQuery(query, sqlParameters).Tables[0];
        //}

        public List<VagManeuver> getVagons(Way way, Side side)
        {
            DataTable table = getVagonsTable(way, side);
            List<VagManeuver> list = (base.getVagons(table)).Select(parent => new VagManeuver(parent)).ToList();

            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                if (row["dt_on_stat"] != DBNull.Value) list[i].dt_on_stat = DateTime.Parse(row["dt_on_stat"].ToString());
                if (row["dt_from_way"] != DBNull.Value) list[i].dt_from_way = DateTime.Parse(row["dt_from_way"].ToString());
                if (row["lock_id_way"] != DBNull.Value) list[i].Lock_id_way = Int32.Parse(row["lock_id_way"].ToString());
                if (row["lock_order"] != DBNull.Value) list[i].Lock_order = Int32.Parse(row["lock_order"].ToString());
                if (row["lock_side"] != DBNull.Value) list[i].Lock_side = (Side)Int32.Parse(row["lock_side"].ToString());
                if (row["lock_id_locom"] != DBNull.Value) list[i].Lock_id_locom = Int32.Parse(row["lock_id_locom"].ToString());
                i++;
            }

            return list;
        }

        public bool addOnManeuver(VagManeuver vagOnStat)
        {
            string query = string.Format("update VAGON_OPERATIONS set lock_id_way = @id_way, lock_order = @order, " +
            "lock_side = @lock_side, lock_id_locom = @lock_id_locom, dt_from_way = @dt_from_way where id_oper = @id_oper");
            SqlParameter[] sqlParameters = new SqlParameter[6];
            if (vagOnStat.Lock_id_way == -1)
                sqlParameters[0] = new SqlParameter("@id_way", DBNull.Value);
            else sqlParameters[0] = new SqlParameter("@id_way", vagOnStat.Lock_id_way);
            sqlParameters[1] = new SqlParameter("@order", vagOnStat.Lock_order);
            sqlParameters[2] = new SqlParameter("@lock_side", vagOnStat.Lock_side);
            sqlParameters[3] = new SqlParameter("@lock_id_locom", vagOnStat.Lock_id_locom);
            if (vagOnStat.dt_from_way < DateTime.Parse("1900-01-01 00:00"))
                sqlParameters[4] = new SqlParameter("@dt_from_way", DBNull.Value);
            else sqlParameters[4] = new SqlParameter("@dt_from_way", vagOnStat.dt_from_way);
            sqlParameters[5] = new SqlParameter("@id_oper", vagOnStat.id_oper);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }

        public bool cancelManeuver(Way way)
        {
            string query = string.Format("update VAGON_OPERATIONS set lock_id_way = null, lock_order = null," +
                "dt_from_way = null, lock_side = null, lock_id_locom = null where id_way = @id_way and lock_order is not null");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_way", way.ID);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }

        public bool cancelVagOnMan(int id_oper)
        {
            string query = string.Format("update VAGON_OPERATIONS set lock_id_way = null, lock_order = null,") +
                "dt_from_way = null, lock_side = null, lock_id_locom = null where id_oper = @id_oper";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_oper", id_oper);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }
        /// <summary>
        /// Выполнить маневр на станции
        /// </summary>
        /// <param name="vagManeuver"></param>
        /// <param name="way"></param>
        /// <returns></returns>
        public int execManeuver(VagManeuver vagManeuver, Way way)
        {
            string query = string.Format("RailCars.ExecManuever_");
            SqlParameter[] sqlParameters = base.paramsForInsert((VagOperations)vagManeuver, way, 4);

            int i = sqlParameters.Length;

            //if (way.Bind_cond.Id == -1) 
            //{
            //    if (vagManeuver.cond.Id == -1)
            //        sqlParameters[i - 2] = new SqlParameter("@id_cond2", DBNull.Value);
            //    else sqlParameters[i - 2] = new SqlParameter("@id_cond2", vagManeuver.cond.Id);
            //}
            //else sqlParameters[i - 2] = new SqlParameter("@id_cond2", way.Bind_cond.Id);
            if (vagManeuver.dt_on_stat < DateTime.Parse("1900-01-01 00:00"))
                sqlParameters[i - 4] = new SqlParameter("@dt_on_stat", DBNull.Value);
            else sqlParameters[i - 4] = new SqlParameter("@dt_on_stat", vagManeuver.dt_on_stat);
            if (vagManeuver.cond.Id == -1) sqlParameters[i - 3] = new SqlParameter("@id_cond2", DBNull.Value);
            else sqlParameters[i - 3] = new SqlParameter("@id_cond2", vagManeuver.cond.Id);
            if (vagManeuver.Lock_id_locom == -1) sqlParameters[i-2] = new SqlParameter("@id_locom", DBNull.Value);
            else sqlParameters[i-2] = new SqlParameter("@id_locom", vagManeuver.Lock_id_locom);
            sqlParameters[i - 1] = new SqlParameter("@new_identity", SqlDbType.Int);
            sqlParameters[i - 1].Direction = ParameterDirection.Output;

            Conn.executeScalarCommand(query, sqlParameters, true);
            if (sqlParameters[i - 1].Value != null)
                return int.Parse(sqlParameters[i - 1].Value.ToString());
            else return -1;
        }

        //public Tuple<int, int> findVagLocation(Station stat, int num)
        //{
        //    string query = string.Format("select vo.id_way, vo.num_vag_on_way "+
        //        "from VAGON_OPERATIONS vo "+
        //        "inner join VAGONS v on vo.id_vagon=v.id_vag "+
        //        "and v.num=@num and vo.is_present=1 "+
        //        "and vo.is_hist=0 and vo.id_stat=@id_stat");
        //    SqlParameter[] sqlParameters = new SqlParameter[2];
        //    sqlParameters[0] = new SqlParameter("@num", num);
        //    sqlParameters[1] = new SqlParameter("@id_stat", stat.ID);
        //    DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];

        //    Tuple<int, int> tuple = new Tuple<int,int>(0, 0);
        //    if (table.Rows.Count > 0 && table.Rows[0]["id_way"] != null && table.Rows[0]["num_vag_on_way"] != null)
        //        tuple = new Tuple<int, int>(Int32.Parse(table.Rows[0]["id_way"].ToString()), Int32.Parse(table.Rows[0]["num_vag_on_way"].ToString()));
        //    return tuple;
        //}
    }
}
