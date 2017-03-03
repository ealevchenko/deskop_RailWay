using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class VagOperations
    {
        // Общая информация
        public int id_oper { get; set; }// = -1; //
        public DateTime? dt_uz { get; set; }//; //
        public DateTime? dt_amkr { get; set; }//; //
        public DateTime? dt_out_amkr { get; set; }//; //
        public int id_sostav { get; set; }// = -1; //
        public int id_vag { get; set; }// = -1;    //TODO: Убрать переход на новый справочник Wagons
        public int num_vag { get; set; }// = -1;

        public DateTime? dt_on_stat { get; set; }//;
        public DateTime? dt_from_stat { get; set; }//;

        public DateTime? dt_on_way { get; set; }//;
        public DateTime? dt_from_way { get; set; }//;
        public int num_vag_on_way { get; set; }//;

        public int id_godn { get; set; }// = -1;
        public string godn { get; set; }// = "";
        public Cond cond { get; set; }// = new Cond();

        public string grvuSAP { get; set; }// = ""; // грузоподъемность
        public string ngruSAP { get; set; }// = ""; // грузополучатель

        // Справочник Wagons
        public string rod { get; set; }// = "";      
        public string owner { get; set; }// = "";
        public string own_country { get; set; }// = ""; //TODO: Убрать переход на новый справочник САП вх. поставки

        //Cправочник САП вх. поставки
        public int id_gruz { get; set; }// = -1;           //TODO: Убрать переход на новый справочник САП вх. поставки
        public string gruz { get; set; }// = "";           //TODO: Убрать переход на новый справочник САП вх. поставки
        public double weight_gruz { get; set; }//;         //TODO: Убрать переход на новый справочник САП вх. поставки
        public int id_ceh_gruz { get; set; }// = -1;       //TODO: Убрать переход на новый справочник САП вх. поставки
        public string ceh_gruz { get; set; }//;            //TODO: Убрать переход на новый справочник САП вх. поставки
        public string outer_station { get; set; }// = ""; //TODO: Убрать переход на новый справочник САП вх. поставки

        public string wagon_country { get; set; }

        public int? NumNakl { get; set; } // Номер накладной
        public string CargoName { get; set; } // Название груза по данным  МТ
        public double WeightDoc { get; set; } // Вес грузза по данным  МТ перезапишет САП по документам
        public int? DocNumReweighing { get; set; } // Номер отвесной по данным САП
        public DateTime? DocDataReweighing { get; set; } // дата отвесной по данным САП
        public double? WeightReweighing { get; set; } // Перевеска по по данным САП
        public DateTime? DateTimeReweighing { get; set; } // дата перевески по данным САП
        public string CodeMaterial { get; set; } // код материала по данным САП
        public string NameMaterial { get; set; } // название материала по данным САП
        public string CodeStationShipment { get; set; } // код станции отпр. по данным САП
        public string NameStationShipment { get; set; } // название станции отпр. по данным САП
        public string CodeShop { get; set; } // код цеха получателя по данным САП
        public string NameShop { get; set; } // название цеха получателя по данным САП
        public string CodeNewShop { get; set; } // код переадресовки цеха получателя по данным САП
        public string NameNewShop { get; set; } // название переадресовки цеха получателя по данным САП
        public string PermissionUnload { get; set; } // разрешение на выгрузку по данным САП
        public bool Step1 { get; set; } // перенесены данные сап по данным САП
        public bool Step2 { get; set; } // перенесены данные по весовым по данным САП


        //Cправочник САП выход. поставки //TODO: Переделать переход на новый справочник САП выход. поставки
        public int id_gruz_amkr { get; set; }// = -1;
        public string gruz_amkr { get; set; }// = "";         

        public int id_tupik { get; set; }// = -1;
        public string tupik { get; set; }// = "";
        public int id_gdstait { get; set; }// = -1;
        public string gdstait { get; set; }// = "";
        public int id_nazn_country { get; set; }// = -1;
        public string nazn_country { get; set; }// = "";
        public string note { get; set; }// = "";

        // Справочник писем
        public DateTime? MailDate { get; set; }// { get; set; }
        public string MailNum { get; set; }// { get; set; }
        public string MailText { get; set; }// { get; set; }
        public string MailStat { get; set; }// { get; set; }
        public string MailSobstv { get; set; }// { get; set; }

        protected void Create(VagOperations vo) 
        {
            this.id_oper = vo.id_oper;//
            this.dt_uz = vo.dt_uz;//
            this.dt_amkr = vo.dt_amkr;//
            this.dt_out_amkr = vo.dt_out_amkr;//
            this.id_sostav = vo.id_sostav;//
            this.id_vag = vo.id_vag;//TODO: Убрать переход на новый справочник Wagons
            this.num_vag = vo.num_vag;//

            this.dt_on_stat = vo.dt_on_stat;//
            this.dt_from_stat = vo.dt_from_stat;//

            this.dt_on_way = vo.dt_on_way;//
            this.dt_from_way = vo.dt_from_way;//
            this.num_vag_on_way = vo.num_vag_on_way;//

            this.id_godn = vo.id_godn;//
            this.godn = vo.godn;//
            this.cond = vo.cond;//

            this.grvuSAP = vo.grvuSAP;//
            this.ngruSAP = vo.ngruSAP;//

            // Справочник Wagons
            this.rod = vo.rod;//     
            this.owner = vo.owner;//
            this.own_country = vo.own_country;//

            this.wagon_country = vo.wagon_country; // страна вагона

            //Cправочник САП вх. поставки
            this.id_gruz = vo.id_gruz;//
            this.gruz = vo.gruz;//
            this.weight_gruz = vo.weight_gruz;//
            this.id_ceh_gruz = vo.id_ceh_gruz;//
            this.ceh_gruz = vo.ceh_gruz;//
            this.outer_station = vo.outer_station;//


            this.NumNakl = vo.NumNakl; // номер накладной
            this.CargoName = vo.CargoName; // Груз
            this.WeightDoc = vo.WeightDoc;
            this.DocNumReweighing = vo.DocNumReweighing;
            this.DocDataReweighing = vo.DocDataReweighing;
            this.WeightReweighing = vo.WeightReweighing;
            this.DateTimeReweighing = vo.DateTimeReweighing;
            this.CodeMaterial = vo.CodeMaterial;
            this.NameMaterial = vo.NameMaterial;
            this.CodeStationShipment = vo.CodeStationShipment;
            this.NameStationShipment = vo.NameStationShipment;
            this.CodeShop = vo.CodeShop;
            this.NameShop = vo.NameShop;
            this.CodeNewShop = vo.CodeNewShop;
            this.NameNewShop = vo.NameNewShop;
            this.PermissionUnload = vo.PermissionUnload;
            this.Step1 = vo.Step1;
            this.Step2 = vo.Step2;



            //Cправочник САП выход. поставки //TODO: Переделать переход на новый справочник САП выход. поставки
            this.id_gruz_amkr = vo.id_gruz_amkr;//
            this.gruz_amkr = vo.gruz_amkr;//        

            this.id_tupik = vo.id_tupik;//
            this.tupik = vo.tupik;//
            this.id_gdstait = vo.id_gdstait;//
            this.gdstait = vo.gdstait;//
            this.id_nazn_country = vo.id_nazn_country;//
            this.nazn_country = vo.nazn_country;//
            this.note = vo.note;//

            // Справочник писем
            this.MailDate = vo.MailDate;//
            this.MailNum = vo.MailNum;//
            this.MailText = vo.MailText;//
            this.MailStat = vo.MailStat;//
            this.MailSobstv = vo.MailSobstv;//
        }

      

        //public DateTime Dt_on_stat { get; set; }
        //public DateTime? Dt_from_stat { get; set; }

        //public int id_oper { get { return id_oper; } set { id_oper = value; } }
        //public DateTime DT_uz { get { return dt_uz; } set { dt_uz = value; } }  //Дата готовности отправки с УЗ
        //public DateTime DT_amkr { get { return dt_amkr; } set { dt_amkr = value; } }
        //public DateTime Dt_out_amkr { get { return dt_out_amkr; } set { dt_out_amkr = value; } }
        //public int IDSostav { get { return id_sostav; } set { id_sostav = value; } }
        //public int Num_vag_on_way { get { return num_vag_on_way; } set { num_vag_on_way = value; } }

        //public int Id_vag { get { return id_vag; } set { id_vag = value; } }

        //public int Num_vag { get { return num_vag; } set { num_vag = value; } }

        //public string Rod
        //{
        //    get { return rod; }
        //    set { rod = value; }
        //}
       
        ////Дата готовности отправки с УЗ
        
       
        

        //public DateTime DT_on_way
        //{
        //    get { return dt_on_way; }
        //    set { dt_on_way = value; }
        //}

        //public string Owner { get { return owner; } set { owner = value; } }

        //public string Own_country { get { return own_country; } set { own_country = value; } }

        //public int Id_godn { get { return id_godn; } set { id_godn = value; } }

        //public string Godn { get { return godn; } set { godn = value; } }

        //public Cond Cond
        //{
        //    get { return cond; }
        //    set { cond = value; }
        //}

        //public int Id_gruz
        //{
        //    get { return id_gruz; }
        //    set { id_gruz = value; }
        //}

        //public string Gruz { get { return gruz; } set { gruz = value; } }

        //public int Id_gruz_amkr
        //{
        //    get { return id_gruz_amkr; }
        //    set { id_gruz_amkr = value; }
        //}


        //public string Gruz_amkr
        //{
        //    get { return gruz_amkr; }
        //    set { gruz_amkr = value; }
        //}

        //public double Weight_gruz { get { return weight_gruz; } set { weight_gruz = value; } }

        //public int Id_ceh_gruz { get { return id_ceh_gruz; } set { id_ceh_gruz = value; } }

        //public string CehGruz { get { return ceh_gruz; } set { ceh_gruz = value; } }

        //public int Id_tupik
        //{
        //    get { return id_tupik; }
        //    set { id_tupik = value; }
        //}

        //public string Tupik
        //{
        //    get { return tupik; }
        //    set { tupik = value; }
        //}

        //public int Id_gdstait
        //{
        //    get { return id_gdstait; }
        //    set { id_gdstait = value; }
        //}


        //public string Gdstait
        //{
        //    get { return gdstait; }
        //    set { gdstait = value; }
        //}

        //public int Id_nazn_country
        //{
        //    get { return id_nazn_country; }
        //    set { id_nazn_country = value; }
        //}


        //public string Nazn_country
        //{
        //    get { return nazn_country; }
        //    set { nazn_country = value; }
        //}


        //public string Note
        //{
        //    get { return note; }
        //    set { note = value; }
        //}

        //public string Outer_station
        //{
        //    get { return outer_station; }
        //    set { outer_station = value; }
        //}

        //public string NgruSAP
        //{
        //    get { return ngruSAP; }
        //    set { ngruSAP = value; }
        //}

        //public string GrvuSAP
        //{
        //    get { return grvuSAP; }
        //    set { grvuSAP = value; }
        //}
    }
}
