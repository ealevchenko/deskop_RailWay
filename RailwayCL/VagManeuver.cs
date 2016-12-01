using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class VagManeuver : VagOperations
    {
        private DateTime dt_on_stat;
        private DateTime? dt_from_way = null;
        private int lock_id_way = -1;
        private int lock_order = -1;
        private Side lock_side = Side.Empty;
        private int lock_id_locom = -1;


        public VagManeuver()
        {
        }

        public VagManeuver(VagOperations vo)
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

        public DateTime DT_on_stat
        {
            get { return dt_on_stat; }
            set { dt_on_stat = value; }
        }

        public DateTime? DT_from_way
        {
            get { return dt_from_way; }
            set { dt_from_way = value; }
        }

        public int Lock_id_way
        {
            get { return lock_id_way; }
            set { lock_id_way = value; }
        }

        public int Lock_order
        {
            get { return lock_order; }
            set { lock_order = value; }
        }

        public Side Lock_side
        {
            get { return lock_side; }
            set { lock_side = value; }
        }

        public int Lock_id_locom
        {
            get { return lock_id_locom; }
            set { lock_id_locom = value; }
        }
    }
}
