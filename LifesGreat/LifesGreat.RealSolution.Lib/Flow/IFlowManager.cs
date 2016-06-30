namespace LifesGreat.RealSolution.Lib.Flow
{
    public interface IFlowManager<in T1, out T2>
    {
        T2 Process(T1 input);
    }
}