using LifesGreat.RealSolution.Lib.Log;
using LifesGreat.RealSolution.Lib.Parser;
using LifesGreat.RealSolution.Lib.Sort;
using LifesGreat.RealSolution.Lib.Validation;

namespace LifesGreat.RealSolution.Lib.Flow
{
    public class FlowManager: IFlowManager<string, string>
    {
        private readonly ILogger _logger;

        public FlowManager(ILogger logger)
        {
            _logger = logger;
        }
        
        public string Process(string inputString)
        {
            //Validate input string
            var inputStringValidator = new InputStringValidator(_logger);
            if (!inputStringValidator.IsValid(inputString)) return null;
            
            //Parse input string
            var inputStringParser = new InputStringParser(_logger);
            var jobs = inputStringParser.Parse(inputString);
            if (jobs == null) return null;

            //Validate jobs collection
            var jobCollectionValidator = new JobCollectionValidator(_logger);
            if (!jobCollectionValidator.AreValid(jobs)) return null;

            //Sort jobs
            var jobCollectionSorter = new JobCollectionSorter(_logger);
            var sortedList = jobCollectionSorter.Sort(jobs);
            if (sortedList == null) return null;

            //Parse jobs collection to string
            var jobCollectionParser = new JobCollectionParser(_logger);
            var outputString = jobCollectionParser.Parse(sortedList);

            return outputString;
        }
    }
}
