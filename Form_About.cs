using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clicks_Per_Second
{
    public partial class Form_About : Form
    {
        public Form_About()
        {
            InitializeComponent();
            label2.Text = "Your CPS record: " + Properties.Settings.Default.CPS_Record + " CPS";
        }

        private void Form_About_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
