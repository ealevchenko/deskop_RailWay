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
    public partial class fRepPeriod : Form
    {
        public DateTime Bd { get; set; }
        public DateTime Ed { get; set; }

        public fRepPeriod()
        {
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            Bd = dtpBeg.Value;
            Ed = dtpEnd.Value;
        }
    }
}
