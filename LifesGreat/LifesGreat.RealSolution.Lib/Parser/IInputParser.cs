using LifesGreat.RealSolution.Lib.Job;

namespace LifesGreat.RealSolution.Lib.Parser
{
    public interface IInputParser<in T1, out T2>
    {
        T2 Parse(T1 inputString);
    }
}
