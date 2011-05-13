using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using React;

namespace ElectionVotingSystem.Simulation
{
    class Voter : Process
    { 
        // turnout rate - Normal - determin the turnout - control creating the voters for each Precinct
        // arrival rate - Poisson distribution 
        internal Voter(React.Simulation sim)
            : base(sim)
        {
        }

        protected override IEnumerator<Task> GetProcessSteps()
        {
            Resource DREs = (Resource)ActivationData;
            yield return DREs.Acquire(this);

            System.Diagnostics.Debug.Assert(DREs == Activator);
            System.Diagnostics.Debug.Assert(ActivationData != null);
            DRE dre = (DRE)ActivationData;

            yield return dre;

            yield return DREs.Release(this);
        }

    }
}
