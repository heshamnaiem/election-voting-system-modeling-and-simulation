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

        private const long ClosingTime = 1 * 60;
        public int Number;

        private int xi;   //Number of DRE Machines

        internal Precinct(int num)
        {
            this.Number = num;
            this.xi = 1;                //initial value
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


        public IEnumerator<Task> Generator(Process p, object data)
        {
            //Console.WriteLine("The Precinct Number {0} is opening for Voters." , data);
            Resource DREs = CreateDREs(xi);

            React.Distribution.Normal n = new Normal(5.0, 1.0);

            int index = 1;
            do
            {
                long d;
                do
                {
                    d = (long)n.NextDouble();
                }
                while (d <= 0L);

                yield return p.Delay(d);

                if (Now <= ClosingTime)
                {
                    Voter voter = new Voter(this, index, data);
                    voter.Activate(null, 0L, DREs);
                    index++;
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
                DREs[i] = new DRE(this, i + 1);
            }
            return Resource.Create(DREs);
        }

    }
}

