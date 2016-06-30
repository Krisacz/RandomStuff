namespace LifesGreat.RealSolution.Lib.Validation
{
    public interface IInputValidator <in T>
    {
        bool IsValid(T input);
    }
}