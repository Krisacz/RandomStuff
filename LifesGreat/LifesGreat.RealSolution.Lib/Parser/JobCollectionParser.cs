using System;
using System.Linq;
using LifesGreat.RealSolution.Lib.Job;
using LifesGreat.RealSolution.Lib.Log;

namespace LifesGreat.RealSolution.Lib.Parser
{
    public class JobCollectionParser : IParser<JobCollection<JobObject>, string>
    {
        private readonly ILogger _logger;

        public JobCollectionParser(ILogger logger)
        {
            _logger = logger;
        }

        public string Parse(JobCollection<JobObject> input)
        {
            try
            {
                //Convert jobs list to string
                return input.Cast<JobObject>().Aggregate(string.Empty, (current, j) => current + j.Job);
            }
            catch (Exception ex)
            {
                _logger.AddError("JobCollectionValidator >> AreValid", ex);
            }

            return null;
        }
    }
}