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
    public class VagSendOthStDB : VagOperationsDB
    {
        //private static VagSendOthStDB vagSendOthStDB;

        //private VagSendOthStDB() { }

        //public static VagSendOthStDB GetInstance()
        //{
        //    // для исключения возможности создания двух объектов 
        //    // при многопоточном приложении
        //    if (vagSendOthStDB == null)
        //    {
        //        lock (typeof(VagSendOthStDB))
        //        {
        //            if (vagSendOthStDB == null)
        //                vagSendOthStDB = new VagSendOthStDB();
        //        }
        //    }

        //    return vagSendOthStDB;
        //}

        private DataTable getVagonsTable(Way way, Side side)
        {
            string query = "[RailCars].[GetSendOthStWagons]";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@idway", way.ID);
            sqlParameters[1] = new SqlParameter("@side", way.Stat.Outer_side == side ? 1 : 0);
            return Conn.executeProc(query, sqlParameters).Tables[0];
        }
        //private DataTable getVagonsTable(Way way, Side side)
        //{
        //    string str = "";
        //    if (way.Stat.Outer_side == side) str = "desc";

        //        string query = string.Format("select vo.*, o.abr as owner_, " +
        //        "c.name as country, vc.name as cond, vc2.name as cond2, vc2.id_cond_after, " +
        //        "g.name as gruz, g2.name as gruz_amkr, v.num, v.rod, v.st_otpr, s.name as shop, t.name as tupik, gd.name as gdstait, nc.name as nazn_country, " +
        //        "p.date_mail, p.n_mail, p.[text], p.nm_stan, p.nm_sobstv " +
        //        "from VAGON_OPERATIONS vo " +
        //        "inner join VAGONS v on vo.id_vagon=v.id_vag " +
        //        "left join OWNERS o on v.id_owner=o.id_owner " +
        //        "left join OWNERS_COUNTRIES c on o.id_country=c.id_own_country " +
        //        "left join VAG_CONDITIONS vc on vo.id_cond=vc.id_cond " +
        //        "left join GRUZS g on vo.id_gruz=g.id_gruz " +
        //        "left join GRUZS g2 on vo.id_gruz_amkr=g2.id_gruz " +
        //        "left join SHOPS s on vo.id_shop_gruz_for=s.id_shop " +
        //        "left join TUPIKI t on vo.id_tupik = t.id_tupik " +
        //        "left join GDSTAIT gd on vo.id_gdstait = gd.id_gdstait " +
        //        "left join NAZN_COUNTRIES nc on vo.id_nazn_country = nc.id_country " +
        //        "left join VAG_CONDITIONS2 vc2 on vo.id_cond2=vc2.id_cond " +
        //        "left join v_p_vozvrat_ip p on p.id = (select top 1 id from v_P_VOZVRAT_IP where n_vag=v.num order by DATE_MAIL desc) "+
        //        "where vo.id_way = @id_way and vo.is_present = 1 " +
        //        "order by vo.num_vag_on_way " + str);
        //        SqlParameter[] sqlParameters = new SqlParameter[1]; ;
        //        sqlParameters[0] = new SqlParameter("@id_way", way.ID);

        //    return Conn.executeSelectQuery(query, sqlParameters).Tables[0];
        //}

        public List<VagSendOthSt> getVagons(Way way, Side side)
        {
            DataTable table = getVagonsTable(way, side);

            List<VagSendOthSt> list = (base.getVagons(table)).Select(parent => new VagSendOthSt(parent)).ToList();

            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                if (row["st_lock_id_stat"] != DBNull.Value) list[i].St_lock_id_stat = Int32.Parse(row["st_lock_id_stat"].ToString());
                if (row["st_lock_order"] != DBNull.Value) list[i].St_lock_order = Int32.Parse(row["st_lock_order"].ToString());
                if (row["st_lock_train"] != DBNull.Value) list[i].St_lock_train = Int32.Parse(row["st_lock_train"].ToString());
                if (row["st_lock_side"] != DBNull.Value) list[i].St_lock_side = (Side)Int32.Parse(row["st_lock_side"].ToString());
                if (row["st_gruz_front"] != DBNull.Value) list[i].St_gruz_front = Int32.Parse(row["st_gruz_front"].ToString());
                if (row["st_shop"] != DBNull.Value) list[i].St_shop = Int32.Parse(row["st_shop"].ToString());
                if (row["st_lock_locom1"] != DBNull.Value) list[i].St_lock_locom1 = Int32.Parse(row["st_lock_locom1"].ToString());
                if (row["st_lock_locom2"] != DBNull.Value) list[i].St_lock_locom2 = Int32.Parse(row["st_lock_locom2"].ToString());

                i++;
            }

            return list;
        }

        public bool addToSend(VagSendOthSt vagSendOthSt)
        {
            string query = string.Format("update VAGON_OPERATIONS set st_lock_id_stat = @id_stat, st_lock_order = @order, " +
            "st_lock_train = @lock_train, st_lock_side = @lock_side, st_gruz_front = "+
            "@gruz_front, st_shop = @st_shop, st_lock_locom1 = @locom1, st_lock_locom2 = @locom2 where id_oper = @id_oper");
            SqlParameter[] sqlParameters = new SqlParameter[9];
            if (vagSendOthSt.St_lock_id_stat == -1)
                sqlParameters[0] = new SqlParameter("@id_stat", DBNull.Value);
            else sqlParameters[0] = new SqlParameter("@id_stat", vagSendOthSt.St_lock_id_stat);
            sqlParameters[1] = new SqlParameter("@order", vagSendOthSt.St_lock_order);
            sqlParameters[2] = new SqlParameter("@lock_train", vagSendOthSt.St_lock_train);
            sqlParameters[3] = new SqlParameter("@id_oper", vagSendOthSt.id_oper);
            sqlParameters[4] = new SqlParameter("@lock_side", (int)vagSendOthSt.St_lock_side);
            sqlParameters[5] = new SqlParameter("@gruz_front", vagSendOthSt.St_gruz_front);
            sqlParameters[6] = new SqlParameter("@st_shop", vagSendOthSt.St_shop);
            sqlParameters[7] = new SqlParameter("@locom1", vagSendOthSt.St_lock_locom1);
            sqlParameters[8] = new SqlParameter("@locom2", vagSendOthSt.St_lock_locom2);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }

        public int getMaxTrainNum()
        {
            string query = string.Format("select coalesce(max(st_lock_train),0) as num from VAGON_OPERATIONS");
            return Int32.Parse(Conn.executeSelectQuery(query, new SqlParameter[0]).Tables[0].Rows[0]["num"].ToString());
        }

        public bool cancelToSend(Way way)
        {
            string query = string.Format("update VAGON_OPERATIONS set st_lock_id_stat = null, st_lock_order = null," +
                "st_lock_train = null, st_lock_side = null, st_lock_locom1 = null, st_lock_locom2 = null, st_shop = null, st_gruz_front = null "+
                "where id_way = @id_way and st_lock_order is not null and is_present=1");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_way", way.ID);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }

        public bool cancelVagToSend(int id_oper)
        {
            string query = string.Format("update VAGON_OPERATIONS set st_lock_id_stat = null, st_lock_order = null,") +
                "st_lock_train = null, st_lock_side = null, st_lock_locom1 = null, st_lock_locom2 = null, "+
                "st_shop = null, st_gruz_front = null where id_oper = @id_oper";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_oper", id_oper);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }
        /// <summary>
        /// Отправка на другую станцию
        /// </summary>
        /// <param name="id_oper"></param>
        /// <param name="id_cond"></param>
        /// <param name="dt"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public bool send(int id_oper, int id_cond, DateTime? dt, DateTime dt2)
        {
            string query = string.Format("update VAGON_OPERATIONS set is_present=0, dt_from_stat=@dt, dt_from_way=@dt2, id_cond2=@id_cond2 "+
                "where id_oper = @id_oper");
            SqlParameter[] sqlParameters = new SqlParameter[4];
            sqlParameters[0] = new SqlParameter("@id_oper", id_oper);
            if (id_cond == -1) sqlParameters[1] = new SqlParameter("@id_cond2", DBNull.Value);
            else sqlParameters[1] = new SqlParameter("@id_cond2", id_cond);
            if (dt == null || dt < DateTime.Parse("1900-01-01 00:00"))
                sqlParameters[2] = new SqlParameter("@dt", DBNull.Value);
            else sqlParameters[2] = new SqlParameter("@dt", dt);
            if (dt2 < DateTime.Parse("1900-01-01 00:00"))
                sqlParameters[3] = new SqlParameter("@dt2", DBNull.Value);
            else sqlParameters[3] = new SqlParameter("@dt2", dt2);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }
    }
}
