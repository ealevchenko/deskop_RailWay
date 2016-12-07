using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace RailwayCL
{
    public class VagOperationsDB : DB
    {
        /// <summary>
        /// Параметры для добавления информации по вагонам
        /// </summary>
        /// <param name="vo"></param>
        /// <param name="way"></param>
        /// <param name="addParamsCount"></param>
        /// <returns></returns>
        protected SqlParameter[] paramsForInsert(VagOperations vo, Way way, int addParamsCount)
        {
            SqlParameter[] sqlParameters = new SqlParameter[17+addParamsCount];
            sqlParameters[0] = new SqlParameter("@id_vagon", vo.Id_vag);
            sqlParameters[1] = new SqlParameter("@id_stat", way.Stat.ID);
            sqlParameters[2] = new SqlParameter("@id_way", way.ID);
            sqlParameters[3] = new SqlParameter("@num_vag_on_way", vo.Num_vag_on_way); // Номер вагона на пути
            if (vo.DT_amkr < DateTime.Parse("1900-01-01 00:00"))
                sqlParameters[4] = new SqlParameter("@dt_amkr", DBNull.Value);
            else sqlParameters[4] = new SqlParameter("@dt_amkr", vo.DT_amkr);
            sqlParameters[5] = new SqlParameter("@id_oper", vo.Id_oper);
            if (vo.Id_godn != -1)
                sqlParameters[6] = new SqlParameter("@id_cond", vo.Id_godn); // годность по прибытию
            else sqlParameters[6] = new SqlParameter("@id_cond", DBNull.Value);
            if (vo.Id_gruz != -1)
                sqlParameters[7] = new SqlParameter("@id_gruz", vo.Id_gruz); // груз
            else sqlParameters[7] = new SqlParameter("@id_gruz", DBNull.Value);
            sqlParameters[8] = new SqlParameter("@weight_gruz", vo.Weight_gruz); // вес
            if (vo.Id_ceh_gruz != -1)
                sqlParameters[9] = new SqlParameter("@id_shop_gruz_for", vo.Id_ceh_gruz); // 
            else sqlParameters[9] = new SqlParameter("@id_shop_gruz_for", DBNull.Value);
            if (vo.Id_tupik != -1)
                sqlParameters[10] = new SqlParameter("@id_tupik", vo.Id_tupik); //тупик
            else sqlParameters[10] = new SqlParameter("@id_tupik", DBNull.Value);
            if (vo.Id_nazn_country != -1)
                sqlParameters[11] = new SqlParameter("@id_nazn_country", vo.Id_nazn_country); // страна назначения
            else sqlParameters[11] = new SqlParameter("@id_nazn_country", DBNull.Value);
            if (vo.Id_gdstait != -1)
                sqlParameters[12] = new SqlParameter("@id_gdstait", vo.Id_gdstait); //станция грузов доставки
            else sqlParameters[12] = new SqlParameter("@id_gdstait", DBNull.Value);
            sqlParameters[13] = new SqlParameter("@note", vo.Note); // примечание груз
            sqlParameters[14] = new SqlParameter("@grvuSAP", vo.GrvuSAP);   //
            sqlParameters[15] = new SqlParameter("@ngruSAP", vo.NgruSAP);   //
            sqlParameters[16] = new SqlParameter("@num_vagon", vo.Num_vag);   //
            return sqlParameters;
        }

        public bool changeVagNumsWayOn(int new_count, int first_id_oper, Way way)
        {
            string query = string.Format("update VAGON_OPERATIONS set num_vag_on_way = num_vag_on_way + @new_count where id_oper < @first_id_oper " +
                "and id_way = @id_way and is_present = 1");
            SqlParameter[] sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@new_count", new_count);
            sqlParameters[1] = new SqlParameter("@first_id_oper", first_id_oper);
            sqlParameters[2] = new SqlParameter("@id_way", way.ID);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }

        public bool changeVagNumsWayFrom(int num_vag_on_way, int id_oper) 
        {
            string query = string.Format("update VAGON_OPERATIONS set num_vag_on_way=@num_vag_on_way " +
                "where id_oper=@id_oper");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@num_vag_on_way", num_vag_on_way);
            sqlParameters[1] = new SqlParameter("@id_oper", id_oper);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }

        protected List<VagOperations> getVagons(DataTable table)
        {
            List<VagOperations> list = new List<VagOperations>();

            foreach (DataRow row in table.Rows)
            {
                VagOperations vagOperations = new VagOperations();
                vagOperations.Id_oper = Int32.Parse(row["id_oper"].ToString());
                if (row["num_vag_on_way"] != DBNull.Value) vagOperations.Num_vag_on_way = Int32.Parse(row["num_vag_on_way"].ToString());//table.Rows.IndexOf(row) + 1;
                if (row["id_vagon"] != DBNull.Value) vagOperations.Id_vag = Int32.Parse(row["id_vagon"].ToString());
                if (row["num_vagon"] != DBNull.Value) vagOperations.Num_vag = Int32.Parse(row["num_vagon"].ToString());
                if (row["rod"] != DBNull.Value) vagOperations.Rod = row["rod"].ToString().Trim();
                if (row["dt_amkr"] != DBNull.Value) vagOperations.DT_amkr = DateTime.Parse(row["dt_amkr"].ToString());
                if (row["dt_uz"] != DBNull.Value) vagOperations.DT_uz = DateTime.Parse(row["dt_uz"].ToString()); // 
                if (row["dt_on_way"] != DBNull.Value) vagOperations.DT_on_way = DateTime.Parse(row["dt_on_way"].ToString());
                if (row["dt_on_stat"] != DBNull.Value) vagOperations.Dt_on_stat = DateTime.Parse(row["dt_on_stat"].ToString());
                if (row["dt_from_stat"] != DBNull.Value) vagOperations.Dt_from_stat = DateTime.Parse(row["dt_from_stat"].ToString());
                else vagOperations.Dt_from_stat = null;
                if (row["owner_"] != DBNull.Value) vagOperations.Owner = row["owner_"].ToString().Trim();
                if (row["country"] != DBNull.Value) vagOperations.Own_country = row["country"].ToString().Trim();
                if (row["id_cond"] != DBNull.Value) vagOperations.Id_godn = Int32.Parse(row["id_cond"].ToString());
                if (row["cond"] != DBNull.Value) vagOperations.Godn = row["cond"].ToString().Trim();
                if (row["id_cond2"] != DBNull.Value) vagOperations.Cond.Id = Int32.Parse(row["id_cond2"].ToString());
                if (row["cond2"] != DBNull.Value) vagOperations.Cond.Name = row["cond2"].ToString().Trim();
                if (row["id_cond_after"] != DBNull.Value) vagOperations.Cond.Id_cond_after = Int32.Parse(row["id_cond_after"].ToString());
                if (row["id_gruz"] != DBNull.Value) vagOperations.Id_gruz = Int32.Parse(row["id_gruz"].ToString());
                if (row["gruz"] != DBNull.Value) vagOperations.Gruz = row["gruz"].ToString().Trim();
                if (row["id_gruz_amkr"] != DBNull.Value) vagOperations.Id_gruz_amkr = Int32.Parse(row["id_gruz_amkr"].ToString());
                if (row["gruz_amkr"] != DBNull.Value) vagOperations.Gruz_amkr = row["gruz_amkr"].ToString().Trim();
                if (row["weight_gruz"] != DBNull.Value) vagOperations.Weight_gruz = Double.Parse(row["weight_gruz"].ToString());
                if (row["id_shop_gruz_for"] != DBNull.Value) vagOperations.Id_ceh_gruz = Int32.Parse(row["id_shop_gruz_for"].ToString());
                if (row["shop"] != DBNull.Value) vagOperations.CehGruz = row["shop"].ToString().Trim();
                if (row["id_tupik"] != DBNull.Value)
                {
                    vagOperations.Id_tupik = Int32.Parse(row["id_tupik"].ToString());
                    vagOperations.Tupik = row["tupik"].ToString().Trim();
                }
                if (row["id_gdstait"] != DBNull.Value)
                {
                    vagOperations.Id_gdstait = Int32.Parse(row["id_gdstait"].ToString());
                    vagOperations.Gdstait = row["gdstait"].ToString().Trim();
                }
                if (row["id_nazn_country"] != DBNull.Value)
                {
                    vagOperations.Id_nazn_country = Int32.Parse(row["id_nazn_country"].ToString());
                    vagOperations.Nazn_country = row["nazn_country"].ToString().Trim();
                }
                if (row["note"] != DBNull.Value) vagOperations.Note = row["note"].ToString().Trim();

                if (row["st_otpr"] != DBNull.Value) vagOperations.Outer_station = row["st_otpr"].ToString().Trim();

                if (row["grvu_SAP"] != DBNull.Value) vagOperations.GrvuSAP = row["grvu_SAP"].ToString().Trim();
                if (row["ngru_SAP"] != DBNull.Value) vagOperations.NgruSAP = row["ngru_SAP"].ToString().Trim();

                if (row["date_mail"] != DBNull.Value) vagOperations.MailDate = DateTime.Parse(row["date_mail"].ToString());
                if (row["n_mail"] != DBNull.Value) vagOperations.MailNum = row["n_mail"].ToString().Trim();
                if (row["text"] != DBNull.Value) vagOperations.MailText = row["text"].ToString().Trim();
                if (row["nm_stan"] != DBNull.Value) vagOperations.MailStat = row["nm_stan"].ToString().Trim();
                if (row["nm_sobstv"] != DBNull.Value) vagOperations.MailSobstv = row["nm_sobstv"].ToString().Trim();

                list.Add(vagOperations);
            }

            return list;
        }

        public bool changeNumVagsAfterCancel(Way way, Train train, List<VagOperations> vagons)
        {
            string str = "";
            if (vagons.Count > 0)
            {
                str = " and id_oper in (";
                foreach (VagOperations vag in vagons)
                {
                    if (vagons.IndexOf(vag) == vagons.Count - 1)
                        str += vag.Id_oper.ToString() + ")";
                    else str += vag.Id_oper.ToString() + ",";
                }
            }
            string query = string.Format("update t1 " +
                "set t1.num_vag_on_way=t2.rowNum " +
                "from VAGON_OPERATIONS as t1 " +
                "inner join (select id_oper as id_oper2, ROW_NUMBER() OVER(order by dt_on_way, num_vag_on_way) as rowNum from VAGON_OPERATIONS where id_way=@id_way and is_hist=0 and " +
                "(is_present=1 or (CAST(FORMAT(dt_from_way,'yyyy-MM-dd HH:mm:ss') AS datetime)=CAST(FORMAT(@dt,'yyyy-MM-dd HH:mm:ss') as datetime))"+str+")) as t2 " +
                "on t1.id_oper=t2.id_oper2 " +
                "where id_way=@id_way and is_hist=0 and " +
                "(is_present=1 or (CAST(FORMAT(dt_from_way,'yyyy-MM-dd HH:mm:ss') AS datetime)=CAST(FORMAT(@dt,'yyyy-MM-dd HH:mm:ss') as datetime)"+str+"))");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@id_way", way.ID);
            sqlParameters[1] = new SqlParameter("@dt", train.DateFromStat);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }

        public Tuple<int, int> findVagLocation(Station stat, int num)
        {
            string query = string.Format("select vo.id_way, vo.num_vag_on_way " +
                "from VAGON_OPERATIONS vo " +
                "inner join VAGONS v on vo.id_vagon=v.id_vag " +
                "and v.num=@num and vo.is_present=1 " +
                "and vo.is_hist=0 and vo.id_stat=@id_stat");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@num", num);
            sqlParameters[1] = new SqlParameter("@id_stat", stat.ID);
            DataTable table = Conn.executeSelectQuery(query, sqlParameters).Tables[0];

            Tuple<int, int> tuple = new Tuple<int, int>(0, 0);
            if (table.Rows.Count > 0 && table.Rows[0]["id_way"] != null && table.Rows[0]["num_vag_on_way"] != null)
                tuple = new Tuple<int, int>(Int32.Parse(table.Rows[0]["id_way"].ToString()), Int32.Parse(table.Rows[0]["num_vag_on_way"].ToString()));
            return tuple;
        }

        public bool deleteVagOperations(int id_oper)
        {
            string query = string.Format("delete FROM [KRR-PA-CNT-Railcars].[dbo].[VAGON_OPERATIONS]  where id_oper=@id_oper ");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@id_oper", id_oper);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }
    }
}
