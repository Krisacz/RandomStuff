namespace LifesGreat.RealSolution.Lib.Parser
{
    public interface IParser<in T1, out T2>
    {
        T2 Parse(T1 input);
    }
}
