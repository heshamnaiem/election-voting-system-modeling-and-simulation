using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using React;
using React.Distribution;

namespace ElectionVotingSystem
{
    class DRE : Process  //direct-recording electronic machine
    {
        int capacity;
        public int Capacity
        {
            set
            {
                capacity = value;
            }

            get
            {
                return capacity;
            }
        }
        int ID;
        public int MachineID
        {
            set 
            {
                ID = value;
            }
            get
            {
                return ID;
            }
        }
        internal DRE(React.Simulation sim, string name)
            : base(sim)
        {
            this.Name = name;
        }



        // we need more investigation on this
        // Gamma scale parameter = 0.58 ~ 1.05 
        // Gamma Shape parameter = 5.71
        Gamma gamma = new Gamma(0.58, 5.71);

        protected override IEnumerator<Task> GetProcessSteps()
        {
            // get delay in Milliseconds
            int delay = (int)(gamma.NextSingle() * 60 * 1000);
            // starting delay
            yield return Delay(delay); 
            // end of delay
            yield break;
        }
        
    }
}
