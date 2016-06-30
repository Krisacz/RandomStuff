namespace LifesGreat.RealSolution.Lib.Validation
{
    public interface IJobsValidator <in T>
    {
        bool AreValid(T jobs);
    }
}
