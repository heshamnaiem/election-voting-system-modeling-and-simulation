﻿using System;
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
        int it;
        int row;

        int[,] save;
        int[] Best;
        long[,] BestM_W_T_P;

        double turnOutRate;
        double gammaScale;

        Precinct[] prec;
        Task[] t;
       

        public RA(int precinct_no, int dre_no, double to_rate, double gScale)
        {
            this.Precinct_No = precinct_no;
            this.DRE_No = dre_no;
            this.it = 4;
            this.row = -1;

            this.save = new int[it, Precinct_No + 1];
            this.Best = new int[Precinct_No];
            this.BestM_W_T_P = new long[it, Precinct_No];

            this.turnOutRate = to_rate;
            this.gammaScale = gScale;

            this.prec = new Precinct[precinct_no];
            this.t = new Task[precinct_no];
        }

        public void RandomAlgo()
        {
            long temp = 2147483647; // the min MaxWaitingTime
            for (int i = 0; i < this.Precinct_No; i++)
            {
                this.prec[i] = new Precinct(i + 1, this.turnOutRate, this.gammaScale);
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
                    if (count < DRE_No)
                    {
                        if (i == Precinct_No - 1)
                        {
                            if (DRE_No - count > 0)
                            {
                                prec[i].Xi = DRE_No - count;
                            }
                        }
                        else
                            prec[i].Xi = rand.Next(1, this.DRE_No / 2);
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

        public double calculate_equity()
        {
            long sum = 0;
            //calculation of equity
            for (int i = 0; i < this.Precinct_No; i++)
            {
                for (int j = i + 1; j < this.Precinct_No; j++)
                {
                    sum = sum + Math.Abs(BestM_W_T_P[row, i] - BestM_W_T_P[row, j]);
                }
            }

            double numtodivide = (this.Precinct_No) * (this.Precinct_No - 1) / 2;

            double eq = sum / numtodivide;

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