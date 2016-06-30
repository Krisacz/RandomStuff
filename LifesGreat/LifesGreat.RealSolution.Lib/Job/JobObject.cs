namespace LifesGreat.RealSolution.Lib.Job
{
    public class JobObject
    {
        public string Job { get; }
        public string DependentJob { get; }

        public JobObject(string job, string dependentJob)
        {
            Job = job;
            DependentJob = dependentJob;
        }

        public bool DependentJobExist()
        {
            return !string.IsNullOrWhiteSpace(DependentJob);
        }


    }
}
