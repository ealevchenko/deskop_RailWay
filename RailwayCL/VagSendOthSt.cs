using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class VagSendOthSt : VagOperations
    {
        private DateTime? dt_from_way = null;
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
            this.Id_oper = vo.Id_oper;
            this.Num_vag_on_way = vo.Num_vag_on_way;
            this.DT_amkr = vo.DT_amkr;
            this.Id_vag = vo.Id_vag;
            this.Num_vag = vo.Num_vag;
            this.Rod = vo.Rod;
            this.DT_on_way = vo.DT_on_way;
            this.Owner = vo.Owner;
            this.Own_country = vo.Own_country;
            this.Id_godn = vo.Id_godn;
            this.Godn = vo.Godn;
            this.Id_gruz = vo.Id_gruz;
            this.Gruz = vo.Gruz;
            this.Id_gruz_amkr = vo.Id_gruz_amkr;
            this.Gruz_amkr = vo.Gruz_amkr;
            this.Weight_gruz = vo.Weight_gruz;
            this.Id_ceh_gruz = vo.Id_ceh_gruz;
            this.CehGruz = vo.CehGruz;
            this.Id_tupik = vo.Id_tupik;
            this.Tupik = vo.Tupik;
            this.Id_gdstait = vo.Id_gdstait;
            this.Gdstait = vo.Gdstait;
            this.Id_nazn_country = vo.Id_nazn_country;
            this.Nazn_country = vo.Nazn_country;
            this.Note = vo.Note;
            this.Outer_station = vo.Outer_station;
            this.Cond = vo.Cond;
            this.GrvuSAP = vo.GrvuSAP;
            this.NgruSAP = vo.NgruSAP;
            this.MailDate = vo.MailDate;
            this.MailNum = vo.MailNum;
            this.MailText = vo.MailText;
            this.MailStat = vo.MailStat;
            this.MailSobstv = vo.MailSobstv;
        }

        public DateTime? DT_from_way
        {
            get { return dt_from_way; }
            set { dt_from_way = value; }
        }

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
            this.Id_oper = vo.Id_oper;
            this.Num_vag_on_way = vo.Num_vag_on_way;
            this.DT_amkr = vo.DT_amkr;
            this.Id_vag = vo.Id_vag;
            this.Num_vag = vo.Num_vag;
            this.Rod = vo.Rod;
            this.DT_on_way = vo.DT_on_way;
            this.Owner = vo.Owner;
            this.Own_country = vo.Own_country;
            this.Id_godn = vo.Id_godn;
            this.Godn = vo.Godn;
            this.Id_gruz = vo.Id_gruz;
            this.Gruz = vo.Gruz;
            this.Id_gruz_amkr = vo.Id_gruz_amkr;
            this.Gruz_amkr = vo.Gruz_amkr;
            this.Weight_gruz = vo.Weight_gruz;
            this.Id_ceh_gruz = vo.Id_ceh_gruz;
            this.CehGruz = vo.CehGruz;
            this.Id_tupik = vo.Id_tupik;
            this.Tupik = vo.Tupik;
            this.Id_gdstait = vo.Id_gdstait;
            this.Gdstait = vo.Gdstait;
            this.Id_nazn_country = vo.Id_nazn_country;
            this.Nazn_country = vo.Nazn_country;
            this.Note = vo.Note;
            this.Outer_station = vo.Outer_station;
            this.Cond = vo.Cond;
        }
    }
}
