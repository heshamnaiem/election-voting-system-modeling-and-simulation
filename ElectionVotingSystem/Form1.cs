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
            const int p = 20;
            Voter.totalPrecNum = 20;
            // number of DRE
            const int d = 30;
            //turnout rate
            double to = 0.92;
            //gamma scale parameter
            double gs = 1.05;

            GIA GIA_Object = new GIA(p, d,to,gs);
            GIA_Object.Phase_I();
            double eq1=GIA_Object.calculate_equity();
            GIA_Object.Phase_II();
            double eq2 = GIA_Object.calculate_equity();
         //   GIA_Object.Phase_II();
        }
    }
}
