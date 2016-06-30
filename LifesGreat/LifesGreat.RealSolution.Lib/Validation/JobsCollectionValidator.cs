using LifesGreat.RealSolution.Lib.Job;

namespace LifesGreat.RealSolution.Lib.Validation
{
    public  class JobsCollectionValidator : IJobsValidator<JobCollection<JobObject>>
    {
        public bool AreValid(JobCollection<JobObject> jobs)
        {
            //Main loop will go through all tokens - all internal loops will perform individual validations
            foreach(JobObject j in jobs)
            {
                var job1 = j.Job;
                var dependentJob1 = j.DependentJob;
                var index1 = jobs.IndexOf(j);
                
                //Job defined multiple times
                if (tokens.Select(t => t.Item1).Where((job2, i2) => string.Equals(job1, job2) && i1 != i2).Any())
                {
                    return $"ERROR: Job {job1} defined multiple times.";
                }

                //Further validations are relying on dependent job being present so there is no point to carry on if there is none
                if (string.IsNullOrWhiteSpace(dependentJob1)) continue;

                #region JOB DEPENDS ON IT SELF

                if (string.Equals(job1, dependentJob1))
                    return $"ERROR: Job can not depends on itself. [{job1} > {dependentJob1}]";

                #endregion

                #region DEPENDENT JOB NOT DEFINED                

                var foundJob = tokens.Any(t2 => string.Equals(dependentJob1, t2.Item1));
                if (foundJob == false) return $"ERROR: Job {job1} depends on undefined {dependentJob1} job.";

                #endregion

                #region CIRCULAR DEPENDENCIES             

                var foundDependentJobAsJob = false;
                var searchingFor = dependentJob1;
                var exit = false;
                var visitedJobs = new List<string>();
                while (!foundDependentJobAsJob && !string.IsNullOrWhiteSpace(searchingFor) && !exit)
                {
                    var found = false;
                    foreach (var token in tokens)
                    {
                        if (!string.Equals(token.Item1, searchingFor)) continue;
                        visitedJobs.Add(searchingFor);
                        searchingFor = token.Item2;
                        found = true;
                        break;
                    }

                    if (visitedJobs.Any(x => x.Equals(searchingFor))) exit = true;
                    if (!found) exit = true;
                    if (searchingFor == job1) foundDependentJobAsJob = true;
                }
                if (foundDependentJobAsJob) return "$ERROR: Jobs can not have circular dependencies";

                #endregion
            }

            #endregion
        }
    }
}