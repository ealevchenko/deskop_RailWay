using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace RailwayCL
{
    public class VagWaitRemoveAdmissDB : VagOperationsDB // вагоны, ожидающие снятия зачисления
    {
        //private static VagWaitRemoveAdmissDB vagWaitRemoveAdmissDB;

        //private VagWaitRemoveAdmissDB() { }

        //public static VagWaitRemoveAdmissDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (vagWaitRemoveAdmissDB == null)
        //    {
        //        lock (typeof(VagWaitRemoveAdmissDB))
        //        {
        //            if (vagWaitRemoveAdmissDB == null)
        //                vagWaitRemoveAdmissDB = new VagWaitRemoveAdmissDB();
        //        }
        //    }

        //    return vagWaitRemoveAdmissDB;
        //}

        private DataTable getTrainsTable(Station stat)
        {
            string query = "[RailCars].[GetRemoveTrains]";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@idstation", stat.ID);
            return Conn.executeProc(query, sqlParameters).Tables[0];
        }
        //private DataTable getTrainsTable(Station stat)
        //{
        //    string query = string.Format(
        //        "select vo.st_lock_id_stat, s.name as stat, vo.st_lock_id_stat, vo.st_lock_train, "+
        //        "max(FORMAT(vo.dt_from_stat, 'yyyy-MM-dd HH:mm:ss')) AS dt_from_stat, count(vo.id_vagon) as vag_amount "+
        //        "from VAGON_OPERATIONS vo inner join STATIONS s on vo.st_lock_id_stat=s.id_stat "+
        //        "where vo.id_stat=@id_stat and vo.is_present=0 and vo.is_hist=0 "+
        //        "and ((vo.st_shop = -1 and vo.st_gruz_front = -1) or (vo.st_shop is null and vo.st_gruz_front is null)) "+
        //        "and ((s.is_uz !=1) or (vo.dt_from_stat>GETDATE()-1)) "+
        //        "group by vo.id_stat, s.name, vo.st_lock_id_stat, vo.st_lock_train, FORMAT(vo.dt_from_stat, 'yyyy-MM-dd HH:mm:ss') "+
        //        "order by dt_from_stat");
        //    SqlParameter[] sqlParameters = new SqlParameter[1];
        //    sqlParameters[0] = new SqlParameter("@id_stat", stat.ID);
        //    return Conn.executeSelectQuery(query, sqlParameters).Tables[0];
        //}

        public List<Train> getTrains(Station stat)
        {
            DataTable table = getTrainsTable(stat);

            List<Train> list = new List<Train>();

            foreach (DataRow row in table.Rows)
            {
                Train train = new Train();
                train.Num = Int32.Parse(row["st_lock_train"].ToString());

                Station stationTo = new Station();
                stationTo.ID = Int32.Parse(row["st_lock_id_stat"].ToString());
                stationTo.Name = "ст. " + row["stat"].ToString().Trim();
                train.StationTo = stationTo;
                train.SendingPoint = stat;

                train.DateFromStat = DateTime.Parse(row["dt_from_stat"].ToString());
                train.Vag_amount = Int32.Parse(row["vag_amount"].ToString());
                list.Add(train);
            }

            return list;
        }

        public new Tuple<DateTime, int> findVagLocation(Station stat, int num)
        {
            string query = string.Format("select vo.dt_from_stat, vo.st_lock_order " +
                "from VAGON_OPERATIONS vo " +
                "inner join VAGONS v on vo.id_vagon=v.id_vag " +
                "inner join STATIONS s on vo.st_lock_id_stat=s.id_stat "+
                "and v.num=@num and vo.id_stat=@id_stat and vo.is_present=0 and vo.is_hist=0 "+
                "and ((vo.st_shop = -1 and vo.st_gruz_front = -1) or (vo.st_shop is null and vo.st_gruz_front is null)) "+
                "and ((s.is_uz !=1) or (vo.dt_from_stat>GETDATE()-1)) ");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@num", num);
            sqlParameters[1] = new SqlParameter("@id_stat", stat.ID);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];

            Tuple<DateTime, int> tuple = new Tuple<DateTime, int>(DateTime.MinValue, -1);
            if (table.Rows.Count > 0)
            {
                if (table.Rows[0]["dt_from_stat"] != null && table.Rows[0]["st_lock_order"] != null)
                    tuple = new Tuple<DateTime, int>(DateTime.Parse(table.Rows[0]["dt_from_stat"].ToString()), Int32.Parse(table.Rows[0]["st_lock_order"].ToString()));
            }
            return tuple;
        }

        private DataTable getVagonsTable(Train train, Station stat)
        {
            string query = "[RailCars].[GetRemoveWagons]";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@idstation", stat.ID);
            sqlParameters[1] = new SqlParameter("@dt", train.DateFromStat);
            return Conn.executeProc(query, sqlParameters).Tables[0];
        }
        //private DataTable getVagonsTable(Train train, Station stat)
        //{
        //    string query = string.Format("select vo.*, o.abr as owner_, " +
        //    "c.name as country, vc.name as cond, vc2.name as cond2, vc2.id_cond_after, " +
        //    "g.name as gruz, g2.name as gruz_amkr, v.num, v.rod, v.st_otpr, s.name as shop, t.name as tupik, gd.name as gdstait, nc.name as nazn_country, " +
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
        //    "left join v_p_vozvrat_ip p on p.id = (select top 1 id from v_P_VOZVRAT_IP where n_vag=v.num order by DATE_MAIL desc) "+
        //    "where vo.id_stat=@id_stat and vo.is_present=0 " + 
        //    "and CAST(FORMAT(vo.dt_from_stat,'yyyy-MM-dd HH:mm:ss') AS datetime) " +
        //    "=CAST(FORMAT(@dt,'yyyy-MM-dd HH:mm:ss') as datetime) "+
        //    "order by CAST(FORMAT(vo.dt_from_stat,'yyyy-MM-dd HH:mm:ss') AS datetime), vo.st_lock_order ");
        //    SqlParameter[] sqlParameters = new SqlParameter[2];
        //    sqlParameters[0] = new SqlParameter("@dt", train.DateFromStat);
        //    sqlParameters[1] = new SqlParameter("@id_stat", stat.ID);

        //    return Conn.executeSelectQuery(query, sqlParameters).Tables[0];
        //}

        public List<VagWaitRemoveAdmiss> getVagons(Train train, Station stat)
        {
            DataTable table = getVagonsTable(train, stat);

            List<VagWaitRemoveAdmiss> list = (base.getVagons(table)).Select(parent => new VagWaitRemoveAdmiss(parent)).ToList();

            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                list[i].num_vag_on_way = table.Rows.IndexOf(row) + 1;

                i++;
            }

            return list;
        }

        public bool cancelTrainSending(Train train, Station stat, List<VagWaitRemoveAdmiss> vagons)
        {
            string query = string.Format("update VAGON_OPERATIONS set st_lock_id_stat = null, st_lock_order = null," +
                "st_lock_train = null, st_lock_side = null, st_lock_locom1 = null, st_lock_locom2 = null, st_shop = null, st_gruz_front = null, " +
                "is_present=1, dt_from_stat = null, dt_from_way = null " +
                "where id_stat=@id_stat and is_present=0 and is_hist=0 " +
                "and ((st_shop = -1 and st_gruz_front = -1) or (st_shop is null and st_gruz_front is null)) "+
                "and CAST(FORMAT(dt_from_stat,'yyyy-MM-dd HH:mm:ss') AS datetime)=CAST(FORMAT(@dt,'yyyy-MM-dd HH:mm:ss') as datetime)");
            if (vagons.Count > 0)
            {
                query += " and id_oper in (";
                foreach (VagWaitRemoveAdmiss vag in vagons)
                {
                    if (vagons.IndexOf(vag) == vagons.Count - 1)
                        query += vag.id_oper.ToString() + ")";
                    else query += vag.id_oper.ToString() + ",";
                }
            }
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@id_stat", stat.ID);
            if (train.DateFromStat < DateTime.Parse("1900-01-01 00:00"))
                sqlParameters[1] = new SqlParameter("@dt", DBNull.Value);
            else sqlParameters[1] = new SqlParameter("@dt", train.DateFromStat);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }

    }
}
