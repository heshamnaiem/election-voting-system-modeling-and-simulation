using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using React;

namespace ElectionVotingSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // number of Precinct
            const int p = 3;
            // number of DRE
            const int d = 40;

            GIA GIA_Object = new GIA(p, d);
            GIA_Object.Phase_I();
            GIA_Object.Phase_II();
        }
    }
}
