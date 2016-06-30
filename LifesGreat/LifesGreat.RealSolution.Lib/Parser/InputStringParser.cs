using System;
using LifesGreat.RealSolution.Lib.Job;
using LifesGreat.RealSolution.Lib.Log;

namespace LifesGreat.RealSolution.Lib.Parser
{
    public class InputStringParser : IParser<string, JobCollection<JobObject>>
    {
        private readonly ILogger _logger;

        public InputStringParser(ILogger logger)
        {
            _logger = logger;
        }

        public JobCollection<JobObject> Parse(string input)
        {
            try
            {
                var jobs = new JobCollection<JobObject>();
                var skipChar = 0;

                //Loop through all character in the inputString 
                for (var i = 0; i < input.Length; i++)
                {
                    //Skip character if we've added dependent job in previous loop cycle(s)
                    if (skipChar > 0)
                    {
                        skipChar--;
                        continue;
                    }

                    //Add new token - job and it's dependent job (if any)
                    var job = input[i].ToString();
                    var dependentJob = string.Empty;

                    //Check if further down the string there is current's job dependency                
                    if (i + 1 < input.Length - 1)
                    {
                        if (input[i + 1] == '>')
                        {
                            dependentJob = input[i + 2].ToString();
                            skipChar = 2;
                        }
                    }

                    //Create new job+dependent job
                    var jobObj = new JobObject(job, dependentJob);
                    jobs.Add(jobObj);
                }

                return jobs;
            }
            catch (Exception ex)
            {
                _logger.AddError("InputStringParser >> Parse", ex);
            }

            return null;
        }
    }
}