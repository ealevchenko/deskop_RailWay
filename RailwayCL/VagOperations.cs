using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class VagOperations
    {
        private int id_oper = -1;       
        private int num_vag_on_way;

        private DateTime dt_uz;  //Дата готовности отправки с УЗ

        private int id_vag = -1;
        private int num_vag = -1;
        private string rod = "";      
        private DateTime dt_on_way;
        public DateTime Dt_on_stat { get; set; }
        public DateTime? Dt_from_stat { get; set; }
        private string owner = "";
        private string own_country = "";
        private int id_godn = -1;
        private string godn = "";
        private Cond cond = new Cond();     
        private int id_gruz = -1;  
        private string gruz = "";
        private DateTime dt_amkr;
        private int id_gruz_amkr = -1;
        private string gruz_amkr = "";    
        private double weight_gruz;
        private int id_ceh_gruz = -1;
        private string ceh_gruz;
        private int id_tupik = -1;
        private string tupik = "";
        private int id_gdstait = -1;
        private string gdstait = "";
        private int id_nazn_country = -1;
        private string nazn_country = "";
        private string note = "";
        private string  outer_station = "";
        private string grvuSAP = ""; // грузоподъемность
        private string ngruSAP = ""; // грузополучатель

        public DateTime MailDate { get; set; }
        public string MailNum { get; set; }
        public string MailText { get; set; }
        public string MailStat { get; set; }
        public string MailSobstv { get; set; }
      

        public int Id_oper
        {
            get { return id_oper; }
            set { id_oper = value; }
        }

        public int Num_vag_on_way { get { return num_vag_on_way; } set { num_vag_on_way = value; } }

        public int Id_vag { get { return id_vag; } set { id_vag = value; } }

        public int Num_vag { get { return num_vag; } set { num_vag = value; } }

        public string Rod
        {
            get { return rod; }
            set { rod = value; }
        }
       
        //Дата готовности отправки с УЗ
        public DateTime DT_uz { get { return dt_uz; } set { dt_uz = value; } } 
       
        public DateTime DT_amkr { get { return dt_amkr; } set { dt_amkr = value; } }

        public DateTime DT_on_way
        {
            get { return dt_on_way; }
            set { dt_on_way = value; }
        }

        public string Owner { get { return owner; } set { owner = value; } }

        public string Own_country { get { return own_country; } set { own_country = value; } }

        public int Id_godn { get { return id_godn; } set { id_godn = value; } }

        public string Godn { get { return godn; } set { godn = value; } }

        public Cond Cond
        {
            get { return cond; }
            set { cond = value; }
        }

        public int Id_gruz
        {
            get { return id_gruz; }
            set { id_gruz = value; }
        }

        public string Gruz { get { return gruz; } set { gruz = value; } }

        public int Id_gruz_amkr
        {
            get { return id_gruz_amkr; }
            set { id_gruz_amkr = value; }
        }


        public string Gruz_amkr
        {
            get { return gruz_amkr; }
            set { gruz_amkr = value; }
        }

        public double Weight_gruz { get { return weight_gruz; } set { weight_gruz = value; } }

        public int Id_ceh_gruz { get { return id_ceh_gruz; } set { id_ceh_gruz = value; } }

        public string CehGruz { get { return ceh_gruz; } set { ceh_gruz = value; } }

        public int Id_tupik
        {
            get { return id_tupik; }
            set { id_tupik = value; }
        }

        public string Tupik
        {
            get { return tupik; }
            set { tupik = value; }
        }

        public int Id_gdstait
        {
            get { return id_gdstait; }
            set { id_gdstait = value; }
        }


        public string Gdstait
        {
            get { return gdstait; }
            set { gdstait = value; }
        }

        public int Id_nazn_country
        {
            get { return id_nazn_country; }
            set { id_nazn_country = value; }
        }


        public string Nazn_country
        {
            get { return nazn_country; }
            set { nazn_country = value; }
        }


        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string Outer_station
        {
            get { return outer_station; }
            set { outer_station = value; }
        }

        public string NgruSAP
        {
            get { return ngruSAP; }
            set { ngruSAP = value; }
        }

        public string GrvuSAP
        {
            get { return grvuSAP; }
            set { grvuSAP = value; }
        }
    }
}
