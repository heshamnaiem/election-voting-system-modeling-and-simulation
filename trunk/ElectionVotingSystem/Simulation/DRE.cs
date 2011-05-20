using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using React;
using React.Distribution;

namespace ElectionVotingSystem
{
    class DRE  : Process //direct-recording electronic machine
    {
        //number of machines

        internal DRE(React.Simulation sim, int number)
            : base(sim)
        {
            this.Name = number.ToString();
        }

        // Gamma scale parameter = 0.58 ~ 1.05 
        // Gamma Shape parameter = 5.71
        Gamma gamma = new Gamma(1.05, 5.71);

        protected override IEnumerator<Task> GetProcessSteps()
        {
            // get delay in Milliseconds
            long  delay = (long)(gamma.NextDouble()); // * 60 * 1000);
            //Console.WriteLine("Delay = {0} ", delay);
            // starting delay
            yield return Delay(delay);
            // end of delay
            yield break;
        }
    }
}
