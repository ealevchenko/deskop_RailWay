using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.ComponentModel;

namespace RailwayUI
{
    class VagWaitAdmissUtils : VagOperationsUtils
    {
        public override void makeVagDgvColumns(DataGridView dgv)
        {
            base.makeVagDgvColumns(dgv);

            DataGridViewComboBoxColumn col;

            col = new DataGridViewComboBoxColumn();
            col.Name = "RospPlan";
            col.HeaderText = "План роспуска";
            //col.DataPropertyName = "";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DisplayIndex = 6;
            col.Visible = false;
            col.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            dgv.Columns.Add(col);

            col = new DataGridViewComboBoxColumn();
            col.Name = "RospFact";
            col.HeaderText = "Факт роспуска";
            //col.DataPropertyName = "";
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DisplayIndex = 7;
            col.Visible = false;
            col.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            dgv.Columns.Add(col);
        }

        //public void fillRospColumns(DataGridView dgv, List<Way> list)
        //{
        //    DataGridViewComboBoxColumn col;

        //    if (dgv.Columns["RospPlan"] is DataGridViewComboBoxColumn)
        //    {
        //        col = (DataGridViewComboBoxColumn)dgv.Columns["RospPlan"];
        //        col.DataSource = list;
        //        col.DisplayMember = WayUtils.GetInstance().CbDisplay;
        //        col.ValueMember = WayUtils.GetInstance().CbValue;
        //        col.AutoComplete = false;
        //    }
        //    if (dgv.Columns["RospFact"] is DataGridViewComboBoxColumn)
        //    {
        //        col = (DataGridViewComboBoxColumn)dgv.Columns["RospFact"];
        //        col.DataSource = list;
        //        col.DisplayMember = WayUtils.GetInstance().CbDisplay;
        //        col.ValueMember = WayUtils.GetInstance().CbValue;
        //    }
        //}
    }
}
