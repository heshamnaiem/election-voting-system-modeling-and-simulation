using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using React;

namespace ElectionVotingSystem.GIA
{
    class GIA
    {
        int Counter;
        int Precinct_No;        //Number Of Precincts
        int DRE_No;        //Number Of DRE Machines
        //  Precinct[] AllPrecincts;  //Array of all Precincts
        ArrayList AllPrecincts;

        public GIA(int precinct_no, int dre_no)
        {
            // this.AllPrecincts = new Precinct[precinct_no];
            this.Counter = 0;
            this.Precinct_No = precinct_no;
            this.DRE_No = dre_no;
        }

        public void Phase_I()
        {
            //  Step 1. Assign initial values to xi for each precinct i (usually we set xi = 1 for all i). 
            //  Set Counter = Sum of xi

            for (int i = 1; i <= this.Precinct_No; i++)
            {
                AllPrecincts.Add(new Precinct(i));
                this.Counter++;
            }

            while (this.Counter < this.DRE_No)
            {
                //  Step 2. Let xi = xi+1 for the precinct i with the largest estimated expected waiting time in queue, Wi(xi).

                //Here run simulation and get largest estimated expected waiting time in queue  

                int Large_PrecinctNo = 5;
                ((Precinct)AllPrecincts[Large_PrecinctNo]).AddDRE();

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

            ((Precinct)AllPrecincts[precinct_no1]).RemoveDRE();
            ((Precinct)AllPrecincts[precinct_no2]).AddDRE();
            
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
