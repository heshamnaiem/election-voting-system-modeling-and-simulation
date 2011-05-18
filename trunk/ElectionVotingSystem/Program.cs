using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using React;

namespace ElectionVotingSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // number of Precinct
            const int c = 4;

            Precinct[] prec = new Precinct[c];
            Task[] t = new Task[c];

            for (int i = 0; i < c; i++)
            {
                prec[i] = new Precinct(i+1);
                t[i] = new Process(prec[0], prec[0].Generator,prec[i].Number);
            }

            prec[0].Run(t);
        }
    }
}
