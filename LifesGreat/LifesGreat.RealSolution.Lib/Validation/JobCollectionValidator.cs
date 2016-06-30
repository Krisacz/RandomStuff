using System;
using System.Collections.Generic;
using System.Linq;
using LifesGreat.RealSolution.Lib.Job;
using LifesGreat.RealSolution.Lib.Log;

namespace LifesGreat.RealSolution.Lib.Validation
{
    public class JobCollectionValidator : IJobsValidator<JobCollection<JobObject>>
    {
        private readonly ILogger _logger;

        public JobCollectionValidator(ILogger logger)
        {
            _logger = logger;
        }

        public bool AreValid(JobCollection<JobObject> jobs)
        {
            try
            {
                //Main loop will go through all "jobs" - all internal loops will perform individual validations
                foreach (JobObject j in jobs)
                {
                    var job = j.Job;
                    var dependentJob = j.DependentJob;
                    var index = jobs.IndexOf(j);

                    //Job defined multiple times
                    foreach (JobObject checkJ in jobs)
                    {
                        var checkJob = checkJ.Job;
                        var checkIndex = jobs.IndexOf(checkJ);
                        if (checkJob != job || checkIndex == index) continue;
                        _logger.AddError($"Job {job} defined multiple times.", null);
                        return false;
                    }

                    //Further validations are relying on dependent job being present so there is no point to carry on if there is none
                    if (!j.DependentJobExist()) continue;

                    //Job depends on itself
                    if (j.DependentJobExist() && string.Equals(job, dependentJob))
                    {
                        _logger.AddError($"Job can not depend on itself. [{job} > {dependentJob}]", null);
                        return false;
                    }

                    //Dependent job not defined
                    if (!jobs.Cast<JobObject>().Any(x => string.Equals(x.Job, dependentJob)))
                    {
                        _logger.AddError($"Job {job} depends on undefined {dependentJob} job.", null);
                        return false;
                    }

                    //Circular dependencies
                    var foundDependentJobAsJob = false;
                    var searchingFor = dependentJob;
                    var exit = false;
                    var visitedJobs = new List<string>();
                    while (!foundDependentJobAsJob && !string.IsNullOrWhiteSpace(searchingFor) && !exit)
                    {
                        var found = false;
                        foreach (JobObject x in jobs)
                        {
                            if (!string.Equals(x.Job, searchingFor)) continue;
                            visitedJobs.Add(searchingFor);
                            searchingFor = x.DependentJob;
                            found = true;
                            break;
                        }

                        if (visitedJobs.Any(x => x.Equals(searchingFor))) exit = true;
                        if (!found) exit = true;
                        if (searchingFor == job) foundDependentJobAsJob = true;
                    }
                    if (!foundDependentJobAsJob) continue;
                    _logger.AddError("Jobs can not have circular dependencies", null);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.AddError("JobCollectionValidator >> AreValid", ex);
            }

            return false;
        }
    }
}