using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    /// <summary>
    /// Прибытие вагонов
    /// </summary>
    public class VagWaitAdmiss : VagOperations
    {
        private Way wayPlan = new Way();

        private Way wayFact = new Way();

      
        public VagWaitAdmiss()
        {
        }

        public VagWaitAdmiss(VagOperations vo)
        {
            this.Id_oper = vo.Id_oper;
            this.Num_vag_on_way = vo.Num_vag_on_way;
            this.DT_amkr = vo.DT_amkr;
            this.Id_vag = vo.Id_vag;
            this.Num_vag = vo.Num_vag;
            this.Rod = vo.Rod;
            this.DT_on_way = vo.DT_on_way;
            this.Dt_on_stat = vo.Dt_on_stat;
            this.Dt_from_stat = vo.Dt_from_stat;
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

        public Way WayPlan
        {
            get { return wayPlan; }
            set { wayPlan = value; }
        }

        public Way WayFact
        {
            get { return wayFact; }
            set { wayFact = value; }
        }
        
    }
}
