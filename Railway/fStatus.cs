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
    public partial class fStatus : Form
    {
        public fStatus()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pb1.Value < 100)
            {
                pb1.Value += 10;
            }
            else pb1.Value = 0;

        }
    }
}
