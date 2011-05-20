using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using React;

namespace ElectionVotingSystem
{
    class Voter : Process
    {
        // turnout rate - Normal - determin the turnout control creating the voters for each Precinct
        // arrival rate - Poisson distribution

        int PrecNumber;
        long CreationTime;
        long ActivationTime;
        long WaitingTime;
        internal static long MaxWaitingTime = -1;
        internal static int PrecinctNumber = -1; // with the Max Waiting Time 

        static public long GetMaxWaitingTimeVariable()
        {
            return MaxWaitingTime;
        }

        internal Voter(React.Simulation sim, int number, object PrecNumber)
            : base(sim)
        {
            this.Name = number.ToString();
            this.PrecNumber = (int)PrecNumber;
            this.CreationTime = this.Now;
            this.ActivationTime = -1;
            this.WaitingTime = 0;
            //Console.WriteLine("Creating Voter number {0} for Precinct {1} @ time {2}. ", this.Name,this.PrecNumber, this.CreationTime);
        }

        protected override IEnumerator<Task> GetProcessSteps()
        {
            
            Resource DREs = (Resource)ActivationData;
            yield return DREs.Acquire(this);

            this.ActivationTime = this.Now;
            //Console.WriteLine("Voter number {0} is Activated at time {1}", this.Name, this.ActivationTime);

            System.Diagnostics.Debug.Assert(DREs == Activator);
            System.Diagnostics.Debug.Assert(ActivationData != null);
            DRE dre = (DRE)ActivationData;

            yield return dre;

            WaitingTime = this.ActivationTime - this.CreationTime;
            //Console.WriteLine("Voter # {0} Used Machine # {1} for Voting in Precinct # {2} at time {3} .", this.Name, dre.Name, this.PrecNumber, this.Now);
            //Console.WriteLine("Voter # {0} is in the system for {1} and in the Queue for {2} ", Name, Now - CreationTime, WaitingTime);
            CalculateMaxWitingTime(WaitingTime);

            yield return DREs.Release(this);
        }

        protected void CalculateMaxWitingTime(long waitingtime)
        {
            if (MaxWaitingTime < waitingtime)
            {
                MaxWaitingTime = waitingtime;
                PrecinctNumber = this.PrecNumber;
                //Console.WriteLine("Max Waiting Time is {0} in Precinct number {1}  .", MaxWaitingTime, PrecinctNumber);
            }
        }
    }
}
