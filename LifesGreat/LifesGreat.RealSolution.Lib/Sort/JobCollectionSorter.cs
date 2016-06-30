using System;
using LifesGreat.RealSolution.Lib.Job;
using LifesGreat.RealSolution.Lib.Log;

namespace LifesGreat.RealSolution.Lib.Sort
{
    public class JobCollectionSorter : IJobSorter<JobCollection<JobObject>>
    {
        private readonly ILogger _logger;

        public JobCollectionSorter(ILogger logger)
        {
            _logger = logger;
        }

        public JobCollection<JobObject> Sort(JobCollection<JobObject> jobs)
        {
            try
            {
                //Mirror jobs list - this is the one we will be modifying as we can not modify an array while we are looping through it
                var sortedJobs = new JobCollection<JobObject>(jobs.GetCopy());

                //We are processing all job until all are in correct place
                var correct = false;
                while (!correct)
                {
                    var changeMade = false;
                    foreach (JobObject j in jobs)
                    {
                        var job = j.Job;
                        var dependentJob = j.DependentJob;

                        //If job does't have dependent job check if any other job depends on it and place it before it (lowest possible index)
                        if (string.IsNullOrWhiteSpace(dependentJob))
                        {
                            var jobIndex = sortedJobs.IndexOf(j);

                            //Initialize with maximum index
                            var minimumIndex = jobs.Count() - 1;
                            foreach (JobObject x in sortedJobs)
                            {
                                var dependentJob1 = x.DependentJob;
                                var i = sortedJobs.IndexOf(x);
                                if (string.Equals(job, dependentJob1)) minimumIndex = Math.Min(minimumIndex, i);
                            }

                            if (minimumIndex >= jobIndex) continue;
                            sortedJobs.RemoveAt(jobIndex);
                            sortedJobs.Insert(minimumIndex, j);
                            changeMade = true;
                        }
                        //...otherwise place current job after job it depends on
                        else
                        {
                            var jobIndex = sortedJobs.IndexOf(j);

                            //Initialize with minimum index
                            var maximumIndex = 0;
                            foreach (JobObject x in sortedJobs)
                            {
                                var job2 = x.Job;
                                var i = sortedJobs.IndexOf(x);
                                if (string.Equals(dependentJob, job2)) maximumIndex = Math.Max(maximumIndex, i);
                            }

                            if (maximumIndex <= jobIndex) continue;
                            sortedJobs.RemoveAt(jobIndex);
                            sortedJobs.Insert(maximumIndex, j);
                            changeMade = true;
                        }
                    }

                    correct = !changeMade;
                }

                return sortedJobs;
            }
            catch (Exception ex)
            {
                _logger.AddError("JobCollectionSorter >> Sort", ex);
            }

            return null;
        }
    }
}