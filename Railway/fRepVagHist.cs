using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RailwayUI
{
    public partial class fRepVagHist : Form
    {
        public int Num_Vag { get; set; }

        public fRepVagHist()
        {
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (tbNumVag.Text == "")
            {
                MessageBox.Show("Введите № вагона!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DialogResult = DialogResult.Abort;
            }
            else
            {
                try
                {
                    Num_Vag = Int32.Parse(tbNumVag.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Неверный формат № вагона!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    DialogResult = DialogResult.Abort;
                }
            }
        }

        private void fRepVagHist_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == System.Windows.Forms.DialogResult.Abort) e.Cancel = true;
        }
    }
}
