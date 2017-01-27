using EFRailCars.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class VagSendOthSt : VagOperations
    {
        //private DateTime? dt_from_way = null;
        private int st_lock_id_stat = -1;
        private int st_lock_order = -1;
        private int st_lock_train = 0;
        private Side st_lock_side = Side.Empty;
        private int st_lock_locom1 = -1;
        private int st_lock_locom2 = -1;    
        private int st_gruz_front = -1;
        private int st_shop = -1;

        public VagSendOthSt()
        {
        }

        public VagSendOthSt(VagOperations vo)
        {
            base.Create(vo);
            //this.id_oper = vo.id_oper;
            //this.num_vag_on_way = vo.num_vag_on_way;
            //this.dt_amkr = vo.dt_amkr;
            //this.id_vag = vo.id_vag;
            //this.num_vag = vo.num_vag;
            //this.rod = vo.rod;
            //this.dt_on_way = vo.dt_on_way;
            //this.owner = vo.owner;
            //this.own_country = vo.own_country;
            //this.id_godn = vo.id_godn;
            //this.godn = vo.godn;
            //this.id_gruz = vo.id_gruz;
            //this.gruz = vo.gruz;
            //this.id_gruz_amkr = vo.id_gruz_amkr;
            //this.gruz_amkr = vo.gruz_amkr;
            //this.weight_gruz = vo.weight_gruz;
            //this.id_ceh_gruz = vo.id_ceh_gruz;
            //this.ceh_gruz = vo.ceh_gruz;
            //this.id_tupik = vo.id_tupik;
            //this.tupik = vo.tupik;
            //this.id_gdstait = vo.id_gdstait;
            //this.gdstait = vo.gdstait;
            //this.id_nazn_country = vo.id_nazn_country;
            //this.nazn_country = vo.nazn_country;
            //this.note = vo.note;
            //this.outer_station = vo.outer_station;
            //this.cond = vo.cond;
            //this.grvuSAP = vo.grvuSAP;
            //this.ngruSAP = vo.ngruSAP;
            //this.MailDate = vo.MailDate;
            //this.MailNum = vo.MailNum;
            //this.MailText = vo.MailText;
            //this.MailStat = vo.MailStat;
            //this.MailSobstv = vo.MailSobstv;
        }

        //public DateTime? DT_from_way
        //{
        //    get { return dt_from_way; }
        //    set { dt_from_way = value; }
        //}

        public int St_lock_id_stat
        {
            get { return st_lock_id_stat; }
            set { st_lock_id_stat = value; }
        }
       
        public int St_lock_order
        {
            get { return st_lock_order; }
            set { st_lock_order = value; }
        }

        public int St_lock_train
        {
            get { return st_lock_train; }
            set { st_lock_train = value; }
        }

        public Side St_lock_side
        {
            get { return st_lock_side; }
            set { st_lock_side = value; }
        }

        public int St_lock_locom1
        {
            get { return st_lock_locom1; }
            set { st_lock_locom1 = value; }
        }

        public int St_lock_locom2
        {
            get { return st_lock_locom2; }
            set { st_lock_locom2 = value; }
        }

        public int St_gruz_front 
        {
            get { return st_gruz_front; }
            set { st_gruz_front = value; } 
        }

        public int St_shop 
        {
            get { return st_shop; }
            set { st_shop = value; } 
        }

        public VagSendOthSt(VagWaitAdmiss vo) // for Transit
        {
            base.Create(vo);
            //this.id_oper = vo.id_oper;
            //this.num_vag_on_way = vo.num_vag_on_way;
            //this.dt_amkr = vo.dt_amkr;
            //this.id_vag = vo.id_vag;
            //this.num_vag = vo.num_vag;
            //this.rod = vo.rod;
            //this.dt_on_way = vo.dt_on_way;
            //this.owner = vo.owner;
            //this.own_country = vo.own_country;
            //this.id_godn = vo.id_godn;
            //this.godn = vo.godn;
            //this.id_gruz = vo.id_gruz;
            //this.gruz = vo.gruz;
            //this.id_gruz_amkr = vo.id_gruz_amkr;
            //this.gruz_amkr = vo.gruz_amkr;
            //this.weight_gruz = vo.weight_gruz;
            //this.id_ceh_gruz = vo.id_ceh_gruz;
            //this.ceh_gruz = vo.ceh_gruz;
            //this.id_tupik = vo.id_tupik;
            //this.tupik = vo.tupik;
            //this.id_gdstait = vo.id_gdstait;
            //this.gdstait = vo.gdstait;
            //this.id_nazn_country = vo.id_nazn_country;
            //this.nazn_country = vo.nazn_country;
            //this.note = vo.note;
            //this.outer_station = vo.outer_station;
            //this.cond = vo.cond;
        }
    }
}
