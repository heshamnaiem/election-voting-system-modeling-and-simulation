using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using React;
using React.Distribution;

namespace ElectionVotingSystem
{
    class GIA
    {
        int Counter;
        int Precinct_No;        //Number Of Precincts
        int DRE_No;        //Number Of DRE Machines        
        Precinct[] prec;
        Task[] t;
        double turnOutRate;
        double gammaScale;
        double equity;

        Weibull weibull;



        public GIA(int precinct_no, int dre_no, double to_rate, double gScale)
        {
            this.Counter = 0;
            this.Precinct_No = precinct_no;
            this.DRE_No = dre_no;

            this.turnOutRate = to_rate;
            this.gammaScale = gScale;

            this.prec = new Precinct[precinct_no];
            this.t = new Task[precinct_no];

            weibull = new Weibull(60.884, 6.9514);

        }

        public void Phase_I()
        {
            

            //  Step 1. Assign initial values to xi for each precinct i (usually we set xi = 1 for all i). 
            //  Set Counter = Sum of xi

            for (int i = 0; i < this.Precinct_No; i++)
            {
                long to_fit = (long)(weibull.NextDouble());
                this.prec[i] = new Precinct(i + 1, to_fit, this.gammaScale);
                this.t[i] = new Process(prec[0], prec[0].Generator, prec[i].GetPrecinctNumber());
                this.Counter++;
            }

            while (this.Counter < this.DRE_No)
            {
                Voter.ResetM_W_T_P();
                Voter.MaxWaitingTime = -1;
                Voter.PrecinctNumber = -1;
                //  Step 2. Let xi = xi+1 for the precinct i with the largest estimated expected waiting time in queue, Wi(xi).

                for (int i = 0; i < this.Precinct_No; i++)
                {
                    this.t[i] = new Process(prec[0], prec[0].Generator, prec[i].GetPrecinctNumber());
                }

                //Here run simulation and get largest estimated expected waiting time in queue 

                prec[0].Run(t);
               // Console.WriteLine("Max Waiting Time is {0} in Precinct number {1}  .", Voter.MaxWaitingTime, Voter.PrecinctNumber);


                int Large_PrecinctNo = Voter.PrecinctNumber-1;
                prec[Large_PrecinctNo].AddDRE();

                
                //Step 3. Counter = Counter + 1.
                this.Counter++;

                //Step 4. If Counter = N, stop. Otherwise, go to step 2.
            }

            for (int i = 0; i < this.Precinct_No; i++)
            {
                this.t[i] = new Process(prec[0], prec[0].Generator, prec[i].GetPrecinctNumber());
            }
            prec[0].Run(t);

         //   this.equity = calculate_equity();


        }

        public void Phase_II()
        {
            bool definecase;
            for (int i = 0; i < (this.Precinct_No-1); i++)
            {
                    //calculate Z(X) before
                    double Z_before = this.calculate_equity();

                    if (Voter.M_W_T_P[i] < Voter.M_W_T_P[i+1])
                    {
                        prec[i+1].AddDRE();
                        prec[i].RemoveDRE();
                        definecase = true;
                    }
                    else
                    {
                        prec[i].AddDRE();
                        prec[i+1].RemoveDRE();
                        definecase = false;
                    }

                    Voter.ResetM_W_T_P();
                    Voter.MaxWaitingTime = -1;
                    Voter.PrecinctNumber = -1;
                    //  Step 2. Let xi = xi+1 for the precinct i with the largest estimated expected waiting time in queue, Wi(xi).

                    for (int k = 0; k < this.Precinct_No; k++)
                    {
                        this.t[k] = new Process(prec[0], prec[0].Generator, prec[k].GetPrecinctNumber());
                    }

                    //Here run simulation and get largest estimated expected waiting time in queue 

                    prec[0].Run(t);

                    //calculate Z(X) after
                    double Z_after = this.calculate_equity();

                    //Improve Z(X) 

                    if (Z_after > Z_before)
                    {
                        //return to previous
                        if (definecase)
                        {
                            prec[i+1].RemoveDRE();
                            prec[i].AddDRE();

                        }
                        else
                        {
                            prec[i].RemoveDRE();
                            prec[i+1].AddDRE();

                        }

                        //run simulation again

                        Voter.ResetM_W_T_P();
                        Voter.MaxWaitingTime = -1;
                        Voter.PrecinctNumber = -1;
                        //  Step 2. Let xi = xi+1 for the precinct i with the largest estimated expected waiting time in queue, Wi(xi).

                        for (int k = 0; k < this.Precinct_No; k++)
                        {
                            this.t[k] = new Process(prec[0], prec[0].Generator, prec[k].GetPrecinctNumber());
                        }

                        //Here run simulation and get largest estimated expected waiting time in queue 

                        prec[0].Run(t);


                    }                 
                                       
               

            }

            



        }

        public double calculate_equity()
        {
            long sum=0;
            //calculation of equity
            for (int i = 0; i < this.Precinct_No; i++)
            {
                for (int j = i+1; j < this.Precinct_No; j++)
                {
                    sum = sum + Math.Abs(Voter.M_W_T_P[i]- Voter.M_W_T_P[j]);
                }

            }

            double numtodivide = (this.Precinct_No) * (this.Precinct_No - 1) / 2;

            double eq = sum / numtodivide;

            eq = eq / 60;

            return eq;
        }     





    }
}
