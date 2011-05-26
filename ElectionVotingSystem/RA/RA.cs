using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using React;

namespace ElectionVotingSystem
{
    class RA   // Best Random Algorithm
    {
        int Precinct_No;        //Number Of Precincts
        int DRE_No;        //Number Of DRE Machines

        Precinct[] prec;
        Task[] t;

        public RA(int precinct_no, int dre_no)
        {
            this.Precinct_No = precinct_no;
            this.DRE_No = dre_no;

            this.prec = new Precinct[precinct_no];
            this.t = new Task[precinct_no];
        }

        public void RandomAlgo()
        {
            Random rand = new Random();


            for (int i = 0; i < this.Precinct_No; i++)
            {
                this.prec[i] = new Precinct(i + 1);
                this.prec[i].Xi = rand.Next(1, this.DRE_No / 2);
                this.t[i] = new Process(prec[0], prec[0].Generator, prec[i].GetPrecinctNumber());
            }

            prec[0].Run(t);
        }
    
    }
}
