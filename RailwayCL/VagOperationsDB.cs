using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using EFRailCars.Entities;
using EFRailCars.Railcars;

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
            sqlParameters[0] = new SqlParameter("@id_vagon", vo.id_vag);
            sqlParameters[1] = new SqlParameter("@id_stat", way.Stat.ID);
            sqlParameters[2] = new SqlParameter("@id_way", way.ID);
            sqlParameters[3] = new SqlParameter("@num_vag_on_way", vo.num_vag_on_way); // Номер вагона на пути
            if (vo.dt_amkr < DateTime.Parse("1900-01-01 00:00") | (vo.dt_amkr == null))
                sqlParameters[4] = new SqlParameter("@dt_amkr", DBNull.Value);
            else sqlParameters[4] = new SqlParameter("@dt_amkr", vo.dt_amkr);
            sqlParameters[5] = new SqlParameter("@id_oper", vo.id_oper);
            if (vo.id_godn != -1)
                sqlParameters[6] = new SqlParameter("@id_cond", vo.id_godn); // годность по прибытию
            else sqlParameters[6] = new SqlParameter("@id_cond", DBNull.Value);
            if (vo.id_gruz != -1)
                sqlParameters[7] = new SqlParameter("@id_gruz", vo.id_gruz); // груз
            else sqlParameters[7] = new SqlParameter("@id_gruz", DBNull.Value);
            sqlParameters[8] = new SqlParameter("@weight_gruz", vo.weight_gruz); // вес
            if (vo.id_ceh_gruz != -1)
                sqlParameters[9] = new SqlParameter("@id_shop_gruz_for", vo.id_ceh_gruz); // 
            else sqlParameters[9] = new SqlParameter("@id_shop_gruz_for", DBNull.Value);
            if (vo.id_tupik != -1)
                sqlParameters[10] = new SqlParameter("@id_tupik", vo.id_tupik); //тупик
            else sqlParameters[10] = new SqlParameter("@id_tupik", DBNull.Value);
            if (vo.id_nazn_country != -1)
                sqlParameters[11] = new SqlParameter("@id_nazn_country", vo.id_nazn_country); // страна назначения
            else sqlParameters[11] = new SqlParameter("@id_nazn_country", DBNull.Value);
            if (vo.id_gdstait != -1)
                sqlParameters[12] = new SqlParameter("@id_gdstait", vo.id_gdstait); //станция грузов доставки
            else sqlParameters[12] = new SqlParameter("@id_gdstait", DBNull.Value);
            sqlParameters[13] = new SqlParameter("@note", vo.note); // примечание груз
            sqlParameters[14] = new SqlParameter("@grvuSAP", vo.grvuSAP);   //
            sqlParameters[15] = new SqlParameter("@ngruSAP", vo.ngruSAP);   //
            sqlParameters[16] = new SqlParameter("@num_vagon", vo.num_vag);   //
            return sqlParameters;
        }
        /// <summary>
        ///  изменить нумерацию вагонов на пути на который поставили вагоны
        /// </summary>
        /// <param name="new_count"></param>
        /// <param name="first_id_oper"></param>
        /// <param name="way"></param>
        /// <returns></returns>
        public bool changeVagNumsWayOn(int new_count, int first_id_oper, Way way)
        {
            //string query = string.Format("update VAGON_OPERATIONS set num_vag_on_way = num_vag_on_way + @new_count where id_oper < @first_id_oper " +
            //    "and id_way = @id_way and is_present = 1");
            //TODO: Изменил принцип нумерации вагонов на пути куда переставили вагоны после маневра
            // Получим вагоны по которым нужно изменить нумерацию
            //bool result = true;
            //List<VAGON_OPERATIONS> list_vag = rc_vo.GetVagonsOperations().Where(o=>o.id_oper < first_id_oper & o.id_way == way.ID & o.is_present==1).OrderBy(o=>o.num_vag_on_way).ToList();
            //int num = new_count+1;
            //foreach (VAGON_OPERATIONS wag in list_vag) 
            //{
            //    wag.num_vag_on_way = num;
            //    num++;
            //    int res = rc_vo.SaveVagonsOperations(wag);
            //    if (res < 0) result = false;
            //}
            //return result;
            string query = string.Format("update VAGON_OPERATIONS set num_vag_on_way = num_vag_on_way + @new_count where id_oper < @first_id_oper " +
                "and id_way = @id_way and is_present = 1");
            SqlParameter[] sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@new_count", new_count);
            sqlParameters[1] = new SqlParameter("@first_id_oper", first_id_oper);
            sqlParameters[2] = new SqlParameter("@id_way", way.ID);
            return Conn.executeNonQueryCommand(query, sqlParameters);
        }
        /// <summary>
        /// изменить нумерацию вагонов на пути с которого хзабрали вагоны
        /// </summary>
        /// <param name="num_vag_on_way"></param>
        /// <param name="id_oper"></param>
        /// <returns></returns>
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
                VagOperations vagOperations = new VagOperations()
                {
                    // Общая информация

                    //id_oper = -1,
                    //TODO:Сделал определение id_oper (для закрытия старой записи)
                    id_oper = row["id_oper"] != DBNull.Value ? int.Parse(row["id_oper"].ToString()) : -1,
                    dt_uz = row["dt_uz"] != DBNull.Value ? row["dt_uz"] as DateTime? : null,
                    dt_amkr = row["dt_amkr"] != DBNull.Value ? row["dt_amkr"] as DateTime? : null,
                    dt_out_amkr = row["dt_out_amkr"] != DBNull.Value ? row["dt_out_amkr"] as DateTime? : null,
                    id_sostav = row["IDSostav"] != DBNull.Value ? int.Parse(row["IDSostav"].ToString()): -1,
                    id_vag = row["id_vagon"] != DBNull.Value ? int.Parse(row["id_vagon"].ToString()): -1,
                    num_vag = row["num_vagon"] != DBNull.Value ? int.Parse(row["num_vagon"].ToString()) : -1,
                    dt_on_stat = row["dt_on_stat"] != DBNull.Value ? row["dt_on_stat"] as DateTime? : null,
                    dt_from_stat = row["dt_from_stat"] != DBNull.Value ? row["dt_from_stat"] as DateTime? : null,
                    dt_on_way = row["dt_on_way"] != DBNull.Value ? row["dt_on_way"] as DateTime? : null,
                    dt_from_way = row["dt_from_way"] != DBNull.Value ? row["dt_from_way"] as DateTime? : null,
                    num_vag_on_way = row["num_vag_on_way"] != DBNull.Value ? int.Parse(row["num_vag_on_way"].ToString()) : -1,
                    id_godn = row["id_cond"] != DBNull.Value ? int.Parse(row["id_cond"].ToString()) : -1,
                    godn = row["cond"] != DBNull.Value ? row["cond"] as string : "",
                    
                    cond = new Cond() {
                        Id = row["id_cond2"] != DBNull.Value ? int.Parse(row["id_cond2"].ToString()) : -1,
                        Name = row["cond2"] != DBNull.Value ? row["cond2"] as string : "",
                        Id_cond_after = row["id_cond_after"] != DBNull.Value ? int.Parse(row["id_cond_after"].ToString()) : -1,
                    },
                    grvuSAP = row["grvu_SAP"] != DBNull.Value ? row["grvu_SAP"] as string : "",
                    ngruSAP = row["ngru_SAP"] != DBNull.Value ? row["ngru_SAP"] as string : "",


                    // Справочник Wagons
                    rod = row["rod"] != DBNull.Value ? row["rod"] as string : "",
                    owner = row["owner_"] != DBNull.Value ? row["owner_"] as string : "",
                    own_country = row["country"] != DBNull.Value ? row["country"] as string : "",

                    wagon_country = row["wagon_country"] != DBNull.Value ? row["wagon_country"] as string : "",

                    //Cправочник САП вх. поставки
                    id_gruz = row["id_gruz"] != DBNull.Value ? int.Parse(row["id_gruz"].ToString()) : -1,
                    gruz = row["gruz"] != DBNull.Value ? row["gruz"] as string : "",
                    weight_gruz = row["weight_gruz"] != DBNull.Value ? Double.Parse(row["weight_gruz"].ToString()) : 0,
                    id_ceh_gruz = row["id_shop_gruz_for"] != DBNull.Value ? int.Parse(row["id_shop_gruz_for"].ToString()) : -1,
                    ceh_gruz = row["shop"] != DBNull.Value ? row["shop"] as string : "",
                    outer_station = row["st_otpr"] != DBNull.Value ? row["st_otpr"] as string : "",

                    NumNakl = row["NumNakl"] != DBNull.Value ? row["NumNakl"] as int? : null, // Номер накладной
                    CargoName = row["CargoName"] != DBNull.Value ? row["CargoName"].ToString().Trim() : "",
                    WeightDoc = row["WeightDoc"] != DBNull.Value ? Double.Parse(row["WeightDoc"].ToString()) : 0,
                    DocNumReweighing = row["DocNumReweighing"] != DBNull.Value ? row["DocNumReweighing"] as int? : null, // Номер отвесной
                    DocDataReweighing = row["DocDataReweighing"] != DBNull.Value ? row["DocDataReweighing"] as DateTime? : null,
                    WeightReweighing = row["WeightReweighing"] != DBNull.Value ? row["WeightReweighing"] as double? : null,
                    DateTimeReweighing = row["DateTimeReweighing"] != DBNull.Value ? row["DateTimeReweighing"] as DateTime? : null,
                    CodeMaterial = row["CodeMaterial"] != DBNull.Value ? row["CodeMaterial"].ToString().Trim() : "", 
                    NameMaterial = row["NameMaterial"] != DBNull.Value ? row["NameMaterial"].ToString().Trim() : "",
                    CodeStationShipment = row["CodeStationShipment"] != DBNull.Value ? row["CodeStationShipment"].ToString().Trim() : "",
                    NameStationShipment = row["NameStationShipment"] != DBNull.Value ? row["NameStationShipment"].ToString().Trim() : "",
                    CodeShop = row["CodeShop"] != DBNull.Value ? row["CodeShop"].ToString().Trim() : "",
                    NameShop = row["NameShop"] != DBNull.Value ? row["NameShop"].ToString().Trim() : "",
                    CodeNewShop = row["CodeNewShop"] != DBNull.Value ? row["CodeNewShop"].ToString().Trim() : "",
                    NameNewShop = row["NameNewShop"] != DBNull.Value ? row["NameNewShop"].ToString().Trim() : "",
                    PermissionUnload = row["PermissionUnload"] != DBNull.Value ? bool.Parse(row["PermissionUnload"].ToString()) : false,
                    Step1 = row["Step1"] != DBNull.Value ? bool.Parse(row["Step1"].ToString()) : false,
                    Step2 = row["Step2"] != DBNull.Value ? bool.Parse(row["Step2"].ToString()) : false,


                    //Cправочник САП выход. поставки //TODO: Переделать переход на новый справочник САП выход. поставки
                    id_gruz_amkr = row["id_gruz_amkr"] != DBNull.Value ? int.Parse(row["id_gruz_amkr"].ToString()) : -1,
                    gruz_amkr = row["gruz_amkr"] != DBNull.Value ? row["gruz_amkr"] as string : "",
                    id_tupik = row["id_tupik"] != DBNull.Value ? int.Parse(row["id_tupik"].ToString()) : -1,
                    tupik = row["tupik"] != DBNull.Value ? row["tupik"] as string : "",
                    id_gdstait = row["id_gdstait"] != DBNull.Value ? int.Parse(row["id_gdstait"].ToString()) : -1,
                    gdstait = row["gdstait"] != DBNull.Value ? row["gdstait"] as string : "",
                    id_nazn_country = row["id_nazn_country"] != DBNull.Value ? int.Parse(row["id_nazn_country"].ToString()) : -1,
                    nazn_country = row["nazn_country"] != DBNull.Value ? row["nazn_country"] as string : "",
                    note = row["note"] != DBNull.Value ? row["note"] as string : "",

                    // Справочник писем
                    MailDate = row["date_mail"] != DBNull.Value ? row["date_mail"] as DateTime? : null,
                    MailNum = row["n_mail"] != DBNull.Value ? row["n_mail"].ToString().Trim() : "",
                    MailText = row["text"] != DBNull.Value ? row["text"].ToString().Trim() : "",
                    MailStat = row["nm_stan"] != DBNull.Value ? row["nm_stan"].ToString().Trim() : "",
                    MailSobstv = row["nm_sobstv"] != DBNull.Value ? row["nm_sobstv"].ToString().Trim() : "",
                };
                list.Add(vagOperations);
            }
            return list;
        }

        //protected List<VagOperations> getVagons1(DataTable table)
        //{
        //    List<VagOperations> list = new List<VagOperations>();

        //    foreach (DataRow row in table.Rows)
        //    {
        //        VagOperations vagOperations = new VagOperations();
        //        // Общая информация

        //        vagOperations.id_oper = Int32.Parse(row["id_oper"].ToString());
        //        if (row["dt_uz"] != DBNull.Value) vagOperations.DT_uz = DateTime.Parse(row["dt_uz"].ToString());        // отправка с УЗ
        //        if (row["dt_amkr"] != DBNull.Value) vagOperations.dt_amkr = DateTime.Parse(row["dt_amkr"].ToString());
        //        if (row["dt_out_amkr"] != DBNull.Value) vagOperations.Dt_out_amkr = DateTime.Parse(row["dt_out_amkr"].ToString());
        //        vagOperations.IDSostav = row["IDSostav"] != DBNull.Value ? int.Parse(row["IDSostav"].ToString()) : -1;


        //        if (row["num_vag_on_way"] != DBNull.Value) vagOperations.num_vag_on_way = Int32.Parse(row["num_vag_on_way"].ToString());//table.Rows.IndexOf(row) + 1;
        //        if (row["id_vagon"] != DBNull.Value) vagOperations.id_vag = Int32.Parse(row["id_vagon"].ToString());  //TODO: Убрать переход на новый справочник Wagons
        //        if (row["num_vagon"] != DBNull.Value) vagOperations.num_vag = Int32.Parse(row["num_vagon"].ToString());
        //        if (row["rod"] != DBNull.Value) vagOperations.rod = row["rod"].ToString().Trim();


        //        if (row["dt_on_way"] != DBNull.Value) vagOperations.dt_on_way = DateTime.Parse(row["dt_on_way"].ToString());
        //        if (row["dt_on_stat"] != DBNull.Value) vagOperations.dt_on_stat = DateTime.Parse(row["dt_on_stat"].ToString());
        //        if (row["dt_from_stat"] != DBNull.Value) vagOperations.Dt_from_stat = DateTime.Parse(row["dt_from_stat"].ToString());
        //        else vagOperations.Dt_from_stat = null;
        //        // Справочник Wagons

        //        //Cправочник САП вх. поставки

        //        //Cправочник САП вsх. поставки

        //        if (row["owner_"] != DBNull.Value) vagOperations.owner = row["owner_"].ToString().Trim(); // абр. владельца
        //        if (row["country"] != DBNull.Value) vagOperations.own_country = row["country"].ToString().Trim(); //TODO: Убрать переход на новый справочник САП вх. поставки
        //        if (row["Country"] != DBNull.Value) vagOperations.own_country = row["Country"].ToString().Trim(); // Стана вагона берем из справочника САП вх. поставки
        //        if (row["id_cond"] != DBNull.Value) vagOperations.id_godn = Int32.Parse(row["id_cond"].ToString());
        //        if (row["cond"] != DBNull.Value) vagOperations.godn = row["cond"].ToString().Trim();
        //        if (row["id_cond2"] != DBNull.Value) vagOperations.cond.Id = Int32.Parse(row["id_cond2"].ToString());
        //        if (row["cond2"] != DBNull.Value) vagOperations.cond.Name = row["cond2"].ToString().Trim();
        //        if (row["id_cond_after"] != DBNull.Value) vagOperations.cond.Id_cond_after = Int32.Parse(row["id_cond_after"].ToString());
        //        if (row["id_gruz"] != DBNull.Value) vagOperations.id_gruz = Int32.Parse(row["id_gruz"].ToString());//TODO: Убрать переход на новый справочник САП вх. поставки
        //        if (row["gruz"] != DBNull.Value) vagOperations.gruz = row["gruz"].ToString().Trim(); //TODO: Убрать переход на новый справочник САП вх. поставки


        //        if (row["id_gruz_amkr"] != DBNull.Value) vagOperations.id_gruz_amkr = Int32.Parse(row["id_gruz_amkr"].ToString()); //TODO: В дальнейшем переход на новый справочник САП выход. поставки
        //        if (row["gruz_amkr"] != DBNull.Value) vagOperations.gruz_amkr = row["gruz_amkr"].ToString().Trim(); //TODO: В дальнейшем переход на новый справочник САП выход. поставки


        //        if (row["weight_gruz"] != DBNull.Value) vagOperations.weight_gruz = Double.Parse(row["weight_gruz"].ToString());//TODO: Убрать переход на новый справочник САП вх. поставки
        //        if (row["id_shop_gruz_for"] != DBNull.Value) vagOperations.id_ceh_gruz = Int32.Parse(row["id_shop_gruz_for"].ToString());//TODO: Убрать переход на новый справочник САП вх. поставки
        //        if (row["shop"] != DBNull.Value) vagOperations.ceh_gruz = row["shop"].ToString().Trim();//TODO: Убрать переход на новый справочник САП вх. поставки
        //        if (row["id_tupik"] != DBNull.Value)
        //        {
        //            vagOperations.id_tupik = Int32.Parse(row["id_tupik"].ToString());
        //            vagOperations.tupik = row["tupik"].ToString().Trim();
        //        }
        //        if (row["id_gdstait"] != DBNull.Value)
        //        {
        //            vagOperations.id_gdstait = Int32.Parse(row["id_gdstait"].ToString());
        //            vagOperations.gdstait = row["gdstait"].ToString().Trim();
        //        }
        //        if (row["id_nazn_country"] != DBNull.Value)//TODO: В дальнейшем переход на новый справочник САП выход. поставки
        //        {
        //            vagOperations.id_nazn_country = Int32.Parse(row["id_nazn_country"].ToString());
        //            vagOperations.nazn_country = row["nazn_country"].ToString().Trim();
        //        }
        //        if (row["note"] != DBNull.Value) vagOperations.note = row["note"].ToString().Trim();//TODO: В дальнейшем переход на новый справочник САП выход. поставки

        //        if (row["st_otpr"] != DBNull.Value) vagOperations.outer_station = row["st_otpr"].ToString().Trim();//TODO: Убрать переход на новый справочник САП вх. поставки

        //        if (row["grvu_SAP"] != DBNull.Value) vagOperations.grvuSAP = row["grvu_SAP"].ToString().Trim();
        //        if (row["ngru_SAP"] != DBNull.Value) vagOperations.ngruSAP = row["ngru_SAP"].ToString().Trim();

        //        if (row["date_mail"] != DBNull.Value) vagOperations.MailDate = DateTime.Parse(row["date_mail"].ToString());
        //        if (row["n_mail"] != DBNull.Value) vagOperations.MailNum = row["n_mail"].ToString().Trim();
        //        if (row["text"] != DBNull.Value) vagOperations.MailText = row["text"].ToString().Trim();
        //        if (row["nm_stan"] != DBNull.Value) vagOperations.MailStat = row["nm_stan"].ToString().Trim();
        //        if (row["nm_sobstv"] != DBNull.Value) vagOperations.MailSobstv = row["nm_sobstv"].ToString().Trim();

        //        list.Add(vagOperations);
        //    }

        //    return list;
        //}

        public bool changeNumVagsAfterCancel(Way way, Train train, List<VagOperations> vagons)
        {
            string str = "";
            if (vagons.Count > 0)
            {
                str = " and id_oper in (";
                foreach (VagOperations vag in vagons)
                {
                    if (vagons.IndexOf(vag) == vagons.Count - 1)
                        str += vag.id_oper.ToString() + ")";
                    else str += vag.id_oper.ToString() + ",";
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
