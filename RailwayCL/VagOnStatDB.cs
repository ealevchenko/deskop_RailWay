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
    public class VagOnStatDB : VagOperationsDB
    {
        //private static VagOnStatDB vagOnStatDB;

        //private VagOnStatDB() { }

        //public static VagOnStatDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (vagOnStatDB == null)
        //    {
        //        lock (typeof(VagOnStatDB))
        //        {
        //            if (vagOnStatDB == null)
        //                vagOnStatDB = new VagOnStatDB();
        //        }
        //    }

        //    return vagOnStatDB;
        //}
        /// <summary>
        /// Выборка данных по вагонам
        /// </summary>
        /// <param name="way"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        private DataTable getVagonsTable(Way way, Side side)
        {
            string query = "[RailCars].[GetOnStatWagons]";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@idway", way.ID);
            sqlParameters[1] = new SqlParameter("@side", way.Stat.Outer_side == side ? 1:0);
            return Conn.executeProc(query, sqlParameters).Tables[0];

            //if (row["dt_from_way"] != DBNull.Value) vagOnStat.DT_from_way = DateTime.Parse(row["dt_from_way"].ToString());
        }
        //TODO: ПЕРЕХОД НА НОВОЕ ОПРЕДЕЛЕНИЕ ИНФОРМАЦИИ ПО ВАГОНАМ
        //private DataTable getVagonsTable(Way way, Side side)
        //{
        //    string str = "";
        //    if (way.Stat.Outer_side == side) 
        //        str = "desc";

        //    string query = string.Format("select vo.*, v.num, v.rod, v.st_otpr, o.abr as owner_, c.name as country, vc.name as cond,  " +
        //    "g.name as gruz, g2.name as gruz_amkr, s.name as shop, t.name as tupik, gd.name as gdstait, nc.name as nazn_country, vc2.name as cond2, vc2.id_cond_after, " +
        //    "p.date_mail, p.n_mail, p.[text], p.nm_stan, p.nm_sobstv " +
        //    "from VAGON_OPERATIONS vo " +
        //    "inner join VAGONS v on vo.id_vagon=v.id_vag " +
        //    "left join OWNERS o on v.id_owner=o.id_owner " +
        //    "left join OWNERS_COUNTRIES c on o.id_country=c.id_own_country "+
        //    "left join VAG_CONDITIONS vc on vo.id_cond=vc.id_cond "+
        //    "left join GRUZS g on vo.id_gruz=g.id_gruz "+
        //    "left join GRUZS g2 on vo.id_gruz_amkr=g2.id_gruz "+
        //    "left join SHOPS s on vo.id_shop_gruz_for=s.id_shop "+
        //    "left join TUPIKI t on vo.id_tupik = t.id_tupik "+
        //    "left join GDSTAIT gd on vo.id_gdstait = gd.id_gdstait "+
        //    "left join NAZN_COUNTRIES nc on vo.id_nazn_country = nc.id_country "+
        //    "left join VAG_CONDITIONS2 vc2 on vo.id_cond2=vc2.id_cond "+
        //    "left join v_p_vozvrat_ip p on p.id = (select top 1 id from v_P_VOZVRAT_IP where n_vag=v.num order by DATE_MAIL desc) "+
        //    "where vo.id_way = @id_way and vo.is_present = 1 " +
        //    "order by vo.num_vag_on_way " + str);
        //    SqlParameter[] sqlParameters = new SqlParameter[1];
        //    sqlParameters[0] = new SqlParameter("@id_way", way.ID);

        //    return Conn.executeSelectQuery(query, sqlParameters).Tables[0];

        //                    //if (row["dt_from_way"] != DBNull.Value) vagOnStat.DT_from_way = DateTime.Parse(row["dt_from_way"].ToString());
        //}

        private DataTable getVagonsTable(GruzFront gf)
        {
            string query = "[RailCars].[GetOnStatGruzFrontWagons]";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@gf", gf.ID);
            return Conn.executeProc(query, sqlParameters).Tables[0];
        }
        //private DataTable getVagonsTable(GruzFront gf)
        //{
        //    string query = string.Format("select vo.*, v.num, v.rod, v.st_otpr, o.abr as owner_, c.name as country, vc.name as cond,  " +
        //    "g.name as gruz, g2.name as gruz_amkr, s.name as shop, t.name as tupik, gd.name as gdstait, nc.name as nazn_country, vc2.name as cond2, vc2.id_cond_after, " +
        //    "p.date_mail, p.n_mail, p.[text], p.nm_stan, p.nm_sobstv " +
        //    "from VAGON_OPERATIONS vo " +
        //    "inner join VAGONS v on vo.id_vagon=v.id_vag " +
        //    "left join OWNERS o on v.id_owner=o.id_owner " +
        //    "left join OWNERS_COUNTRIES c on o.id_country=c.id_own_country "+
        //    "left join VAG_CONDITIONS vc on vo.id_cond=vc.id_cond "+
        //    "left join GRUZS g on vo.id_gruz=g.id_gruz "+
        //    "left join GRUZS g2 on vo.id_gruz_amkr=g2.id_gruz "+
        //    "left join SHOPS s on vo.id_shop_gruz_for=s.id_shop "+
        //    "left join TUPIKI t on vo.id_tupik = t.id_tupik "+
        //    "left join GDSTAIT gd on vo.id_gdstait = gd.id_gdstait "+
        //    "left join NAZN_COUNTRIES nc on vo.id_nazn_country = nc.id_country "+
        //    "left join VAG_CONDITIONS2 vc2 on vo.id_cond2=vc2.id_cond "+
        //    "left join v_p_vozvrat_ip p on p.id = (select top 1 id from v_P_VOZVRAT_IP where n_vag=v.num order by DATE_MAIL desc) "+
        //    "where vo.st_gruz_front=@gf and vo.is_hist=0 and vo.is_present = 0 " +
        //    "order by CAST(FORMAT(vo.dt_from_stat,'yyyy-MM-dd HH:mm:ss') AS datetime), vo.st_lock_order");
        //    SqlParameter[] sqlParameters = new SqlParameter[1];
        //    sqlParameters[0] = new SqlParameter("@gf", gf.ID);

        //    return Conn.executeSelectQuery(query, sqlParameters).Tables[0];
        //}

        private DataTable getVagonsTable(Shop shop)
        {
            string query = "[RailCars].[GetOnStatShopWagons]";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@shop", shop.ID);
            return Conn.executeProc(query, sqlParameters).Tables[0];
        }
        //private DataTable getVagonsTable(Shop shop)
        //{
        //    string query = string.Format("select vo.*, v.num, v.rod, v.st_otpr, o.abr as owner_, c.name as country, vc.name as cond,  " +
        //    "g.name as gruz, g2.name as gruz_amkr, s.name as shop, t.name as tupik, gd.name as gdstait, nc.name as nazn_country, vc2.name as cond2, vc2.id_cond_after, " +
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
        //    "where vo.st_shop=@shop and vo.is_hist = 0 " +
        //    "order by CAST(FORMAT(vo.dt_from_stat,'yyyy-MM-dd HH:mm:ss') AS datetime), vo.st_lock_order");
        //    SqlParameter[] sqlParameters = new SqlParameter[1];
        //    sqlParameters[0] = new SqlParameter("@shop", shop.ID);

        //    return Conn.executeSelectQuery(query, sqlParameters).Tables[0];            
        //}

        public List<VagOnStat> getVagons(Way way, Side side)
        {
            List<VagOnStat> list = (base.getVagons(getVagonsTable(way, side))).Select(parent => new VagOnStat(parent)).ToList();
            return list;
        }

        public List<VagOnStat> getVagons(GruzFront gf)
        {
            List<VagOnStat> list = (base.getVagons(getVagonsTable(gf))).Select(parent => new VagOnStat(parent)).ToList();
            
            int i = 0;
            foreach (VagOnStat item in list)
            {
                list[i].Num_vag_on_way = list.IndexOf(item) + 1;

                i++;
            }

            return list;
        }

        public List<VagOnStat> getVagons(Shop shop)
        {
            List<VagOnStat> list = (base.getVagons(getVagonsTable(shop))).Select(parent => new VagOnStat(parent)).ToList();
            return list;
        }
    }
}
