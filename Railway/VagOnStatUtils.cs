using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace RailwayUI
{
    class VagOnStatUtils : VagOperationsUtils
    {
        public override void makeVagDgvColumns(DataGridView dgv)
        {
            base.makeVagDgvColumns(dgv);

            //DataGridViewTextBoxColumn col;

            //col = new DataGridViewTextBoxColumn();
            //col.HeaderText = "Дата/время захода на АМКР";
            //col.DataPropertyName = "DT_amkr";
            //col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ////col.DisplayIndex = 6;
            //col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //dgv.Columns.Add(col);

            //col = new DataGridViewTextBoxColumn();
            //col.HeaderText = "Станц. отправл. УЗ";
            //col.DataPropertyName = "Outer_station";
            //col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //dgv.Columns.Add(col);

            //col = new DataGridViewTextBoxColumn();
            //col.HeaderText = "Груз по прибыт. на АМКР";
            //col.DataPropertyName = "Gruz";
            //col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //dgv.Columns.Add(col);

            //col = new DataGridViewTextBoxColumn();
            //col.HeaderText = "Вес груза";
            //col.DataPropertyName = "Weight_gruz";
            //col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //dgv.Columns.Add(col);

            //col = new DataGridViewTextBoxColumn();
            //col.HeaderText = "Цех-получ. груза";
            //col.DataPropertyName = "CehGruz";
            //col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //dgv.Columns.Add(col);
        }
    }
}
