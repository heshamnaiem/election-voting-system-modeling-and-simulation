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
            //sample size
            int sample_size = 10;

            // number of Precinct
            const int p = 50;
            Voter.totalPrecNum = 50;
            
            // number of DRE
            const int d = 180;

            //turnout rate (not used now)
            double to = 0.72;

            //gamma scale parameter
            double gs = 0.583;

            double[] equity_array = new double[sample_size];

            for (int i = 0; i < sample_size; i++)
            {
                GIA GIA_Object = new GIA(p, d, to, gs);
                GIA_Object.Phase_I();
             //   double eq1 = GIA_Object.calculate_equity();
                GIA_Object.Phase_II();
                double eq2 = GIA_Object.calculate_equity();
                equity_array[i] = eq2;
            }

            //calculate mean
            double sum=0;
            for (int j = 0; j < sample_size; j++)
            {
                sum = sum + equity_array[j];
            }

            double mean_equity = sum / sample_size;

            //calculate standard deviation
            double temp;
            sum = 0;
            for (int j = 0; j < sample_size; j++)
            {
                temp=equity_array[j]-mean_equity;
                equity_array[j] = temp * temp;
                sum = sum + equity_array[j];

            }

            double std_equity = Math.Sqrt(sum / sample_size);
         
        }

        private void RandomAlgo_Click(object sender, EventArgs e)
        {
            //sample size
            int sample_size = 100;

            // number of Precinct
            const int p = 20;
            Voter.totalPrecNum = 20;

            // number of DRE
            const int d = 72;

            //turnout rate (not used now)
            double to = 0.56;

            //gamma scale parameter
            double gs = 0.583;

            double[] RA_equity_array = new double[sample_size];

            for (int i = 0; i < sample_size; i++)
            {
                RA RA_Obj = new RA(p, d, to, gs);
                RA_Obj.RandomAlgo();
                RA_equity_array[i] = RA_Obj.calculate_equity();       
                RA_Obj.Reset();
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\simulation\Random_Equity.txt", true))
                {
                    file.WriteLine(RA_equity_array[i].ToString());
                }
            }
        }
    }
}
