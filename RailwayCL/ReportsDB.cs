using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace RailwayCL
{
    public class ReportsDB : DB
    {
        public List<RepPos> getPos(Station station, int repType)
        {
            List<RepPos> list = new List<RepPos>();
            string query = "";
            switch (repType) 
            {
                case 1:
                    query = String.Format(
                        "select w.id_way, w.num as num_way, "+
                        "(case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end) as is_loaded, "+
                        "o.abr as details, count(vo.id_vagon) as amount "+
                        "from WAYS w "+
                        "left join VAGON_OPERATIONS vo on w.id_way=vo.id_way and vo.is_present=1 and vo.is_hist=0 "+
                        "inner join STATIONS s on w.id_stat=s.id_stat "+
                        "left join VAGONS v on vo.id_vagon=v.id_vag "+
                        "left join OWNERS o on v.id_owner = o.id_owner "+
                        "where w.id_stat=@id_stat "+
                        "group by w.id_way, w.num, w.name, case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end, o.abr, w.[order] "+
                        "order by w.[order]"
                    );
                    break;
                case 2:
                    query = String.Format(
				        "select w.id_way, w.num as num_way, "+
                        "(case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end) as is_loaded, "+
                        "g.name as details, count(vo.id_vagon) as amount "+
                        "from WAYS w "+
                        "left join VAGON_OPERATIONS vo on w.id_way=vo.id_way and vo.is_present=1 and vo.is_hist=0 "+
                        "inner join STATIONS s on w.id_stat=s.id_stat "+
                        "left join GRUZS g on vo.id_gruz = g.id_gruz "+
                        "where w.id_stat=@id_stat " +
                        "group by w.id_way, w.num, w.name, case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end, g.name, w.[order] "+
                        "order by w.[order]"
                    );
                    break;
                case 3:
                    query = String.Format(
                        "select w.id_way, w.num as num_way, " +
                        "(case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end) as is_loaded, " +
                        "sh.name as details, count(vo.id_vagon) as amount " +
                        "from WAYS w " +
                        "left join VAGON_OPERATIONS vo on w.id_way=vo.id_way and vo.is_present=1 and vo.is_hist=0 " +
                        "inner join STATIONS s on w.id_stat=s.id_stat " +
                        "left join SHOPS sh on vo.id_shop_gruz_for = sh.id_shop " +
                        "where w.id_stat=@id_stat " +
                        "group by w.id_way, w.num, w.name, case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end, sh.name, w.[order] " +
                        "order by w.[order] "
                    );
                    break;
                case 4:
                    query = String.Format(
                        "select w.id_way, w.num as num_way, "+
                        "(case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end) as is_loaded, "+
                        "c.name as details, count(vo.id_vagon) as amount "+
                        "from WAYS w "+
                        "left join VAGON_OPERATIONS vo on w.id_way=vo.id_way and vo.is_present=1 and vo.is_hist=0 "+
                        "inner join STATIONS s on w.id_stat=s.id_stat "+
                        "left join VAG_CONDITIONS2 c on vo.id_cond2 = c.id_cond "+
                        "where w.id_stat=@id_stat " +
                        "group by w.id_way, w.num, w.name, case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end, c.name, w.[order] "+
                        "order by w.[order] "
                    );
                    break;
            }
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_stat", station.ID);

            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                RepPos repPos = new RepPos();
                repPos.WayOrStatId = Int32.Parse(row["id_way"].ToString());
                repPos.WayOrStatName = row["num_way"].ToString().Trim();
                repPos.IsLoaded = Int32.Parse(row["is_loaded"].ToString());
                repPos.DetailsName = row["details"].ToString().Trim();
                repPos.Amount = Int32.Parse(row["amount"].ToString());
                list.Add(repPos);
            }
            return list;
        }

        public List<RepPos> getPos(int repType)
        {
            List<RepPos> list = new List<RepPos>();
            string query = "";
            switch (repType)
            {
                case 1:
                    query = String.Format(
                        "select s.id_stat, s.name, " +
                        "(case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end) as is_loaded, " +
                        "o.abr as details, count(vo.id_vagon) as amount " +
                        "from STATIONS s " +
                        "left join VAGON_OPERATIONS vo on s.id_stat=vo.id_stat and vo.is_present=1 and vo.is_hist=0 " +
                        "left join VAGONS v on vo.id_vagon=v.id_vag " +
                        "left join OWNERS o on v.id_owner = o.id_owner " +
                        "where s.is_uz = 0 "+
                        "group by s.id_stat, s.name, case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end, o.abr"
                    );
                    break;
                case 2:
                    query = String.Format(
                        "select s.id_stat, s.name, " +
                        "(case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end) as is_loaded, " +
                        "g.name as details, count(vo.id_vagon) as amount " +
                        "from STATIONS s " +
                        "left join VAGON_OPERATIONS vo on s.id_stat=vo.id_stat and vo.is_present=1 and vo.is_hist=0 " +
                        "left join GRUZS g on vo.id_gruz = g.id_gruz " +
                        "where s.is_uz = 0 " +
                        "group by s.id_stat, s.name, case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end, g.name "
                    );
                    break;
                case 3:
                    query = String.Format(
                        "select s.id_stat, s.name, "+
                        "(case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end) as is_loaded, "+
                        "sh.name as details, count(vo.id_vagon) as amount "+
                        "from STATIONS s "+
                        "left join VAGON_OPERATIONS vo on s.id_stat=vo.id_stat and vo.is_present=1 and vo.is_hist=0 "+
                        "left join SHOPS sh on vo.id_shop_gruz_for = sh.id_shop "+
                        "where s.is_uz = 0 " +
                        "group by s.id_stat, s.name, case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end, sh.name "
                    );
                    break;
                case 4:
                    query = String.Format(
                        "select s.id_stat, s.name, "+
                        "(case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end) as is_loaded, "+
                        "c.name as details, count(vo.id_vagon) as amount "+
                        "from STATIONS s "+
                        "left join VAGON_OPERATIONS vo on s.id_stat=vo.id_stat and vo.is_present=1 and vo.is_hist=0 "+
                        "left join VAG_CONDITIONS2 c on vo.id_cond2 = c.id_cond "+
                        "where s.is_uz = 0 " +                       
                        "group by s.id_stat, s.name, case when vo.id_gruz=6 then 0 when vo.id_gruz is null then -1 else 1 end, c.name "
                    );
                    break;
            }

            DataTable table = Conn.executeSelectQuery(query, new SqlParameter[0]).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                RepPos repPos = new RepPos();
                repPos.WayOrStatId = Int32.Parse(row["id_stat"].ToString());
                repPos.WayOrStatName = row["name"].ToString().Trim();
                repPos.IsLoaded = Int32.Parse(row["is_loaded"].ToString());
                repPos.DetailsName = row["details"].ToString().Trim();
                repPos.Amount = Int32.Parse(row["amount"].ToString());
                list.Add(repPos);
            }

            return list;
        }

        public List<RepProst> getProst(Station stat)
        {
            List<RepProst> list = new List<RepProst>();
            string query = string.Format("select va.num, o.abr as owner, va.rod, g.name as gruz, w.num as way, v.dt_amkr, /*convert(float, DATEDIFF(MINUTE, dt_on_stat, GETDATE()))/60*/ " +
                "DATEDIFF(hour, v.dt_amkr, GETDATE()) as hour_on_amkr, v.dt_on_stat, DATEDIFF(hour, v.dt_on_stat, GETDATE()) as hour_on_stat, "+
                "vc.name as godn, vc2.name as cond "+
                "from VAGON_OPERATIONS v "+
                "inner join VAGONS va on v.id_vagon= va.id_vag "+
                "left join OWNERS o on va.id_owner=o.id_owner "+
                "left join WAYS w on v.id_way = w.id_way "+
                "left join GRUZS g on v.id_gruz = g.id_gruz " +
                "left join VAG_CONDITIONS vc on v.id_cond = vc.id_cond "+
                "left join VAG_CONDITIONS2 vc2 on v.id_cond2 = vc2.id_cond "+
                "where v.id_stat = @id_stat and v.is_present=1 and v.is_hist=0 "+
                "order by w.num");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_stat", stat.ID);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                RepProst repProst = new RepProst();
                repProst.NumVag = Int32.Parse(row["num"].ToString());
                if (row["owner"] != DBNull.Value) repProst.owner = row["owner"].ToString().Trim();
                if (row["rod"] != DBNull.Value) repProst.TypeVag = row["rod"].ToString().Trim();
                if (row["gruz"] != DBNull.Value) repProst.gruz = row["gruz"].ToString().Trim();
                if (row["godn"] != DBNull.Value) repProst.godn = row["godn"].ToString().Trim();
                if (row["cond"] != DBNull.Value) repProst.cond = row["cond"].ToString().Trim();
                if (row["way"] != DBNull.Value) repProst.NumWay = row["way"].ToString().Trim();
                if (row["dt_amkr"] != DBNull.Value) repProst.dt_amkr = DateTime.Parse(row["dt_amkr"].ToString());
                if (row["hour_on_amkr"] != DBNull.Value) repProst.Hour_Amkr = Int32.Parse(row["hour_on_amkr"].ToString());
                if (row["dt_on_stat"] != DBNull.Value) repProst.dt_on_stat = DateTime.Parse(row["dt_on_stat"].ToString());
                if (row["hour_on_stat"] != DBNull.Value) repProst.Hour_on_stat = Int32.Parse(row["hour_on_stat"].ToString());
                list.Add(repProst);
            }

            return list;
        }

        public List<RepProst> getProst()
        {
            List<RepProst> list = new List<RepProst>();
            string query = string.Format("select va.num, o.abr as owner, va.rod, g.name as gruz, s.name as stat, w.num as way, v.dt_amkr, /*convert(float, DATEDIFF(MINUTE, dt_on_stat, GETDATE()))/60*/ " +
                "DATEDIFF(hour, v.dt_amkr, GETDATE()) as hour_on_amkr, v.dt_on_stat, DATEDIFF(hour, v.dt_on_stat, GETDATE()) as hour_on_stat, "+
                "vc.name as godn, vc2.name as cond " +
                "from VAGON_OPERATIONS v "+
                "inner join VAGONS va on v.id_vagon= va.id_vag "+
                "left join OWNERS o on va.id_owner=o.id_owner "+
                "left join STATIONS s on v.id_stat=s.id_stat "+
                "left join WAYS w on v.id_way = w.id_way "+
                "left join GRUZS g on v.id_gruz = g.id_gruz "+
                "left join VAG_CONDITIONS vc on v.id_cond = vc.id_cond "+
                "left join VAG_CONDITIONS2 vc2 on v.id_cond2 = vc2.id_cond "+
                "where v.is_present=1 and v.is_hist=0 "+
                "order by stat, way");
            DataTable table = Conn.executeSelectQuery(query, new SqlParameter[0]).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                RepProst repProst = new RepProst();
                repProst.NumVag = Int32.Parse(row["num"].ToString());
                if (row["owner"] != DBNull.Value) repProst.owner = row["owner"].ToString().Trim();
                if (row["rod"] != DBNull.Value) repProst.TypeVag = row["rod"].ToString().Trim();
                if (row["gruz"] != DBNull.Value) repProst.gruz = row["gruz"].ToString().Trim();
                if (row["godn"] != DBNull.Value) repProst.godn = row["godn"].ToString().Trim();
                if (row["cond"] != DBNull.Value) repProst.cond = row["cond"].ToString().Trim();
                if (row["stat"] != DBNull.Value) repProst.Station = row["stat"].ToString().Trim();
                if (row["way"] != DBNull.Value) repProst.NumWay = row["way"].ToString().Trim();
                if (row["dt_amkr"] != DBNull.Value) repProst.dt_amkr = DateTime.Parse(row["dt_amkr"].ToString());
                if (row["hour_on_amkr"] != DBNull.Value) repProst.Hour_Amkr = Int32.Parse(row["hour_on_amkr"].ToString());
                if (row["dt_on_stat"] != DBNull.Value) repProst.dt_on_stat = DateTime.Parse(row["dt_on_stat"].ToString());
                if (row["hour_on_stat"] != DBNull.Value) repProst.Hour_on_stat = Int32.Parse(row["hour_on_stat"].ToString());
                list.Add(repProst);
            }

            return list;
        }

        public List<RepVagHist> getVagHist(int num_vag)
        {
            List<RepVagHist> list = new List<RepVagHist>();
            string query = string.Format(
                  "select 'way' as point_type, s.name, w.num as point_, dt_on_way, dt_on_stat, dt_from_stat, " +
                  "case when dt_from_way is not null " +
                  "     then datediff(SS,dt_on_way,dt_from_way)/3600 " +
                  "     else datediff(SS,dt_on_way,GETDATE())/3600 end as hours_at_point, " +
                  "case when dt_from_way is not null " +
                  "     then (datediff(SS,dt_on_way,dt_from_way)%3600)/60 " +
                  "     else (datediff(SS,dt_on_way,GETDATE())%3600)/60 end as min_at_point " +
                  "from VAGON_OPERATIONS vo " +
                  "inner join VAGONS v on vo.id_vagon=v.id_vag and v.num = @vag_num " +
                  "inner join STATIONS s on vo.id_stat=s.id_stat " +
                  "inner join WAYS w on vo.id_way=w.id_way " +
                  "union " +
                  "select 'gf' as point_type, s.name, N'в-д ' + gf.name as point_, dt_on_way, dt_on_stat, dt_from_stat, " +
				  "case when is_hist=0 "+
				  "     then datediff(SS,dt_from_way,GETDATE())/3600 "+
				  "	   else datediff(SS,dt_from_way,(select dt_on_way from VAGON_OPERATIONS where id_oper_parent=vo.id_oper))/3600 end as hours_at_point, "+ 
				  "case when is_hist=0 "+
				  "	   then (datediff(SS,dt_from_way,GETDATE())%3600)/60 "+
				  "	   else (datediff(SS,dt_from_way,(select dt_on_way from VAGON_OPERATIONS where id_oper_parent=vo.id_oper))%3600)/60 end as min_at_point "+
                  "from VAGON_OPERATIONS vo " +
                  "inner join VAGONS v on vo.id_vagon=v.id_vag and v.num = @vag_num " +
                  "inner join STATIONS s on vo.id_stat=s.id_stat " +
                  "inner join GRUZ_FRONTS gf on vo.st_gruz_front=gf.id_gruz_front " +
                  "where vo.st_gruz_front>0 " +
                  "union " +
                  "select 'shop' as point_type, s.name, N'цех ' + sh.name as point_, dt_on_way, dt_on_stat, dt_from_stat, " +
                  "datediff(SS,dt_from_way,GETDATE())/3600 as hours_at_point, (datediff(SS,dt_from_way,GETDATE())%3600)/60 as min_at_point " +
                  "from VAGON_OPERATIONS vo " +
                  "inner join VAGONS v on vo.id_vagon=v.id_vag and v.num = @vag_num " +
                  "inner join STATIONS s on vo.id_stat=s.id_stat " +
                  "inner join SHOPS sh on vo.st_shop=sh.id_shop " +
                  "where vo.st_shop>0 " +
                  "order by dt_on_way, point_type desc ");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@vag_num", num_vag);            
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];
            int parseInt = 0;
            DateTime parseDt = DateTime.MinValue;
            foreach (DataRow row in table.Rows)
            {
                RepVagHist repVagHist = new RepVagHist();
                if (row["point_type"] != null) repVagHist.PointType = row["point_type"].ToString().Trim();
                if (row["name"] != null) repVagHist.Station_name = row["name"].ToString().Trim();
                if (row["point_"] != null) repVagHist.Point = row["point_"].ToString().Trim();
                if (row["hours_at_point"] != null)
                {
                    Int32.TryParse(row["hours_at_point"].ToString(), out parseInt);
                    repVagHist.HoursAtPoint = parseInt;
                    parseInt = 0;
                }
                if (row["min_at_point"] != null)
                {
                    Int32.TryParse(row["min_at_point"].ToString(), out parseInt);
                    repVagHist.MinAtPoint = parseInt;
                    parseInt = 0;
                }
                if (row["dt_on_stat"] != null)
                {
                    DateTime.TryParse(row["dt_on_stat"].ToString(), out parseDt);
                    if (parseDt != DateTime.MinValue) repVagHist.DtOnStat = parseDt.ToString("g");
                    else repVagHist.DtOnStat = "";
                    parseDt = DateTime.MinValue;
                }
                if (row["dt_from_stat"] != null)
                {
                    DateTime.TryParse(row["dt_from_stat"].ToString(), out parseDt);
                    if (parseDt != DateTime.MinValue) repVagHist.DtFromStat = parseDt.ToString("g");
                    else repVagHist.DtFromStat = "";
                    parseDt = DateTime.MinValue;
                }
                list.Add(repVagHist);
            }
            return list;
        }

        public List<RepWaysLoaded> getWaysLoaded(DateTime bd, DateTime ed)
        {
            List<RepWaysLoaded> list = new List<RepWaysLoaded>();
            string query = string.Format(
                "select s.name, w.num, count(distinct id_vagon) as vag_amount "+
                "from VAGON_OPERATIONS vo "+
                "inner join STATIONS s on vo.id_stat=s.id_stat "+
                "inner join WAYS w on vo.id_way=w.id_way "+
                "/*where vo.is_hist=0 and vo.is_present=1*/ "+
                "where vo.dt_on_way < @ed and (vo.dt_from_way > @bd or vo.dt_from_way is null) "+
                "group by s.name, w.num, w.[order] "+
                "order by s.name, w.[order]");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@bd", bd);
            sqlParameters[1] = new SqlParameter("@ed", ed);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                RepWaysLoaded repWaysLoaded = new RepWaysLoaded();
                repWaysLoaded.Station_name = row["name"].ToString().Trim();
                repWaysLoaded.Way_num = row["num"].ToString().Trim();
                repWaysLoaded.Vag_amount = Int32.Parse(row["vag_amount"].ToString());
                list.Add(repWaysLoaded);
            }
            return list;
        }

        public List<RepGfUnlTurnover> getGfUnloadTurnover(DateTime bd, DateTime ed)
        {
            List<RepGfUnlTurnover> list = new List<RepGfUnlTurnover>();
            string query = string.Format(
                //"select s.name as station, g.name as gruz, count(distinct id_vagon) as vag_amount "+
                //"from VAGON_OPERATIONS vo "+
                //"inner join STATIONS s on vo.id_stat=s.id_stat "+
                //"inner join GRUZS g on vo.id_gruz=g.id_gruz "+
                //"where ((select dt_on_way from VAGON_OPERATIONS where id_oper_parent=vo.id_oper)>@bd or vo.is_hist=0) "+
                //"and vo.st_gruz_front>0 and dt_from_way < @ed "+
                //"group by s.name, g.name "+
                //"order by s.name");
                "select s.name as station, g.name as gruz, case when vo.st_gruz_front>0 "+
                "then N'в-д '+ gf.name else N'цех '+ sh.name end as gf_sh, count(distinct id_vagon) as vag_amount "+
                "from VAGON_OPERATIONS vo "+
                "inner join STATIONS s on vo.id_stat=s.id_stat "+
                "inner join GRUZS g on vo.id_gruz=g.id_gruz "+
                "left join GRUZ_FRONTS gf on vo.st_gruz_front=gf.id_gruz_front "+
                "left join SHOPS sh on vo.st_shop=sh.id_shop "+
                "where ((select top 1 dt_on_way from VAGON_OPERATIONS where id_oper_parent=vo.id_oper)>@bd or vo.is_hist=0) "+
                "and (vo.st_gruz_front>0 or (vo.st_shop>0 and vo.id_cond2=5)) and dt_from_way < @ed "+
                "group by s.name, vo.dt_from_way, g.name, vo.st_gruz_front, gf.name, sh.name "+
                "order by s.name, vo.dt_from_way");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@bd", bd);
            sqlParameters[1] = new SqlParameter("@ed", ed);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                RepGfUnlTurnover repGfUnlTurnover = new RepGfUnlTurnover();
                repGfUnlTurnover.Station_name = row["station"].ToString().Trim();
                repGfUnlTurnover.Gf_sh = row["gf_sh"].ToString().Trim();
                repGfUnlTurnover.Gruz_name = row["gruz"].ToString().Trim();
                repGfUnlTurnover.Vag_amount = Int32.Parse(row["vag_amount"].ToString());
                list.Add(repGfUnlTurnover);
            }
            return list;
        }

        public List<RepVagOnCleanWays> getVagOnCleaWays(DateTime bd, DateTime ed, Station station)
        {
            List<RepVagOnCleanWays> list = new List<RepVagOnCleanWays>();
            string query = string.Format(
                "select v.num, w.num as way_num, vo.dt_on_way, vo.dt_from_way " +
                "from VAGON_OPERATIONS vo " +
                "inner join VAGONS v on vo.id_vagon = v.id_vag " +
                "inner join WAYS w on vo.id_way=w.id_way and w.bind_id_cond=2 " +
                "where vo.id_stat=@id_stat and vo.dt_on_way < @ed and (vo.dt_from_way > @bd or vo.dt_from_way is null) " +
                "order by w.num, vo.dt_on_way");
            SqlParameter[] sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@bd", bd);
            sqlParameters[1] = new SqlParameter("@ed", ed);
            sqlParameters[2] = new SqlParameter("@id_stat", station.ID);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                RepVagOnCleanWays repVagOnCleanWays = new RepVagOnCleanWays();
                repVagOnCleanWays.NumVag = Int32.Parse(row["num"].ToString());
                repVagOnCleanWays.NumWay = row["way_num"].ToString();
                if (row["dt_from_way"] == DBNull.Value) repVagOnCleanWays.DtFromWay = DateTime.Parse("1900-01-01 00:00");
                else repVagOnCleanWays.DtFromWay = DateTime.Parse(row["dt_from_way"].ToString());
                repVagOnCleanWays.DtOnWay = DateTime.Parse(row["dt_on_way"].ToString());
                list.Add(repVagOnCleanWays);
            }
            return list;
        }
    }
}
