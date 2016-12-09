using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayCL
{
    public class VagWaitRemoveAdmiss : VagOperations
    {
        public VagWaitRemoveAdmiss()
        {
        }

        public VagWaitRemoveAdmiss(VagOperations vo)
        {
            base.Create(vo);
        //this.id_oper = vo.id_oper ;//
        //this.dt_uz = vo.dt_uz ;//
        //this.dt_amkr = vo.dt_amkr ;//
        //this.dt_out_amkr = vo.dt_out_amkr ;//
        //this.id_sostav = vo.id_sostav ;//
        //this.id_vag = vo.id_vag ;//TODO: Убрать переход на новый справочник Wagons
        //this.num_vag = vo.num_vag ;//

        //this.dt_on_stat = vo.dt_on_stat ;//
        //this.dt_from_stat = vo.dt_from_stat ;//

        //this.dt_on_way = vo.dt_on_way ;//
        //this.dt_from_way = vo.dt_from_way ;//
        //this.num_vag_on_way = vo.num_vag_on_way ;//

        //this.id_godn = vo.id_godn ;//
        //this.godn = vo.godn ;//
        //this.cond = vo.cond ;//

        //this.grvuSAP = vo.grvuSAP ;//
        //this.ngruSAP = vo.ngruSAP ;//

        //// Справочник Wagons
        //this.rod = vo.rod ;//     
        //this.owner = vo.owner ;//
        //this.own_country = vo.own_country ;//

        ////Cправочник САП вх. поставки
        //this.id_gruz = vo.id_gruz ;//
        //this.gruz = vo.gruz ;//
        //this.weight_gruz = vo.weight_gruz ;//
        //this.id_ceh_gruz = vo.id_ceh_gruz ;//
        //this.ceh_gruz = vo.ceh_gruz ;//
        //this.outer_station = vo.outer_station ;//


        ////Cправочник САП выход. поставки //TODO: Переделать переход на новый справочник САП выход. поставки
        //this.id_gruz_amkr = vo.id_gruz_amkr ;//
        //this.gruz_amkr = vo.gruz_amkr ;//        

        //this.id_tupik = vo.id_tupik ;//
        //this.tupik = vo.tupik ;//
        //this.id_gdstait = vo.id_gdstait ;//
        //this.gdstait = vo.gdstait ;//
        //this.id_nazn_country = vo.id_nazn_country ;//
        //this.nazn_country = vo.nazn_country ;//
        //this.note = vo.note ;//

        //// Справочник писем
        //this.MailDate = vo.MailDate ;//
        //this.MailNum = vo.MailNum ;//
        //this.MailText = vo.MailText ;//
        //this.MailStat = vo.MailStat ;//
        //this.MailSobstv = vo.MailSobstv ;//
            
            
            
            
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

        //public VagWaitRemoveAdmiss(VagOperations vo)
        //{
        //    this.id_oper = vo.id_oper;
        //    this.num_vag_on_way = vo.num_vag_on_way;
        //    this.dt_amkr = vo.dt_amkr;
        //    this.id_vag = vo.id_vag;
        //    this.num_vag = vo.num_vag;
        //    this.rod = vo.rod;
        //    this.dt_on_way = vo.dt_on_way;
        //    this.owner = vo.owner;
        //    this.own_country = vo.own_country;
        //    this.id_godn = vo.id_godn;
        //    this.godn = vo.godn;
        //    this.id_gruz = vo.id_gruz;
        //    this.gruz = vo.gruz;
        //    this.id_gruz_amkr = vo.id_gruz_amkr;
        //    this.gruz_amkr = vo.gruz_amkr;
        //    this.weight_gruz = vo.weight_gruz;
        //    this.id_ceh_gruz = vo.id_ceh_gruz;
        //    this.ceh_gruz = vo.ceh_gruz;
        //    this.id_tupik = vo.id_tupik;
        //    this.tupik = vo.tupik;
        //    this.id_gdstait = vo.id_gdstait;
        //    this.gdstait = vo.gdstait;
        //    this.id_nazn_country = vo.id_nazn_country;
        //    this.nazn_country = vo.nazn_country;
        //    this.note = vo.note;
        //    this.outer_station = vo.outer_station;
        //    this.cond = vo.cond;
        //    this.grvuSAP = vo.grvuSAP;
        //    this.ngruSAP = vo.ngruSAP;
        //    this.MailDate = vo.MailDate;
        //    this.MailNum = vo.MailNum;
        //    this.MailText = vo.MailText;
        //    this.MailStat = vo.MailStat;
        //    this.MailSobstv = vo.MailSobstv;
        //}
    }
}
