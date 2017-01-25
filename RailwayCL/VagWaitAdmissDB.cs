using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace RailwayCL
{
    public class VagWaitAdmissDB : VagOperationsDB // вагоны, ожидающие зачисления
    {
        //private static VagWaitAdmissDB vagWaitAdmissDB;

        //private VagWaitAdmissDB() { }

        //public static VagWaitAdmissDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (vagWaitAdmissDB == null)
        //    {
        //        lock (typeof(VagWaitAdmissDB))
        //        {
        //            if (vagWaitAdmissDB == null)
        //                vagWaitAdmissDB = new VagWaitAdmissDB();
        //        }
        //    }

        //    return vagWaitAdmissDB;
        //}
        /// <summary>
        /// Получить таблицу вагонов состава
        /// </summary>
        /// <param name="train"></param>
        /// <param name="stat"></param>
        /// <param name="side"></param>
        /// <param name="isGF"></param>
        /// <param name="isCeh"></param>
        /// <returns></returns>
        private DataTable getVagonsTable(Train train, Station stat, Side side, bool isGF, bool isCeh)
        {
            string query = "[RailCars].[GetAdmissWagons]";
            SqlParameter[] sqlParameters = new SqlParameter[6];
            sqlParameters[0] = new SqlParameter("@idstation", stat.ID);
            sqlParameters[1] = new SqlParameter("@trainNum", !isGF & !isCeh ? train.Num : -1);
            sqlParameters[2] = new SqlParameter("@dt", train.DateFromStat);
            sqlParameters[3] = new SqlParameter("@shop", isCeh ? train.SendingPoint.ID : SqlInt32.Null);
            sqlParameters[4] = new SqlParameter("@gf", isGF ? train.SendingPoint.ID : SqlInt32.Null);
            sqlParameters[5] = new SqlParameter("@side", isCeh && stat.ID != 17 ? 1 : 0);
            return Conn.executeProc(query, sqlParameters).Tables[0];
            //string str = "";
            //if (/*stat.Outer_side == side || */isCeh && stat.ID != 17) str = "desc";

            //SqlParameter[] sqlParameters = new SqlParameter[3];
            //string strWhere = "";
            //if (isGF/*train.SendingPoint.GetType().IsAssignableFrom(typeof(GruzFront))*/)
            //{
            //    strWhere = " and vo.st_gruz_front=@gr_front ";
            //    sqlParameters[0] = new SqlParameter("@gr_front", train.SendingPoint.ID);
            //}
            //else if (isCeh/*train.SendingPoint.GetType().IsAssignableFrom(typeof(Shop))*/)
            //{
            //    strWhere = " and vo.st_shop=@st_shop ";
            //    sqlParameters[0] = new SqlParameter("@st_shop", train.SendingPoint.ID);
            //}
            //else
            //{
            //    strWhere = " and CAST(FORMAT(vo.dt_from_stat,'yyyy-MM-dd HH:mm:ss') AS datetime) " +
            //"=CAST(FORMAT(@dt,'yyyy-MM-dd HH:mm:ss') as datetime) ";
            //    sqlParameters[0] = new SqlParameter("@dt", train.DateFromStat);
            //}

            //string query = string.Format("select vo.*, o.abr as owner_, "+
            //"c.name as country, vc.name as cond, vc2.name as cond2, vc2.id_cond_after, " +
            //"g.name as gruz, g2.name as gruz_amkr, v.num, v.rod, v.st_otpr, s.name as shop, t.name as tupik, gd.name as gdstait, nc.name as nazn_country, " +
            //"p.date_mail, p.n_mail, p.[text], p.nm_stan, p.nm_sobstv " +
            //"from VAGON_OPERATIONS vo "+
            //"inner join VAGONS v on vo.id_vagon=v.id_vag "+
            //"left join OWNERS o on v.id_owner=o.id_owner "+
            //"left join OWNERS_COUNTRIES c on o.id_country=c.id_own_country " +
            //"left join VAG_CONDITIONS vc on vo.id_cond=vc.id_cond " +
            //"left join GRUZS g on vo.id_gruz=g.id_gruz " +
            //"left join GRUZS g2 on vo.id_gruz_amkr=g2.id_gruz " +
            //"left join SHOPS s on vo.id_shop_gruz_for=s.id_shop " +
            //"left join TUPIKI t on vo.id_tupik = t.id_tupik " +
            //"left join GDSTAIT gd on vo.id_gdstait = gd.id_gdstait " +
            //"left join NAZN_COUNTRIES nc on vo.id_nazn_country = nc.id_country " +
            //"left join VAG_CONDITIONS2 vc2 on vo.id_cond2=vc2.id_cond " +
            //"left join v_p_vozvrat_ip p on p.id = (select top 1 id from v_P_VOZVRAT_IP where n_vag=v.num order by DATE_MAIL desc) "+
            //"where vo.st_lock_id_stat=@id_stat and vo.is_present=0 and vo.is_hist=0 " + strWhere +
            //" and vo.st_lock_train = @trainNum " +
            //"order by CAST(FORMAT(vo.dt_from_way,'yyyy-MM-dd HH:mm:ss') AS datetime), vo.st_lock_order " + str);
            //sqlParameters[1] = new SqlParameter("@id_stat", stat.ID);
            //sqlParameters[2] = new SqlParameter("@trainNum", train.Num);

            //return Conn.executeSelectQuery(query, sqlParameters).Tables[0];
        }
        /// <summary>
        /// Получить список поездов (!! временное условие "не показывать поезда отправленные на Прокатную-1 с пом. ПО, а только из КИС)
        /// </summary>
        /// <param name="stat">Станция прибытия</param>
        /// <param name="sp">Класс данных отправки (станция, вагоноопрокид, цех)</param>
        /// <returns></returns>
        private DataTable getTrainsTable(Station stat, SendingPoint sp)
        {
            int type = -1;
            if (sp.GetType().IsAssignableFrom(typeof(Station)))
            {
                type = 0;   // станция
            }
            else if (sp.GetType().IsAssignableFrom(typeof(GruzFront)))
            {
                type = 1;   // Вагоноопрокид
            }
            else
            {
                type = 2;   // Цех
            }
            string query = "[RailCars].[GetAdmissTrains]";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@idstation", stat.ID);
            sqlParameters[1] = new SqlParameter("@type", type);
            return Conn.executeProc(query, sqlParameters).Tables[0];
        }
        //private DataTable getTrainsTable(Station stat, SendingPoint sp)
        //{
        //    //string query = string.Format(
        //    //    "select vo.id_stat, s.name as stat, vo.st_lock_id_stat, vo.st_lock_train, " +
        //    //    "max(FORMAT(vo.dt_from_stat, 'yyyy-MM-dd HH:mm:ss')) AS dt_from_stat, count(vo.id_vagon) as vag_amount, " +
        //    //    "vo.st_gruz_front, g.name as gruz_front_name, vo.st_shop, sh.name as shop_name " +
        //    //    "from VAGON_OPERATIONS vo inner join STATIONS s on vo.id_stat=s.id_stat " +
        //    //    "left join GRUZ_FRONTS g on vo.st_gruz_front=g.id_gruz_front " +
        //    //    "left join SHOPS sh on vo.st_shop=sh.id_shop " +
        //    //    "where vo.st_lock_id_stat=@id_stat and vo.is_present=0 and vo.is_hist=0 " +
        //    //    "group by vo.id_stat, s.name, vo.st_lock_id_stat, vo.st_lock_train, " +
        //    //    "    case when (vo.st_shop = -1 and vo.st_gruz_front = -1) or (vo.st_shop is null and vo.st_gruz_front is null) then FORMAT(vo.dt_from_stat, 'yyyy-MM-dd HH:mm:ss') end, " +
        //    //    "vo.st_gruz_front, g.name, vo.st_shop, sh.name " +
        //    //    "order by dt_from_stat");
        //    string query = "";

        //    if (sp.GetType().IsAssignableFrom(typeof(Station)))
        //    {
        //        int type = 0;
        //        // Если станция
        //        query = "[dbo].[rw_GetTrains]";
        //        SqlParameter[] sqlParameters1 = new SqlParameter[2];
        //        sqlParameters1[0] = new SqlParameter("@idstation", stat.ID);
        //        sqlParameters1[1] = new SqlParameter("@type", type);
        //        return Conn.executeProc(query, sqlParameters1).Tables[0];
                
        //        //query = string.Format(
        //        //    "select vo.id_stat, s.name as stat, vo.st_lock_id_stat, vo.st_lock_train, " +
        //        //    "max(FORMAT(vo.dt_from_stat, 'yyyy-MM-dd HH:mm:ss')) AS dt_from_stat, count(vo.id_vagon) as vag_amount, "+
        //        //    "vo.st_gruz_front, g.name as gruz_front_name, vo.st_shop, sh.name as shop_name, vo.st_lock_locom1, vo.st_lock_locom2 "+
        //        //    "from VAGON_OPERATIONS vo inner join STATIONS s on vo.id_stat=s.id_stat "+
        //        //    "left join GRUZ_FRONTS g on vo.st_gruz_front=g.id_gruz_front "+
        //        //    "left join SHOPS sh on vo.st_shop=sh.id_shop "+
        //        //    "where vo.st_lock_id_stat=@id_stat and vo.is_present=0 and vo.is_hist=0 "+
        //        //    "and ((vo.st_shop = -1 and vo.st_gruz_front = -1) or (vo.st_shop is null and vo.st_gruz_front is null)) "+
        //        //    "and vo.st_lock_train = case when @id_stat=17 then -1 else vo.st_lock_train end " + //TODO: временное условие "не показывать поезда отправленные на Прокатную-1 с пом. ПО, а только из КИС"
        //        //    "group by vo.id_stat, s.name, vo.st_lock_id_stat, vo.st_lock_train, FORMAT(vo.dt_from_stat, 'yyyy-MM-dd HH:mm:ss'), "+
        //        //    "vo.st_gruz_front, g.name, vo.st_shop, sh.name, vo.st_lock_locom1, vo.st_lock_locom2 " +
        //        //    "order by dt_from_stat");
        //    //    query = string.Format(
        //    //        "select vo.id_stat, s.name as stat, vo.st_lock_id_stat, vo.st_lock_train, "+
        //    //        "max(FORMAT(vo.dt_from_stat, 'yyyy-MM-dd HH:mm:ss')) AS dt_from_stat, count(vo.id_vagon) as vag_amount, "+
        //    //        "vo.st_gruz_front, g.name as gruz_front_name, vo.st_shop, sh.name as shop_name, vo.st_lock_locom1, vo.st_lock_locom2 "+
        //    //        "from VAGON_OPERATIONS vo inner join STATIONS s on vo.id_stat=s.id_stat "+
        //    //        "left join GRUZ_FRONTS g on vo.st_gruz_front=g.id_gruz_front "+
        //    //        "left join SHOPS sh on vo.st_shop=sh.id_shop "+
        //    //        "where vo.st_lock_id_stat=@id_stat and vo.is_present=0 and vo.is_hist=0 "+
        //    //        "and ((vo.st_shop = -1 and vo.st_gruz_front = -1) or (vo.st_shop is null and vo.st_gruz_front is null)) "+
        //    //        "and vo.st_lock_train = vo.st_lock_train "+ // временное условие "не показывать поезда отправленные на Прокатную-1 с пом. ПО, а только из КИС"
        //    //        "group by vo.id_stat, s.name, vo.st_lock_id_stat, vo.st_lock_train, FORMAT(vo.dt_from_stat, 'yyyy-MM-dd HH:mm:ss'), "+
        //    //        "vo.st_gruz_front, g.name, vo.st_shop, sh.name, vo.st_lock_locom1, vo.st_lock_locom2 " +
        //    //        "order by dt_from_stat");
        //    }
        //    else if (sp.GetType().IsAssignableFrom(typeof(GruzFront)))
        //    {
        //        // Вагоно-опрокид
        //        query = string.Format(
        //            "select vo.id_stat, s.name as stat, vo.st_lock_id_stat, vo.st_lock_train, " +
        //            "max(FORMAT(vo.dt_from_way, 'yyyy-MM-dd HH:mm:ss')) AS dt_from_way, count(vo.id_vagon) as vag_amount, " +
        //            "vo.st_gruz_front, g.name as gruz_front_name, vo.st_shop, sh.name as shop_name, vo.st_lock_locom1, vo.st_lock_locom2 " +
        //            "from VAGON_OPERATIONS vo inner join STATIONS s on vo.id_stat=s.id_stat " +
        //            "left join GRUZ_FRONTS g on vo.st_gruz_front=g.id_gruz_front " +
        //            "left join SHOPS sh on vo.st_shop=sh.id_shop " +
        //            "where vo.st_lock_id_stat=@id_stat and vo.is_present=0 and vo.is_hist=0 " +
        //            "and vo.st_gruz_front <> -1 and vo.st_gruz_front is not null " +
        //            "group by vo.id_stat, s.name, vo.st_lock_id_stat, vo.st_lock_train, " +
        //            "vo.st_gruz_front, g.name, vo.st_shop, sh.name, vo.st_lock_locom1, vo.st_lock_locom2 " +
        //            "order by dt_from_way");
        //    }
        //    else
        //    {
        //       // Цех
        //        //query = string.Format(
        //        //    "select vo.id_stat, s.name as stat, vo.st_lock_id_stat, vo.st_lock_train, " +
        //        //    "max(FORMAT(vo.dt_from_way, 'yyyy-MM-dd HH:mm:ss')) AS dt_from_way, count(vo.id_vagon) as vag_amount, " +
        //        //    "vo.st_gruz_front, g.name as gruz_front_name, vo.st_shop, sh.name as shop_name, vo.st_lock_locom1, vo.st_lock_locom2 " +
        //        //    "from VAGON_OPERATIONS vo inner join STATIONS s on vo.id_stat=s.id_stat " +
        //        //    "left join GRUZ_FRONTS g on vo.st_gruz_front=g.id_gruz_front " +
        //        //    "left join SHOPS sh on vo.st_shop=sh.id_shop " +
        //        //    "where vo.st_lock_id_stat=@id_stat and vo.is_present=0 and vo.is_hist=0 " +
        //        //    "and vo.st_shop <> -1 and vo.st_shop is not null " +
        //        //    "group by vo.id_stat, s.name, vo.st_lock_id_stat, vo.st_lock_train, " +
        //        //    "vo.st_gruz_front, g.name, vo.st_shop, sh.name, vo.st_lock_locom1, vo.st_lock_locom2 " +
        //        //    "order by dt_from_way");
        //        query = string.Format(
        //            "select vo.id_stat, s.name as stat, vo.st_lock_id_stat, vo.st_lock_train, " +
        //            "max(FORMAT(vo.dt_from_way, 'yyyy-MM-dd HH:mm:ss')) AS dt_from_way, count(vo.id_vagon) as vag_amount, " +
        //            "vo.st_gruz_front, g.name as gruz_front_name, vo.st_shop, sh.name as shop_name, vo.st_lock_locom1, vo.st_lock_locom2 " +
        //            "from VAGON_OPERATIONS vo inner join STATIONS s on vo.id_stat=s.id_stat " +
        //            "left join GRUZ_FRONTS g on vo.st_gruz_front=g.id_gruz_front " +
        //            "left join SHOPS sh on vo.st_shop=sh.id_shop " +
        //            "where vo.dt_from_way is not null and vo.st_lock_id_stat=@id_stat and vo.is_present=0 and vo.is_hist=0 " +
        //            "and vo.st_shop <> -1 and vo.st_shop is not null " +
        //            "group by vo.id_stat, s.name, vo.st_lock_id_stat, vo.st_lock_train, " +
        //            "vo.st_gruz_front, g.name, vo.st_shop, sh.name, vo.st_lock_locom1, vo.st_lock_locom2 " +
        //            "order by dt_from_way");
        //    }
        //    SqlParameter[] sqlParameters = new SqlParameter[1];
        //    sqlParameters[0] = new SqlParameter("@id_stat", stat.ID); 
        //    return Conn.executeSelectQuery(query, sqlParameters).Tables[0];
        //}

        /// <summary>
        /// Получить перечень вагонов
        /// </summary>
        /// <param name="train"></param>
        /// <param name="stat"></param>
        /// <param name="side"></param>
        /// <param name="isGF"></param>
        /// <param name="isCeh"></param>
        /// <returns></returns>
        public List<VagWaitAdmiss> getVagons(Train train, Station stat, Side side, bool isGF, bool isCeh)
        {
            DataTable table = getVagonsTable(train, stat, side, isGF, isCeh);

            List<VagWaitAdmiss> list = (base.getVagons(table)).Select(parent => new VagWaitAdmiss(parent)).ToList();

            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                list[i].num_vag_on_way = table.Rows.IndexOf(row) + 1;

                i++;
            }

            return list;
        }
        /// <summary>
        /// сформировать и вернуть список поездов (List<Train>) в прибытии
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public List<Train> getTrains(Station stat, SendingPoint sp)
        {
            DataTable table = getTrainsTable(stat, sp); // Получить список поездов для снятия

            List<Train> list = new List<Train>();

            foreach (DataRow row in table.Rows)
            {
                Train train = new Train();
                train.Num = Int32.Parse(row["st_lock_train"].ToString());
                //train.StationFrom = new Station(Int32.Parse(row["id_stat"].ToString()), "ст. "+row["stat"].ToString().Trim());
                //train.StationFrom.PointName = StationUtils.GetInstance().ClassName;
                if (row["st_gruz_front"] != DBNull.Value && Int32.Parse(row["st_gruz_front"].ToString()) != -1)
                {
                    GruzFront gf = new GruzFront();
                    gf.PointName = GruzFrontUtils.GetInstance().ClassName;
                    gf.ID = Int32.Parse(row["st_gruz_front"].ToString());
                    gf.Name = row["gruz_front_name"].ToString();
                    //gf.Stat = train.StationFrom;
                    train.SendingPoint = gf;
                }
                else
                    if (row["st_shop"] != DBNull.Value && Int32.Parse(row["st_shop"].ToString()) != -1)
                    {
                        Shop shop = new Shop();
                        shop.PointName = ShopUtils.GetInstance().ClassName;
                        shop.ID = Int32.Parse(row["st_shop"].ToString());
                        shop.Name = row["shop_name"].ToString();
                        //train.Shop.Stat = train.StationFrom;
                        train.SendingPoint = shop;
                    }
                    else
                    {
                        Station stationFrom = new Station();
                        stationFrom.PointName = StationUtils.GetInstance().ClassName;
                        stationFrom.ID = Int32.Parse(row["id_stat"].ToString());
                        stationFrom.Name = "ст. " + row["stat"].ToString().Trim();
                        train.SendingPoint = stationFrom;
                    }               
                train.StationTo = stat;
                if (!sp.GetType().IsAssignableFrom(typeof(Station)))
                    train.DateFromStat = DateTime.Parse(row["dt_from_way"].ToString());
                else train.DateFromStat = DateTime.Parse(row["dt_from_stat"].ToString());
                train.Vag_amount = Int32.Parse(row["vag_amount"].ToString());
                if (row["st_lock_locom1"] != DBNull.Value) train.St_lock_locom1 = Int32.Parse(row["st_lock_locom1"].ToString());
                if (row["st_lock_locom2"] != DBNull.Value) train.St_lock_locom2 = Int32.Parse(row["st_lock_locom2"].ToString());
                //if (row["id_ora_23_temp"] != DBNull.Value)
                //{
                //    train.id_ora_23_temp = Int32.Parse(row["id_ora_23_temp"].ToString());//TODO:
                //}
                //if (row["id_oracle"] != DBNull.Value)
                //{
                //    train.id_oracle = Int32.Parse(row["id_oracle"].ToString());             //TODO:
                //}
                list.Add(train);
            }

            return list;
        }
        /// <summary>
        /// Отправка вагона на другую станцию, цех,тупик
        /// </summary>
        /// <param name="vagWaitAdmiss"></param>
        /// <param name="stat"></param>
        /// <param name="way"></param>
        /// <param name="dt_arriv"></param>
        /// <param name="locom1"></param>
        /// <param name="locom2"></param>
        /// <returns></returns>
        public int execAdmissOthStat(VagWaitAdmiss vagWaitAdmiss, Station stat, Way way, DateTime? dt_arriv, int locom1, int locom2)
        {
            string query = string.Format("RailCars.ExecOtherStation_");
            SqlParameter[] sqlParameters = base.paramsForInsert((VagOperations)vagWaitAdmiss, way, 6);

            int i = sqlParameters.Length;

            if (vagWaitAdmiss.cond.Id == -1) sqlParameters[i-6] = new SqlParameter("@id_cond2", DBNull.Value);
            else sqlParameters[i-6] = new SqlParameter("@id_cond2", vagWaitAdmiss.cond.Id);
            if (dt_arriv < DateTime.Parse("1900-01-01 00:00") | dt_arriv==null)
            {
                sqlParameters[i - 5] = new SqlParameter("@dt_on_stat", DBNull.Value);
                sqlParameters[i - 4] = new SqlParameter("@dt_on_way", DBNull.Value);
            }
            else
            {
                sqlParameters[i - 5] = new SqlParameter("@dt_on_stat", dt_arriv);
                sqlParameters[i - 4] = new SqlParameter("@dt_on_way", dt_arriv);
            }

            if (locom1 == -1) sqlParameters[i - 3] = new SqlParameter("@id_locom1", DBNull.Value);
            else sqlParameters[i - 3] = new SqlParameter("@id_locom1", locom1);

            if (locom2 == -1) sqlParameters[i - 2] = new SqlParameter("@id_locom2", DBNull.Value);
            else sqlParameters[i - 2] = new SqlParameter("@id_locom2", locom2);

            sqlParameters[i - 1] = new SqlParameter("@new_identity", SqlDbType.Int);
            sqlParameters[i - 1].Direction = ParameterDirection.Output;

            Conn.executeScalarCommand(query, sqlParameters, true);
            if (sqlParameters[i - 1].Value != null)
                return int.Parse(sqlParameters[i - 1].Value.ToString());
            else return -1;
        }

        public void getLoadingData(VagWaitAdmiss vagWaitAdmiss, Shop shop, DateTime dtFromStat)
        {
            string query = string.Format("SELECT a.*, b.id_gruz "+
              "FROM Loading_data_SAP a "+
              "left join GRUZS b on a.rodU = b.name_full and b.id_ora is not null " +
              "where a.vagu = @vag and @ceh like a.cehu+'%' and a.dat_dsdu >= @dt_from_way");
            SqlParameter[] sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@vag", vagWaitAdmiss.num_vag);
            sqlParameters[1] = new SqlParameter("@ceh", shop.Name);
            if (dtFromStat < DateTime.Parse("1900-01-01 00:00"))
                sqlParameters[2] = new SqlParameter("@dt_from_way", DBNull.Value);
            else sqlParameters[2] = new SqlParameter("@dt_from_way", dtFromStat.Date);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];
            try
            {
                vagWaitAdmiss.id_gruz = Int32.Parse(table.Rows[0]["id_gruz"].ToString());
                vagWaitAdmiss.gruz = table.Rows[0]["rodu"].ToString();
                vagWaitAdmiss.gdstait = table.Rows[0]["stnu"].ToString();
                vagWaitAdmiss.weight_gruz = Double.Parse(table.Rows[0]["tonu5"].ToString());
                vagWaitAdmiss.grvuSAP = table.Rows[0]["grvu"].ToString();
                vagWaitAdmiss.ngruSAP = table.Rows[0]["ngru"].ToString();
            }
            catch (Exception)
            { }

            query = string.Format("declare @id_str int "+
                "select @id_str = id_gdstait from GDSTAIT where name=@name "+
                "if (@id_str is null) "+
                " begin "+
	            "    insert into GDSTAIT (name) values (@name) "+
                "    select @@IDENTITY; "+
                " end "+
                "else select @id_str");
            SqlParameter[] sqlParameters2 = new SqlParameter[1];
            sqlParameters2[0] = new SqlParameter("@name", vagWaitAdmiss.gdstait);
            try
            {
                table = Conn.executeSelectQuery(query, sqlParameters2).Tables[0];
                vagWaitAdmiss.id_gdstait = Int32.Parse(table.Rows[0][0].ToString());
            }
            catch (Exception)
            { }
        }

        public bool cancelGfOrShopSending(Train train, Station stat, bool isGf, List<VagWaitAdmiss> vagons)
        {
            string query = string.Format("update VAGON_OPERATIONS set st_lock_id_stat = null, st_lock_order = null," +
                "st_lock_train = null, st_lock_side = null, st_lock_locom1 = null, st_lock_locom2 = null, st_shop = null, st_gruz_front = null, " +
                "is_present=1, dt_from_way = null " +
                "where id_stat=@id_stat and is_present=0 and is_hist=0 " +
                
                "and CAST(FORMAT(dt_from_way,'yyyy-MM-dd HH:mm:ss') AS datetime)=CAST(FORMAT(@dt,'yyyy-MM-dd HH:mm:ss') as datetime)");
            if (isGf)
                query += " and st_gruz_front is not null and st_gruz_front != -1";
            else query += " and st_shop is not null and st_shop != -1";

            if (vagons.Count > 0)
            {
                query += " and id_oper in (";
                foreach (VagWaitAdmiss vag in vagons)
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

        public new Tuple<DateTime, int, int, int> findVagLocation(Station stat, int num)
        {
            string query = string.Format("select vo.dt_from_way, vo.st_lock_order, vo.st_gruz_front, vo.st_shop " +
                "from VAGON_OPERATIONS vo " +
                "inner join VAGONS v on vo.id_vagon=v.id_vag " +
                "and v.num=@num and vo.is_present=0 " +
                "and vo.is_hist=0 and vo.st_lock_id_stat=@id_stat");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@num", num);
            sqlParameters[1] = new SqlParameter("@id_stat", stat.ID);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];

            Tuple<DateTime, int, int, int> tuple = new Tuple<DateTime, int, int, int>(DateTime.MinValue, -1, 0, 0);
            if (table.Rows.Count > 0)
            {
                int gf = 0; int shop = 0;
                if (table.Rows[0]["st_gruz_front"] != null) gf = Int32.Parse(table.Rows[0]["st_gruz_front"].ToString()); else gf = -1;
                if (table.Rows[0]["st_shop"] != null) shop = Int32.Parse(table.Rows[0]["st_shop"].ToString()); else shop = -1;
                if (table.Rows[0]["dt_from_way"] != null && table.Rows[0]["st_lock_order"] != null)
                    tuple = new Tuple<DateTime, int, int, int>(DateTime.Parse(table.Rows[0]["dt_from_way"].ToString()), Int32.Parse(table.Rows[0]["st_lock_order"].ToString()), gf, shop);
            }
            return tuple;
        }
    }
}
