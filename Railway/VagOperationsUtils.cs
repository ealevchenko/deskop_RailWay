using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;

namespace RailwayUI
{
    public class VagOperationsUtils
    {
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
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Род вагона";
            col.DataPropertyName = "Rod";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.Width = 50;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Собственник";
            col.DataPropertyName = "Owner";
            //col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.Width = 150;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Страна собств.";
            col.DataPropertyName = "Own_country";
            //col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Годность по приб.";
            col.Name = "Годность";
            col.DataPropertyName = "Godn";
            dgv.Columns.Add(col);

            // -------------------------

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Состояние";
            col.Name = "Состояние";
            col.DataPropertyName = "Cond.Name";
            col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Род груза";
            col.Name = "Род груза";
            col.DataPropertyName = "Gruz";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Дата/время готовности отправки с УЗ";
            col.Name = "Дата/время готовности отправки с УЗ";
            col.DataPropertyName = "DT_uz";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            col.Width = 130;
            dgv.Columns.Add(col);
            // --- прибытие с УЗ -------

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Дата/время захода на АМКР";
            col.Name = "Дата/время захода на АМКР";
            col.DataPropertyName = "DT_amkr";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            col.Width = 130;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Станц. отправл. УЗ";
            col.Name = "Станц. отправл. УЗ";
            col.DataPropertyName = "Outer_station";
            col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Груз по прибыт. на АМКР";
            col.Name = "Груз по прибыт. на АМКР";
            col.DataPropertyName = "Gruz_amkr";
            col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Вес груза";
            col.Name = "Вес груза";
            col.DataPropertyName = "Weight_gruz";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Цех-получ. груза";
            col.Name = "Цех-получ. груза";
            col.DataPropertyName = "CehGruz";
            col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
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
        }

        public void changeColumnsPosition(DataGridView dgv, bool isDepart)
        {
            int i = dgv.Columns["Род груза"].DisplayIndex;

            if (isDepart)
            {
                dgv.Columns["Станция назнач."].DisplayIndex = i + 1;
                dgv.Columns["Примечан."].DisplayIndex = i + 2;
                dgv.Columns["Страна назнач."].DisplayIndex = i + 3;
                dgv.Columns["№ тупика"].DisplayIndex = i + 4;
                dgv.Columns["Грузоподъемность"].DisplayIndex = i + 5;
                dgv.Columns["Грузополучатель"].DisplayIndex = i + 6;
            }
            else
            {
                dgv.Columns["Дата/время захода на АМКР"].DisplayIndex = i + 2;
                dgv.Columns["Станц. отправл. УЗ"].DisplayIndex = i + 3;
                dgv.Columns["Груз по прибыт. на АМКР"].DisplayIndex = i + 4;
                dgv.Columns["Вес груза"].DisplayIndex = i + 5;
                dgv.Columns["Цех-получ. груза"].DisplayIndex = i + 6;
            }

            dgv.Columns["Состояние"].DisplayIndex = dgv.Columns["Дата/время захода на АМКР"].DisplayIndex - 1;
        }

    }
}
