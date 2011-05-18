﻿using System;
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
        internal Voter(React.Simulation sim, int number, object PrecNumber)
            : base(sim)
        {
            this.Name = number.ToString();
            this.PrecNumber = (int)PrecNumber;
        }

        protected override IEnumerator<Task> GetProcessSteps()
        {
            Resource DREs = (Resource)ActivationData;
            yield return DREs.Acquire(this);

            System.Diagnostics.Debug.Assert(DREs == Activator);
            System.Diagnostics.Debug.Assert(ActivationData != null);
            DRE dre = (DRE)ActivationData;

            yield return dre;

            Console.WriteLine("Voter number {0} Used {1} for Voting in Precinct number {2} at time {3} .", this.Name, dre.Name, this.PrecNumber, this.Now);
            yield return DREs.Release(this);
        }

    }
}
