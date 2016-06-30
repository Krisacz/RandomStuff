using System;

namespace LifesGreat.RealSolution.Lib.Log
{
    public interface ILogger
    {
        void AddError(string error, Exception exception);
        void AddInfo(string info);
    }
}