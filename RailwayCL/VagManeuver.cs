﻿using EFRailCars.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class VagManeuver : VagOperations
    {
        //private DateTime dt_on_stat;
        //private DateTime? dt_from_way = null;
        private int lock_id_way = -1;
        private int lock_order = -1;
        private Side lock_side = Side.Empty;
        private int lock_id_locom = -1;


        public VagManeuver()
        {
        }

        public VagManeuver(VagOperations vo)
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

        //public DateTime DT_on_stat
        //{
        //    get { return dt_on_stat; }
        //    set { dt_on_stat = value; }
        //}

        //public DateTime? DT_from_way
        //{
        //    get { return dt_from_way; }
        //    set { dt_from_way = value; }
        //}

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
