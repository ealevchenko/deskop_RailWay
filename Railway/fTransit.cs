using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RailwayCL;

namespace RailwayUI
{   
    /// <summary>
    /// Форма для выполнения функции "транзит"
    /// </summary>
    public partial class fTransit : Form, ITransitDialogView
    {
        TransitDialogPresenter transitDialogPresenter;

        Station station;

        public fTransit(Station station)
        {
            InitializeComponent();
            transitDialogPresenter = new TransitDialogPresenter(this);
            this.station = station;

            transitDialogPresenter.loadInitValues();
        }

        Station ITransitDialogView.getStation
        {
            get { return station; }
        }

        void ITransitDialogView.loadWays(List<Way> list, string cbDisplay, string cbValue, string nonSelected)
        {
            cbWay.DataSource = list;
            cbWay.DisplayMember = cbDisplay;
            cbWay.ValueMember = cbValue;
            cbWay.SelectedIndex = -1;
            cbWay.Text = nonSelected;
        }

        void ITransitDialogView.loadStations(List<Station> list, string cbDisplay, string cbValue, string nonSelected)
        {
            cbStat.DataSource = list;
            cbStat.DisplayMember = cbDisplay;
            cbStat.ValueMember = cbValue;
            cbStat.SelectedIndex = -1;
            cbStat.Text = nonSelected;
        }


        private void bOk_Click(object sender, EventArgs e)
        {
            if (cbWay.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите путь!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DialogResult = DialogResult.Abort;
            }
            if (cbStat.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите станцию!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DialogResult = DialogResult.Abort;
            }
        }

        private void fTransit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == System.Windows.Forms.DialogResult.Abort) e.Cancel = true;
        }

        public Way getWay()
        {
            return (Way)cbWay.SelectedItem;
        }

        public Station getStatTo()
        {
            return (Station)cbStat.SelectedItem;
        }

        public DateTime getDtArriv()
        {
            return DateTime.Parse(dtpDate.Value.ToShortDateString() + " " + dtpTime.Value.ToShortTimeString());
        }
    }
}
