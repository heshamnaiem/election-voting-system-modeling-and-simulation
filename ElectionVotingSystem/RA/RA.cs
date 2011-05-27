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

        int[,] save;
        public int[] Best;

        Precinct[] prec;
        Task[] t;

        public RA(int precinct_no, int dre_no)
        {
            this.Precinct_No = precinct_no;
            this.DRE_No = dre_no;

            int[,] save = new int[3, Precinct_No + 1];
            int[] Best = new int[Precinct_No];

            this.prec = new Precinct[precinct_no];
            this.t = new Task[precinct_no];
        }

        public void RandomAlgo()
        {
            long temp = 2147483647;

            for (int j = 0; j < 3; j++)
            {
                Random rand = new Random();
                Voter.MaxWaitingTime = -1;
                Voter.PrecinctNumber = -1;
                Voter.ResetM_W_T_P();

                for (int i = 0; i < this.Precinct_No; i++)
                {
                    this.prec[i] = new Precinct(i + 1);
                    this.prec[i].Xi = rand.Next(1, this.DRE_No / 2);
                    save[j, prec[i].GetPrecinctNumber()] = prec[i].Xi;
                    this.t[i] = new Process(prec[0], prec[0].Generator, prec[i].GetPrecinctNumber());
                }

                prec[0].Run(t);

                save[j, 0] = (int)Voter.MaxWaitingTime;
            }

            for (int j = 0; j < 3; j++)
            {
                if (temp > save[j, 0])
                {
                    temp = save[j, 0];
                }
            }
            for (int j = 0; j < 3; j++)
            {
                if (temp == save[j, 0])
                {
                    for (int i = 0; i < Precinct_No; i++)
                    {
                        Best[i] = save[j, i + 1];
                    }
                }
            }

        }

    }
}
