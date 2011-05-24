using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using React;

namespace ElectionVotingSystem
{
    class GIA
    {
        int Counter;
        int Precinct_No;        //Number Of Precincts
        int DRE_No;        //Number Of DRE Machines
        //  Precinct[] AllPrecincts;  //Array of all Precincts
        //ArrayList AllPrecincts;
        Precinct[] prec;
        Task[] t;
        int[] P_D_N;


        public GIA(int precinct_no, int dre_no)
        {
            // this.AllPrecincts = new Precinct[precinct_no];
            this.Counter = 0;
            this.Precinct_No = precinct_no;
            this.DRE_No = dre_no;

            this.prec = new Precinct[precinct_no];
            this.t = new Task[precinct_no];
            this.P_D_N = new int[precinct_no];

        }

        public void Phase_I()
        {
            

            //  Step 1. Assign initial values to xi for each precinct i (usually we set xi = 1 for all i). 
            //  Set Counter = Sum of xi

            for (int i = 0; i < this.Precinct_No; i++)
            {
              //  this.prec[i] = new Precinct(i + 1);
              //  this.t[i] = new Process(prec[0], prec[0].Generator, prec[i].GetPrecinctNumber());
                this.Counter++;
                this.P_D_N[i] = 1;

            }

            while (this.Counter < this.DRE_No)
            {
                //  Step 2. Let xi = xi+1 for the precinct i with the largest estimated expected waiting time in queue, Wi(xi).

                for (int i = 0; i < this.Precinct_No; i++)
                {
                    this.prec[i] = new Precinct(i + 1,P_D_N[i]);
                    this.t[i] = new Process(prec[0], prec[0].Generator, prec[i].GetPrecinctNumber());
                    
                }

                //Here run simulation and get largest estimated expected waiting time in queue 

                prec[0].Run(t);
               // Console.WriteLine("Max Waiting Time is {0} in Precinct number {1}  .", Voter.MaxWaitingTime, Voter.PrecinctNumber);


                int Large_PrecinctNo = Voter.PrecinctNumber-1;
               // prec[Large_PrecinctNo].AddDRE();
                P_D_N[Large_PrecinctNo]++;


                
                //Step 3. Counter = Counter + 1.
                this.Counter++;

                //Step 4. If Counter = N, stop. Otherwise, go to step 2.
            }

        }

        public void Phase_II()
        {

            //calculate Z(X) equity using allocation obtained in Phase I
            float Z_before=calculate_equity();

         step2:

            int precinct_no1 = get_smallest_precinct_wt();
            int precinct_no2 = get_neighbor_precinct(precinct_no1);

            prec[precinct_no1].RemoveDRE();
            prec[precinct_no2].AddDRE();
            
            //run simulation

            //calculate Z(X) equity using  modified allocation
            float Z_after = calculate_equity();

                      
            //Improve Z(X) 

            if (Z_after < Z_before)
            {
                Z_before = Z_after;
                goto step2;
            }



        }

        public float calculate_equity()
        {
            //calculation of equity
            float x = 3.5F;
            return x;
        }

        public int get_smallest_precinct_wt()
        {
            //caculation of smallest waiting time precinct no
            int i = 5;
            return i;

        }

        public int get_neighbor_precinct(int pno)
        {
            //caculation of  neigbhor smallest precinct no
            int i = 6;
            return i;

        }





    }
}
