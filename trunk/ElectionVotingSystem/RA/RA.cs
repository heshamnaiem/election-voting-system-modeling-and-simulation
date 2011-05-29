using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using React;
using React.Distribution;

namespace ElectionVotingSystem
{
    class RA   // Best Random Algorithm
    {
        int Precinct_No;        //Number Of Precincts
        int DRE_No;        //Number Of DRE Machines
        int it;
        int row;
        bool flag;

        int[,] save;    // to save all the combinations
        int[] Best;     // contain the Best combination
        long[,] BestM_W_T_P;    //contain the Best M_W_T_P [iteration, precincts] // row iteration is the Best

        double turnOutRate;
        double gammaScale;

        Precinct[] prec;
        Task[] t;

        Weibull weibull;

        public RA(int precinct_no, int dre_no, double to_rate, double gScale)
        {
            this.Precinct_No = precinct_no;
            this.DRE_No = dre_no;
            this.it = 4;
            this.row = -1;
            this.flag = false;

            this.save = new int[it, Precinct_No + 1];
            this.Best = new int[Precinct_No];
            this.BestM_W_T_P = new long[it, Precinct_No];

            this.turnOutRate = to_rate;
            this.gammaScale = gScale;

            this.prec = new Precinct[precinct_no];
            this.t = new Task[precinct_no];


            weibull = new Weibull(60.884, 6.9514);
        }

        public void RandomAlgo()
        {
            for (int i = 0; i < this.Precinct_No; i++)
            {
                long to_fit = ((long)(weibull.NextDouble()));
                double to_fit_rate = (double)(to_fit) / 100;
                this.prec[i] = new Precinct(i + 1, to_fit_rate, gammaScale);
            }

            for (int j = 0; j < it; j++)
            {
                Random rand = new Random();
                Voter.MaxWaitingTime = -1;
                Voter.PrecinctNumber = -1;
                Voter.ResetM_W_T_P();
                int count = 0;

    
                for (int i = 0; i < this.Precinct_No; i++)
                {
                    if (count < DRE_No - Precinct_No/2)
                    {
                        if (i == Precinct_No - 1)
                        {
                            if (DRE_No - count > 0)
                            {
                                prec[i].Xi = DRE_No - count;
                            }
                        }
                        else
                        {
                            int test = prec[i].Xi;
                            prec[i].Xi = rand.Next(1, (DRE_No - Precinct_No) / 4);
                            if ((prec[i].Xi + count) > (DRE_No - Precinct_No/2))
                                prec[i].Xi = test;
                        }
                    }
                    save[j, prec[i].GetPrecinctNumber()] = prec[i].Xi;
                    count += prec[i].Xi;
                    this.t[i] = new Process(prec[0], prec[0].Generator, prec[i].GetPrecinctNumber());
                }

                prec[0].Run(t);

                save[j, 0] = (int)Voter.MaxWaitingTime;
                for (int i = 0; i < Precinct_No; i++)
                {
                    BestM_W_T_P[j, i] = Voter.M_W_T_P[i];
                }
            }

            Calculations();
        }

        public void Calculations()
        {
            long temp = 2147483647; // the min MaxWaitingTime

            for (int j = 0; j < it; j++)
            {
                if (temp > save[j, 0])
                {
                    temp = save[j, 0];  // temp contain the min value for the max waiting Time 
                    row = j;                //the number of row that contain the Best combination
                }
            }
            for (int i = 0; i < Precinct_No; i++)
            {
                Best[i] = save[row, i + 1];
            }
        }

        public void RA_Phase_II()
        {
 
            double eq_before = 0;
            double eq_after = 0;
            int minIndex = 0;
            int maxIndex = 0;

            // set the prec.x with the Best combination
            for (int i = 1; i < Precinct_No; i++)
            {
                prec[i].Xi = Best[i];
                Voter.M_W_T_P[i] = BestM_W_T_P[row, i];
            }

            do
            {
                // Get the min and max of the Best MWTP 
                long min = Voter.M_W_T_P[0];
                minIndex = 0;
                long max = Voter.M_W_T_P[0];
                maxIndex = 0;

                for (int i = 1; i < Precinct_No; i++)
                {
                    if (min > Voter.M_W_T_P[i])
                    {
                        min = Voter.M_W_T_P[i];
                        minIndex = i;
                    }
                    if (max < Voter.M_W_T_P[i])
                    {
                        max = Voter.M_W_T_P[i];
                        maxIndex = i;
                    }
                }

                // calculate equity using M_W_T_P
                flag = true;
                eq_before = calculate_equity();

                prec[minIndex].RemoveDRE();
                prec[maxIndex].AddDRE();

                Voter.ResetM_W_T_P();
                Voter.MaxWaitingTime = -1;
                Voter.PrecinctNumber = -1;

                for (int k = 0; k < this.Precinct_No; k++)
                {
                    this.t[k] = new Process(prec[0], prec[0].Generator, prec[k].GetPrecinctNumber());
                }

                prec[0].Run(t);
                // calculate equity using M_W_T_P
                flag = true;
                eq_after = calculate_equity();

            } while (eq_after < eq_before);

            prec[minIndex].AddDRE();
            prec[maxIndex].RemoveDRE();

            Voter.ResetM_W_T_P();
            Voter.MaxWaitingTime = -1;
            Voter.PrecinctNumber = -1;

            for (int k = 0; k < this.Precinct_No; k++)
            {
                this.t[k] = new Process(prec[0], prec[0].Generator, prec[k].GetPrecinctNumber());
            }

            prec[0].Run(t);
            // calculate equity using M_W_T_P
            flag = true;
            eq_after = calculate_equity();


            save[row, 0] = (int)Voter.MaxWaitingTime;
            for (int i = 0; i < Precinct_No; i++)
            {
                BestM_W_T_P[row, i] = Voter.M_W_T_P[i];
                Best[i] = prec[i].Xi;
                save[row, i] = prec[i].Xi;
            }

            flag = false;
            eq_after = calculate_equity();
        }




        public double calculate_equity()
        {
            long sum = 0;
            //calculation of equity
            for (int i = 0; i < this.Precinct_No; i++)
            {
                for (int j = i + 1; j < this.Precinct_No; j++)
                {
                    switch (flag)
                    {
                        case (false):
                            sum = sum + Math.Abs(BestM_W_T_P[row, i] - BestM_W_T_P[row, j]);
                            break;
                        case (true):
                            sum = sum + Math.Abs(Voter.M_W_T_P[i] - Voter.M_W_T_P[j]);
                            break;
                    }
                }
            }

            double numtodivide = (this.Precinct_No) * (this.Precinct_No - 1) / 2;

            double eq = sum / numtodivide;

            eq = eq / 60;

            return eq;
        }

        public void Reset()
        {
            for (int i = 0; i < it; i++)
            {
                for (int j = 0; j < Precinct_No; j++)
                {
                    save[i, j] = -1;
                    Best[j] = -1;
                    BestM_W_T_P[i, j] = -1;
                }
            }
        }


    }
}
