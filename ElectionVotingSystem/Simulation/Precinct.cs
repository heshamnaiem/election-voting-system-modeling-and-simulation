using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using React;
using React.Distribution;

namespace ElectionVotingSystem
{
    class Precinct : React.Simulation
    {
        // the time in seconds
        // and the total voting time is 13 hours
        private const long ClosingTime = 13 * 60 * 60;
        private int Number;

        double gamma_scale_parameter;


        //Number of DRE Machines
        private int xi;  
        public int Xi
        {
            set { xi = value; }
            get { return xi; }
        }

        public int GetPrecinctNumber()
        {
            return Number;
        }

        internal Precinct(int num, double to_rate, double gScale)
        {
            this.Number = num;
            this.xi = 1;                //initial value
            this.turnOutRate = to_rate;
            this.gamma_scale_parameter = gScale;


            React.Distribution.Normal n = new Normal(1070, 319);
            numOfVoters = (int)n.NextDouble();
        }

        public void AddDRE()
        {
            this.xi++;
            //any nescessary code
        }

        public void RemoveDRE()
        {
            this.xi--;
            //any nescessary code
        }

        // turnout rate can be 0.72 and 0.56
        double turnOutRate;
        public double TurnOutRate
        {
            set { turnOutRate = value; }
            get { return turnOutRate; }
        }

        int numOfVoters = 10000;
        public int NumOfVoters
        {
            set { numOfVoters = value; }
            get { return numOfVoters; }
        }

        double startingTime = 6.5;
        double[] timeOfTheDay = new double[] { 8, 11, 15, 17, 19.5 };
        double[] rateAtTimeOfTheDay = new double[] { 0.2061, 0.2734, 0.2405, 0.1326, 0.1387 };

        public IEnumerator<Task> Generator(Process p, object data)
        {
            //Console.WriteLine("The Precinct Number {0} is opening for Voters." , data);
            Resource DREs = CreateDREs(xi);

            // index specifing the time of the day based on the time of the day table
            int timeIndex = 0;


            //React.Distribution.Normal n = new Normal(5.0, 1.0);
            double lambda = (rateAtTimeOfTheDay[0] * turnOutRate * numOfVoters) / ((timeOfTheDay[0] - startingTime) * 60 * 60);
            React.Distribution.Exponential ex = new Exponential(lambda);


            int index = 1;
            do
            {
                long d;
                //do
                //{
                //d = (long)n.NextDouble();
                d = (long)ex.NextDouble();
                //}
                //while (d <= 0L);

                yield return p.Delay(d);

                if (Now <= ClosingTime)
                {
                    Voter voter = new Voter(this, index, data);
                    voter.Activate(null, 0L, DREs);
                    index++;
                }

                // assuming time of the day is calculated in seconds
                if (Now < ClosingTime && (timeOfTheDay[timeIndex] - startingTime) * 60 * 60 < Now)
                {
                    // change the index and lambda of the generation of the interarrival times
                    timeIndex++;
                    lambda = (rateAtTimeOfTheDay[timeIndex] * turnOutRate * numOfVoters) / ((timeOfTheDay[timeIndex] - timeOfTheDay[timeIndex - 1]) * 60 * 60);
                    ex.Lambda = lambda;
                }

            }
            while (Now <= ClosingTime);

            //if (DREs.BlockCount > 0)
            //{
            //    Console.WriteLine("The Precinct Number {0} have to be open for a little bit longer {1}. ", data, DREs.BlockCount);
            //}
            //else
            //    Console.WriteLine("The Precinct number {0} is closed.", data);

            yield break;
        }


        private Resource CreateDREs(int xi)
        {
            // number of Machines/Precinct

            DRE[] DREs = new DRE[xi];
            for (int i = 0; i < xi; i++)
            {
                DREs[i] = new DRE(this, i + 1,gamma_scale_parameter);
            }
            return Resource.Create(DREs);
        }
    }
}
