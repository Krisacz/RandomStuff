namespace LifesGreat.RealSolution.Lib.Job
{
    public class JobObject
    {
        public string Job { get; private set; }
        public string DependentJob { get; private set; }

        public JobObject(string job, string dependentJob)
        {
            Job = job;
            DependentJob = dependentJob;
        }
    }
}
