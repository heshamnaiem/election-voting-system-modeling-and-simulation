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

        internal Precinct(int num)
        {
            this.Number = num;
        }


        public IEnumerator<Task> Generator(Process p, object data)
        {
            Console.WriteLine("The Precinct Number {0} is opening for Voters." , data);
            Resource DREs = CreateDREs();

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

                Voter voter = new Voter(this,index, data);
                voter.Activate(null, 0L, DREs);
                index++;
            }
            while (Now <= ClosingTime);


            if (DREs.BlockCount > 0)
            {
                Console.WriteLine("The Precinct Number {0} have to be open for a little bit longer {1}. ", data, DREs.BlockCount);
            }
            else
                Console.WriteLine("The Precinct number {0} is closed.", data);

            yield break;


        }


        private Resource CreateDREs()
        {
	     // number of Machines/Precinct
            DRE[] DREs = new DRE[2];
            DREs[0] = new DRE(this, "Machine # 1");
            DREs[1] = new DRE(this, "Machine # 2");

            return Resource.Create(DREs);

        }
    }
}

