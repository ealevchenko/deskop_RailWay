using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace RailwayUI
{
   
    public class VagOperationsUtils
    {
        private Color NotColor = Color.White;
        private Color Wagons = Color.FromArgb(255, 0xf5, 0xf5, 0xf5); //C9E1E8
        private Color Old = Color.LightGray;
        private Color MT = Color.FromArgb(255, 0xf5, 0xf5, 0xf5); //Color.PaleGreen; BAD9AE
        private Color SAP_IS = Color.FromArgb(255, 0xf5, 0xf5, 0xf5); //Color.LightYellow;        
        
        public virtual void makeVagDgvColumns(DataGridView dgv)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "№";
            col.DataPropertyName = "Num_vag_on_way";
            col.Width = 30;
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "№ вагона";
            col.DataPropertyName = "Num_vag";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = Wagons;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Род вагона";
            col.DataPropertyName = "Rod";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.Width = 50;
            col.DefaultCellStyle.BackColor = Wagons;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Собственник";
            col.DataPropertyName = "Owner";
            //col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.Width = 150;
            col.DefaultCellStyle.BackColor = Wagons;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Страна собств.";
            col.DataPropertyName = "Own_country";
            //col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "МТ (Страна)";
            col.Name = "МТ (Страна)";
            col.DataPropertyName = "wagon_country";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            col.DefaultCellStyle.BackColor = MT;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Годность по приб.";
            col.Name = "Годность";
            col.DataPropertyName = "godn";
            dgv.Columns.Add(col);

            // -------------------------

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Состояние";
            col.Name = "Состояние";
            col.DataPropertyName = "cond.Name";
            col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Род груза";
            col.Name = "Род груза";
            col.DataPropertyName = "Gruz";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "МТ (Груз)";
            col.Name = "МТ (Груз)";
            col.DataPropertyName = "CargoName";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            col.DefaultCellStyle.BackColor = MT;
            //col.Width = 130;
            dgv.Columns.Add(col);

            // --- прибытие с УЗ -------
            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "МТ (Дата/время готовности отправки с УЗ)";
            col.Name = "МТ (Дата/время готовности отправки с УЗ)";
            col.DataPropertyName = "DT_uz";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = MT;
            col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Дата/время захода на АМКР";
            col.Name = "Дата/время захода на АМКР";
            col.DataPropertyName = "DT_amkr";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = Old;
            col.Width = 130;
            dgv.Columns.Add(col);



            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Станц. отправл. УЗ";
            col.Name = "Станц. отправл. УЗ";
            col.DataPropertyName = "Outer_station";
            col.DefaultCellStyle.BackColor = Old;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Груз по прибыт. на АМКР";
            col.Name = "Груз по прибыт. на АМКР";
            col.DataPropertyName = "Gruz_amkr";
            col.DefaultCellStyle.BackColor = Old;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Вес груза";
            col.Name = "Вес груза";
            col.DataPropertyName = "Weight_gruz";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.DefaultCellStyle.BackColor = Old;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Цех-получ. груза";
            col.Name = "Цех-получ. груза";
            col.DataPropertyName = "CehGruz";
            col.DefaultCellStyle.BackColor = Old;
            dgv.Columns.Add(col);

            // --- инф. по письмам -----
            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Дата письма";
            col.Name = "Дата письма";
            col.DataPropertyName = "MailDate";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "№ письма";
            col.Name = "№ письма";
            col.DataPropertyName = "MailNum";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Текст письма";
            col.Name = "Текст письма";
            col.DataPropertyName = "MailText";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Станц. указ. в письме";
            col.Name = "Станц. указ. в письме";
            col.DataPropertyName = "MailStat";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Собств. указ. в письме";
            col.Name = "Собств. указ. в письме";
            col.DataPropertyName = "MailSobstv";
            dgv.Columns.Add(col);

            // --- отправка на УЗ ------

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Станция назнач.";
            col.Name = "Станция назнач.";
            col.DataPropertyName = "Gdstait";
            col.Width = 200;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Примечан.";
            col.Name = "Примечан.";
            col.DataPropertyName = "Note";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Страна назнач.";
            col.Name = "Страна назнач.";
            col.DataPropertyName = "Nazn_country";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "№ тупика";
            col.Name = "№ тупика";
            col.DataPropertyName = "Tupik";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Грузоподъемность";
            col.Name = "Грузоподъемность";
            col.DataPropertyName = "GrvuSAP";
            col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Грузополучатель";
            col.Name = "Грузополучатель";
            col.DataPropertyName = "NgruSAP";
            col.Width = 130;
            dgv.Columns.Add(col);

            // --------------------------

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Дата/время постановки на путь";
            col.DataPropertyName = "DT_on_way";
            //col.DisplayIndex = dgv.Columns.Count - 1;
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.Width = 130;
            dgv.Columns.Add(col);

            // САП
            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (№ накладной)";
            col.Name = "САП (№ накладной)";
            col.DataPropertyName = "NumNakl";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "МТ (вес) -> САП (вес по документам)";
            col.Name = "МТ (вес) -> САП (вес по документам)";
            col.DataPropertyName = "WeightDoc";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (номер отвесной)";
            col.Name = "САП (номер отвесной)";
            col.DataPropertyName = "DocNumReweighing";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (дата отвесной)";
            col.Name = "САП (дата отвесной)";
            col.DataPropertyName = "DocDataReweighing";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (вес после перевески)";
            col.Name = "САП (вес после перевески)";
            col.DataPropertyName = "WeightReweighing";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (время перевески)";
            col.Name = "САП (время перевески)";
            col.DataPropertyName = "DateTimeReweighing";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (код материала)";
            col.Name = "САП (код материала)";
            col.DataPropertyName = "CodeMaterial";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (Материал)";
            col.Name = "САП (Материал)";
            col.DataPropertyName = "NameMaterial";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (Код станции отправл. УЗ)";
            col.Name = "САП (Код станции отправл. УЗ)";
            col.DataPropertyName = "CodeStationShipment";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (Название станции отправл. УЗ)";
            col.Name = "САП (Название станции отправл. УЗ)";
            col.DataPropertyName = "NameStationShipment";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (Код цеха)";
            col.Name = "САП (Код цеха)";
            col.DataPropertyName = "CodeShop";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (Цех получатель груза)";
            col.Name = "САП (Цех получатель груза)";
            col.DataPropertyName = "NameShop";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (Код переадр. цеха)";
            col.Name = "САП (Код переадр. цеха)";
            col.DataPropertyName = "CodeNewShop";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (Цех переадр. груза)";
            col.Name = "САП (Цех переадр. груза)";
            col.DataPropertyName = "NameNewShop";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (Разрешение на выгрузку)";
            col.Name = "САП (Разрешение на выгрузку)";
            col.DataPropertyName = "PermissionUnload";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (Перенос САП)";
            col.Name = "САП (Перенос САП)";
            col.DataPropertyName = "Step1";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "САП (Перенос 10 км)";
            col.Name = "САП (Перенос 10 км)";
            col.DataPropertyName = "Step2";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = SAP_IS;
            //col.Width = 130;
            dgv.Columns.Add(col);
        }

        public void changeColumnsPosition(DataGridView dgv, bool isDepart)
        {
            int i = dgv.Columns["МТ (Груз)"].DisplayIndex;

            if (isDepart)
            {
                dgv.Columns["Станция назнач."].DisplayIndex = i + 2;
                dgv.Columns["Примечан."].DisplayIndex = i + 3;
                dgv.Columns["Страна назнач."].DisplayIndex = i + 4;
                dgv.Columns["№ тупика"].DisplayIndex = i + 5;
                dgv.Columns["Грузоподъемность"].DisplayIndex = i + 6;
                dgv.Columns["Грузополучатель"].DisplayIndex = i + 7;
            }
            else
            {
                dgv.Columns["МТ (Дата/время готовности отправки с УЗ)"].DisplayIndex = i + 2;                
                dgv.Columns["Дата/время захода на АМКР"].DisplayIndex = i + 3;
                dgv.Columns["Станц. отправл. УЗ"].DisplayIndex = i + 4;
                dgv.Columns["Груз по прибыт. на АМКР"].DisplayIndex = i + 5;
                dgv.Columns["Вес груза"].DisplayIndex = i + 6;
                dgv.Columns["Цех-получ. груза"].DisplayIndex = i + 7;
                //dgv.Columns["САП (№ накладной)"].DisplayIndex = i + 8;
                //dgv.Columns["МТ (вес) -> САП (вес по документам)"].DisplayIndex = i + 9;
                //dgv.Columns["САП (номер отвесной)"].DisplayIndex = i + 10;
                //dgv.Columns["САП (дата отвесной)"].DisplayIndex = i + 11;
                //dgv.Columns["САП (вес после перевески)"].DisplayIndex = i + 12;
                //dgv.Columns["САП (время перевески)"].DisplayIndex = i + 13;
                //dgv.Columns["САП (код материала)"].DisplayIndex = i + 14;
                //dgv.Columns["САП (Материал)"].DisplayIndex = i + 15;
                //dgv.Columns["САП (Код станции отправл. УЗ)"].DisplayIndex = i + 16;
                //dgv.Columns["САП (Название станции отправл. УЗ)"].DisplayIndex = i + 17;
                //dgv.Columns["САП (Код цеха)"].DisplayIndex = i + 18;
                //dgv.Columns["САП (Цех получатель груза)"].DisplayIndex = i + 19;
                //dgv.Columns["САП (Код переадр. цеха)"].DisplayIndex = i + 20;
                //dgv.Columns["САП (Цех переадр. груза)"].DisplayIndex = i + 21;
                //dgv.Columns["САП (Разрешение на выгрузку)"].DisplayIndex = i + 22;
                //dgv.Columns["САП (Перенос САП)"].DisplayIndex = i + 22;
                //dgv.Columns["САП (Перенос 10 км)"].DisplayIndex = i + 22;
            }

            dgv.Columns["Состояние"].DisplayIndex = dgv.Columns["МТ (Дата/время готовности отправки с УЗ)"].DisplayIndex - 1;
        }

    }
}
