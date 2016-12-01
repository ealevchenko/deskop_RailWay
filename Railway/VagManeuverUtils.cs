using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace RailwayUI
{
    class VagManeuverUtils : VagOperationsUtils
    {
        public override void makeVagDgvColumns(DataGridView dgv)
        {
            base.makeVagDgvColumns(dgv);

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Дата/время снятия с пути";
            col.DataPropertyName = "DT_from_way";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DisplayIndex = 2;
            col.Width = 130;
            dgv.Columns.Add(col);
        }
    }
}
