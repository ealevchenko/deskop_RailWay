using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using RailwayCL;
using EFRailCars.Helpers;

namespace RailwayUI
{
    public partial class VagAcceptForm : Form, IVagAcceptDialogView
    {
        VagAcceptDialogPresenter vagAcceptDialogPresenter;

        SendingPoint sentFrom;
        Station station;

        public VagAcceptForm(Station station, SendingPoint sentFrom, bool sideActiv)
        {
            InitializeComponent();
            vagAcceptDialogPresenter = new VagAcceptDialogPresenter(this);

            this.sentFrom = sentFrom;
            this.station = station;
            cbSide.Enabled = sideActiv;

            vagAcceptDialogPresenter.loadInitValues();
        }

        Station IVagAcceptDialogView.getStation
        {
            get { return station; }
        }

        SendingPoint IVagAcceptDialogView.getSentFrom
        {
            get { return sentFrom; }
        }

        void IVagAcceptDialogView.loadWays(List<Way> list, string cbDisplay, string cbValue, string nonSelected)
        {
            cbWay.DataSource = list;
            cbWay.DisplayMember = cbDisplay;
            cbWay.ValueMember = cbValue;
            cbWay.SelectedIndex = -1;
            cbWay.Text = nonSelected;
        }

        void IVagAcceptDialogView.loadSides(object[] items, int selIdx, string nonSelected)
        {
            cbSide.Items.Clear();
            cbSide.Items.AddRange(items);
            cbSide.SelectedIndex = selIdx;
            if (selIdx == -1) cbSide.Text = nonSelected;
        }

        public Way getWay()
        {
            return (Way)cbWay.SelectedItem;
        }

        public Side getSide()
        {
            return (Side)cbSide.SelectedIndex;
        }

        public DateTime getDtArriv()
        {
            return DateTime.Parse(dtpDate.Value.ToShortDateString() + " " + dtpTime.Value.ToShortTimeString());
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (cbWay.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите путь!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop); 
                DialogResult = DialogResult.Abort;
            }
            if (cbSide.SelectedIndex == -1 && sentFrom.GetType().IsAssignableFrom(typeof(Station)))
            {
                MessageBox.Show("Выберите горловину!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DialogResult = DialogResult.Abort;
            }
        }

        private void VagAcceptForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == System.Windows.Forms.DialogResult.Abort) e.Cancel = true;
        }

        private void cbWay_DropDown(object sender, EventArgs e)
        {
            //Type propertyType;
            //PropertyInfo propertyInfo;

            //if (cbWay.SelectedItem != null && cbWay.DisplayMember.Contains("Num"))
            //{
            //    propertyType = cbWay.SelectedItem.GetType();
            //    propertyInfo = propertyType.GetProperty(cbWay.DisplayMember);

            //    cbWay.Text = propertyInfo.GetValue(cbWay.SelectedItem, null).ToString() + " - " + ((Way)cbWay.SelectedItem).Name;
            //}
        }
    }
}
